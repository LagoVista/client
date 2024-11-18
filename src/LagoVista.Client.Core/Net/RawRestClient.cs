using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.PlatformSupport;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LagoVista.Core.Interfaces;
using LagoVista.Client.Core.Models;
using System.Net.Http.Headers;
using System.Text;
using LagoVista.Core.Validation;
using LagoVista.Core.Authentication.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using LagoVista.Client.Core.Resources;
using LagoVista.Core.Models.UIMetaData;
using System.Collections.Generic;
using LagoVista.Core.Models;
using LagoVista.UserAdmin.Interfaces;

namespace LagoVista.Client.Core.Net
{
    /*
     * 100% of all authenticated calls to the server with go through this singleton, idea is that if
     * a refresh token is upodated and two calls happen at the same time with the same refresh token
     * only one will succeed and the user will be locked out since the refresh tokens are single use.
     */
    public class RawRestClient : IRestClient
    {
        readonly HttpClient _httpClient;
        readonly IAuthManager _authManager;
        readonly ILogger _logger;
        readonly IAuthClient _authClient;
        readonly IDeviceInfo _deviceInfo;
        readonly IAppConfig _appConfig;
        readonly IStorageService _storageService;
        readonly INetworkService _networkService;
        readonly SemaphoreSlim _callSemaphore;

        public RawRestClient(HttpClient httpClient, INetworkService networkService, IDeviceInfo deviceInfo, IStorageService storageService,
                    IAppConfig appConfig, IAuthClient authClient, IAuthManager authManager, ILogger logger)
        {
            _httpClient = httpClient;
            _authClient = authClient;
            _deviceInfo = deviceInfo;
            _authManager = authManager;
            _storageService = storageService;
            _networkService = networkService;
            _logger = logger;
            _appConfig = appConfig;
            _callSemaphore = new SemaphoreSlim(1);
        }

        public async Task<InvokeResult> RenewRefreshToken()
        {
            var authRequest = new AuthRequest();
            authRequest.AppId = _appConfig.AppId;
            authRequest.ClientType = "mobileapp";
            authRequest.DeviceId = _deviceInfo.DeviceUniqueId;
            authRequest.AppInstanceId = _authManager.AppInstanceId;
            authRequest.GrantType = "refreshtoken";
            authRequest.UserName = _authManager.User.Email;
            authRequest.Email = _authManager.User.Email;
            authRequest.RefreshToken = _authManager.RefreshToken;
            if(!EntityHeader.IsNullOrEmpty(_appConfig.SystemOwnerOrg))
            {
                authRequest.OrgId = _appConfig.SystemOwnerOrg.Id;
                authRequest.OrgName = _appConfig.SystemOwnerOrg.Text;
            }

            var response = await _authClient.LoginAsync(authRequest);
            if (response.Successful)
            {
                _authManager.AccessToken = response.Result.AccessToken;
                _authManager.AccessTokenExpirationUTC = response.Result.AccessTokenExpiresUTC;
                _authManager.RefreshToken = response.Result.RefreshToken;
                _authManager.AppInstanceId = response.Result.AppInstanceId;
                _authManager.RefreshTokenExpirationUTC = response.Result.RefreshTokenExpiresUTC;
                _logger.AddCustomEvent(LogLevel.Message, "[RawRestClient_RenewRefreshTokenAsync]", "Access Token Renewed with Refresh Token");
                await _authManager.PersistAsync();
                return InvokeResult.Success;
            }
            else
            {
                _logger.AddCustomEvent(LogLevel.Error, "[RawRestClient_RenewRefreshTokenAsync]", "Could Not Renew Access Token", response.ErrorsToKVPArray());
                var result = new InvokeResult();
                result.Concat(response);
                throw new Exceptions.CouldNotRenewTokenException();
            }
        }

