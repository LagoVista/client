using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using LagoVista.Core.Authentication.Models;
using LagoVista.Core.IOC;
using Newtonsoft.Json.Serialization;
using LagoVista.Core.Validation;
using LagoVista.UserAdmin.Models.Auth;
using LagoVista.UserAdmin.Interfaces;

namespace LagoVista.Client.Core.Auth
{
    public class AuthClient : IAuthClient
    {
        public AuthClient()
        {
            
        }

        public async Task<InvokeResult<AuthResponse>> LoginAsync(AuthRequest loginInfo, CancellationTokenSource cancellationTokenSource = null)
        {
            var client = SLWIOC.Get<HttpClient>();

            var json =  JsonConvert.SerializeObject(loginInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync("/api/v1/auth", content);
                if (response.IsSuccessStatusCode)
                {
                    var resultContent = await response.Content.ReadAsStringAsync();
                    var serializerSettings = new JsonSerializerSettings();
                    serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    var authResponse = JsonConvert.DeserializeObject<InvokeResult<AuthResponse>>(resultContent, serializerSettings);

                    return authResponse;                    
                }
                else
                {
                    return InvokeResult<AuthResponse>.FromErrors(new ErrorMessage() { Message = response.ReasonPhrase });
                }
            }
            catch(Exception ex)
            {
                return InvokeResult<AuthResponse>.FromException("AuthClient_LoginAsync", ex);
            }
        }

        public Task<InvokeResult> ResetPasswordAsync(string emailAddress, CancellationTokenSource cancellationTokenSource = null)
        {
            throw new NotImplementedException();
        }
    }
}
