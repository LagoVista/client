using LagoVista.Core.Models;
using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.Networking.Models;
using LagoVista.Core.PlatformSupport;
using System;
using System.Threading;
using System.Threading.Tasks;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LagoVista.Client.Core.Net
{

    public class FormRestClient<TModel> : IFormRestClient<TModel> where TModel : new()
    {
        const int CALL_TIMEOUT_SECONDS = 60;
        IRestClient _rawRestClient;

        public FormRestClient(IRestClient rawRestClient)
        {
            _rawRestClient = rawRestClient;
        }

        public async Task<InvokeResult> AddAsync(string path, TModel model, CancellationTokenSource cancellationTokenSource = null)
        {
            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            var json = JsonConvert.SerializeObject(model, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver(), });
            var response = await _rawRestClient.PostAsync(path, json, cancellationTokenSource);
            return response.ToInvokeResult();
        }
        
        public Task<InvokeResult<DetailResponse<TModel>>> CreateNewAsync(string path, CancellationTokenSource cancellationTokenSource = null)
        {
            return GetAsync(path, cancellationTokenSource);
        }

        public Task<InvokeResult> DeleteAsync(string path, TModel model, CancellationTokenSource cancellationTokenSource = null)
        {
            throw new NotImplementedException();
        }

        public async Task<InvokeResult<DetailResponse<TModel>>> GetAsync(string path, CancellationTokenSource cancellationTokenSource = null)
        {
            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            var response = await _rawRestClient.GetAsync(path, cancellationTokenSource);
            if (response.Success)
            {
                return InvokeResult<DetailResponse<TModel>>.Create(response.ToDetailResponse<TModel>());
            }
            else
            {
                return InvokeResult<DetailResponse<TModel>>.FromInvokeResult(response.ToInvokeResult());
            }
        }


        public async Task<InvokeResult> UpdateAsync(String path, TModel model, CancellationTokenSource cancellationTokenSource = null)
        {
            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15));

            var json = JsonConvert.SerializeObject(model, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver(), });
            var response = await _rawRestClient.PutAsync(path, json, cancellationTokenSource);
            return response.ToInvokeResult();
        }
    }
}