        private async Task<RawResponse> PerformCall(Func<Task<HttpResponseMessage>> call, CancellationTokenSource cancellationTokenSource = null, ListRequest listRequest = null)
        {
            if (!_networkService.IsInternetConnected)
            {
                return RawResponse.FromNotConnected();
            }

            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            await _callSemaphore.WaitAsync();
            var retry = true;
            var attempts = 0;
            var rawResponse = RawResponse.FromNotCompleted();

            while (retry)
            {
                _httpClient.DefaultRequestHeaders.Clear();
                switch (_appConfig.AuthType)
                {
                    case AuthTypes.ClientApp:
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("APIToken", $"{_appConfig.AppId}:{_appConfig.APIToken}");

                        break;
                    default:
                        if (_authManager.IsAuthenticated)
                        {
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authManager.AccessToken);
                        }

                        break;
                }

                if (listRequest != null)
                {
                    if (!String.IsNullOrEmpty(listRequest.NextPartitionKey)) _httpClient.DefaultRequestHeaders.Add("x-nextpartitionkey", listRequest.NextPartitionKey);
                    if (!String.IsNullOrEmpty(listRequest.NextRowKey)) _httpClient.DefaultRequestHeaders.Add("x-nextrowkey", listRequest.NextRowKey);
                    _httpClient.DefaultRequestHeaders.Add("x-pageindex", listRequest.PageIndex.ToString());
                    _httpClient.DefaultRequestHeaders.Add("x-pagesize", Math.Max(50, listRequest.PageSize).ToString());
                    if (!String.IsNullOrEmpty(listRequest.StartDate)) _httpClient.DefaultRequestHeaders.Add("x-filter-startdate", listRequest.StartDate);
                    if (!String.IsNullOrEmpty(listRequest.EndDate)) _httpClient.DefaultRequestHeaders.Add("x-filter-enddate", listRequest.EndDate);
                }

                retry = false;
                try
                {

                    _logger.AddCustomEvent(LogLevel.Message, "[RawResetClient_PerformCall]", "Begin call");
                    var start = DateTime.Now;
                    var response = await call();
                    var delta = DateTime.Now - start;
                    
                    if (response.IsSuccessStatusCode)
                    {
                        _logger.AddCustomEvent(LogLevel.Message, "[RawResetClient_PerformCall]", "Call Success", delta.ToString().ToKVP("time"));
                        rawResponse = RawResponse.FromSuccess(await response.Content.ReadAsStringAsync());
                        delta = DateTime.Now - start;
                        _logger.AddCustomEvent(LogLevel.Message, "[RawResetClient_PerformCall]", "Got RAW Response", delta.ToString().ToKVP("time"));
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        _logger.AddCustomEvent(LogLevel.Message, "]RawResetClient_PerformCall]", "Call Unauthorized");
                        _logger.AddCustomEvent(LogLevel.Error, "[RawRestClient_PerformCall]", "401 From Server");
                        retry = ((await RenewRefreshToken()).Successful);
                        if (!retry)
                        {
                            rawResponse = RawResponse.FromNotAuthorized();
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                    {

                        await Task.Delay(attempts * 100);
                        retry = attempts++ < 5;
                        _logger.AddCustomEvent(LogLevel.Message, "[RawResetClient_PerformCall]", $"Bad Gateway {attempts} will retry {retry}");
                    }
                    else
                    {
                        _logger.AddCustomEvent(LogLevel.Message, "[RawRestClient_PerformCall]", $"Http Error {(int)response.StatusCode}");
                        /* Check for 401 (I think, if so then attempt to get a new access token,  */
                        rawResponse = RawResponse.FromHttpFault((int)response.StatusCode, $"{ClientResources.Err_GeneralErrorCallingServer} : HTTP{(int)response.StatusCode} - {response.ReasonPhrase}");
                    }

                }
                catch (Exceptions.CouldNotRenewTokenException ex)
                {
                    _logger.AddCustomEvent(LogLevel.Message, "[RawResetClient_PerformCall]", $"Could Not Renew from Refreh Token {attempts} will not retry");
                    _logger.AddException("[RawResetClient_PerformCall[", ex, ex.Message.ToKVP("type"));
                    _callSemaphore.Release();
                    throw;
                }
                catch (TaskCanceledException tce)
                {
                    _logger.AddException("[RawRestClient_PerformCall_TaskCancelled]", tce, tce.Message.ToKVP("type"));
                    rawResponse = RawResponse.FromException(tce, tce.CancellationToken.IsCancellationRequested);
                }
                catch (Exception ex)
                {
                    _logger.AddException("[RawRestClient_PerformCall]", ex, ex.Message.ToKVP("type"));
                    rawResponse = RawResponse.FromException(ex);
                }
            }

            _callSemaphore.Release();

            return rawResponse;
        }

        private Dictionary<string, string> _offlineCache = default;

        private async Task<String> GetCachedRequestAsync(string path)
        {
            if (_offlineCache == null)
            {
                _offlineCache = await _storageService.GetAsync<Dictionary<string, string>>("OfflineCache.json");
                if (_offlineCache == null)
                    _offlineCache = new Dictionary<string, string>();
            }

            if (_offlineCache.ContainsKey(path))
            {
                return _offlineCache[path];
            }
            else
            {
                return null;
            }
        }

        private async Task AddCachedResponseAsync(string path, RawResponse response)
        {
            if (response.Success)
            {
                if (_offlineCache == null)
                {
                    _offlineCache = await _storageService.GetAsync<Dictionary<string, string>>("OfflineCache.json");
                    if (_offlineCache == null)
                        _offlineCache = new Dictionary<string, string>();
                }

                if (_offlineCache.ContainsKey(path))
                {
                    _offlineCache.Remove(path);
                }

                _offlineCache.Add(path, response.Content);
                await _storageService.StoreAsync(_offlineCache, "OfflineCache.json");
            }
        }

        public async Task<RawResponse> GetAsync(string path, CancellationTokenSource cancellationTokenSource = null)
        {
            if (!_networkService.IsInternetConnected)
            {
                var cachedResponse = await GetCachedRequestAsync(path);
                if (cachedResponse != null)
                {
                    return RawResponse.FromSuccess(cachedResponse);
                }
            }

            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            _logger.AddCustomEvent(LogLevel.Message, "RawRestClient_GetAsync", "Begin GET", path.ToKVP("path"));

            var response = await PerformCall(async () =>
            {
                var timedEvent = _logger.StartTimedEvent("RawRestClient_Get", path);
                var result = await _httpClient.GetAsync(path, cancellationTokenSource.Token);
                _logger.EndTimedEvent(timedEvent);
                return result;
            }, cancellationTokenSource);

            if (response.Success)
            {
                await AddCachedResponseAsync(path, response);
            }
            return response;
        }

        public Task<RawResponse> PostAsync(string path, string payload, CancellationTokenSource cancellationTokenSource = null)
        {
            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            _logger.AddCustomEvent(LogLevel.Message, "RawRestClient_GetAsync", "Begin POST", path.ToKVP("path"), payload.ToKVP("content"));

            return PerformCall(async () =>
            {
                var timedEvent = _logger.StartTimedEvent("RawRestClient_Post", path);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var result = await _httpClient.PostAsync(path, content, cancellationTokenSource.Token);
                _logger.EndTimedEvent(timedEvent);
                return result;
            }, cancellationTokenSource);
        }

        public Task<RawResponse> PutAsync(string path, string payload, CancellationTokenSource cancellationTokenSource = null)
        {
            _logger.AddCustomEvent(LogLevel.Message, "RawRestClient_GetAsync", "Begin PUT", path.ToKVP("path"), payload.ToKVP("content"));

            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            return PerformCall(async () =>
            {
                var timedEvent = _logger.StartTimedEvent("RawRestClient_Put", path);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var result = await _httpClient.PutAsync(path, content, cancellationTokenSource.Token);
                _logger.EndTimedEvent(timedEvent);
                return result;
            }, cancellationTokenSource);
        }

        public Task<RawResponse> DeleteAsync(string path, CancellationTokenSource cancellationTokenSource = null)
        {
            _logger.AddCustomEvent(LogLevel.Message, "RawRestClient_GetAsync", "Begin Delete", path.ToKVP("path"));

            return PerformCall(async () =>
            {
                if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));

                var timedEvent = _logger.StartTimedEvent("RawRestClient_DeleteAsync", path);
                var result = await _httpClient.DeleteAsync(path, cancellationTokenSource.Token);
                _logger.EndTimedEvent(timedEvent);
                return result;
            }, cancellationTokenSource);
        }

        public async Task<InvokeResult> PostAsync<TModel>(string path, TModel model, CancellationTokenSource cancellationTokenSource = null) where TModel : class
        {
            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            var json = JsonConvert.SerializeObject(model, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver(), });

            _logger.AddCustomEvent(LogLevel.Message, "RawRestClient_PostAsync", "Begin Post", path.ToKVP("path"), json.ToKVP("content"));

            var timedEvent = _logger.StartTimedEvent("RawRestClient_DeleteAsync", path);
            var response = await PostAsync(path, json, cancellationTokenSource);
            _logger.EndTimedEvent(timedEvent);
            return response.ToInvokeResult();
        }

        public async Task<InvokeResult> PutAsync<TModel>(string path, TModel model, CancellationTokenSource cancellationTokenSource = null) where TModel : class
        {
            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            var json = JsonConvert.SerializeObject(model, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver(), });

            var timedEvent = _logger.StartTimedEvent("RawRestClient_DeleteAsync", path);
            _logger.AddCustomEvent(LogLevel.Message, "RawRestClient_PutAsync", "Begin PUT", path.ToKVP("path"), json.ToKVP("content"));
            var response = await PutAsync(path, json, cancellationTokenSource);
            _logger.EndTimedEvent(timedEvent);
            return response.ToInvokeResult();
        }

        public async Task<InvokeResult<TResponseModel>> PostAsync<TModel, TResponseModel>(string path, TModel model, CancellationTokenSource cancellationTokenSource = null) where TModel : class where TResponseModel : class
        {
            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            var json = JsonConvert.SerializeObject(model, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver(), });
            _logger.AddCustomEvent(LogLevel.Message, "RawRestClient_PostAsync", "Begin POST", path.ToKVP("path"), json.ToKVP("content"));

            var response = await PostAsync(path, json, cancellationTokenSource);
            if (response.Success)
            {
                return JsonConvert.DeserializeObject<InvokeResult<TResponseModel>>(response.Content);
            }
            else
            {
                return response.ToInvokeResult<TResponseModel>();
            }
        }

        public async Task<InvokeResult<TResponseModel>> GetAsync<TResponseModel>(string path, CancellationTokenSource cancellationTokenSource = null) where TResponseModel : class
        {
            if (!_networkService.IsInternetConnected)
            {
                var cachedResponse = await GetCachedRequestAsync(path);
                if (cachedResponse != null)
                {
                    return InvokeResult<TResponseModel>.Create( JsonConvert.DeserializeObject<TResponseModel>(cachedResponse));
                }
            }

            _logger.AddCustomEvent(LogLevel.Message, "RawRestClient_GetAsync", "Begin GET", path.ToKVP("path"));

            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            var response = await GetAsync(path, cancellationTokenSource);

            await AddCachedResponseAsync(path, response);

            return response.ToInvokeResult<TResponseModel>();
        }

        public async Task<ListResponse<TResponseModel>> GetListResponseAsync<TResponseModel>(string path, ListRequest listRequest, CancellationTokenSource cancellationTokenSource = null) where TResponseModel : class
        {
            if (!_networkService.IsInternetConnected)
            {
                var cachedResponse = await GetCachedRequestAsync(path);
                if (cachedResponse != null)
                {
                    return JsonConvert.DeserializeObject<ListResponse<TResponseModel>>(cachedResponse);
                }
            }

            _logger.AddCustomEvent(LogLevel.Message, "RawRestClient_GetListResponseAsync", "Begin Get", path.ToKVP("path"));

            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            var response = await PerformCall(async () =>
            {
                var timedEvent = _logger.StartTimedEvent("RawRestClient_Get", path);
                var httpResponse = await _httpClient.GetAsync(path);
                _logger.EndTimedEvent(timedEvent);
                return httpResponse;

            }, cancellationTokenSource, listRequest);

            await AddCachedResponseAsync(path, response);

            return response.ToListResponse<TResponseModel>();
        }
    }
}
