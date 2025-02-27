using LagoVista.Client.Core.Resources;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;

namespace LagoVista.Client.Core.Models
{
    public enum FaultTypes
    {
        Timeout,
        TokenError,
        NotConnectedError,
        Exception,
        None,
        HttpFault,
        Cancelled,
        NotAuthorized,
        NotCompleted,
    }

    public class RawResponse
    {

        public FaultTypes FaultType { get; protected set; }
        public String Content { get; protected set; }
        public bool Success { get; protected set; }
        public int HttpResponseCode { get; protected set; }
        public bool WasCancelled { get; protected set; }
        public String ErrorMessage { get; protected set; }

        public bool HasContent { get { return !String.IsNullOrEmpty(Content); } }

        public static RawResponse FromSuccess(String content)
        {
            return new RawResponse()
            {
                Success = true,
                Content = content,
                FaultType = FaultTypes.None
            };
        }

        public static RawResponse FromNotCompleted()
        {
            return new RawResponse()
            {
                Success = false,
                FaultType = FaultTypes.NotCompleted,
                ErrorMessage = ClientResources.Err_CallNotCompleted

            };
        }

        public static RawResponse FromNotConnected()
        {
            return new RawResponse()
            {
                Success = false,
                FaultType = FaultTypes.NotConnectedError,
                ErrorMessage = ClientResources.Err_NotConnected

            };
        }

        public static RawResponse FromTokenError()
        {
            return new RawResponse()
            {
                Success = false,
                FaultType = FaultTypes.TokenError,
                ErrorMessage = ClientResources.Err_TokenError,
            };
        }

        public static RawResponse FromException(Exception ex, bool cancelled = false)
        {
            var errMsg = ex.Message;
           if(ex.InnerException != null)
            {
                errMsg += $"\r{ex.InnerException.Message}";
            }

            return new RawResponse()
            {
                WasCancelled = cancelled,
                Success = false,
                ErrorMessage = errMsg,
                FaultType = cancelled ? FaultTypes.Cancelled : FaultTypes.Exception
            };
        }

        public static RawResponse FromNotAuthorized()
        {
            return new RawResponse()
            {
                Success = false,
                FaultType = FaultTypes.NotAuthorized,
                ErrorMessage = ClientResources.Err_NotAuthorized,
            };
        }


        public static RawResponse FromHttpFault(int code, String message)
        {
            return new RawResponse()
            {
                Success = false,
                FaultType = FaultTypes.HttpFault,
                ErrorMessage = message,
                HttpResponseCode = code
            };
        }

        public TModel DeserializeContent<TModel>()
        {
            if (!HasContent)
            {
                throw new InvalidOperationException("Attempt to deserilaized empty content.");
            }

            var response = JsonConvert.DeserializeObject<TModel>(Content, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver(), });
            return response;
        }

        public DetailResponse<TModel> ToDetailResponse<TModel>() where TModel : new()
        {
            return DeserializeContent<DetailResponse<TModel>>();
        }

        public ListResponse<TModel> ToListResponse<TModel>() where TModel : class
        {            
            if (!Success)
            {
                var result = new ListResponse<TModel>();
                result.Errors.Add(new LagoVista.Core.Validation.ErrorMessage(ErrorMessage));
                return result;
            }
            else
            {
                return DeserializeContent<ListResponse<TModel>>();
            }
        }

        public InvokeResult ToInvokeResult()
        {
            var result = new InvokeResult();
            if (!Success)
            {
                result.Errors.Add(new LagoVista.Core.Validation.ErrorMessage(ErrorMessage));
                return result;
            }
            else
            {
                return DeserializeContent<InvokeResult>();
            }
        }

        public InvokeResult<TModel> ToInvokeResult<TModel>() where TModel : class
        {
            var result = new InvokeResult<TModel>();

            if (!Success)
            {
                result.Errors.Add(new LagoVista.Core.Validation.ErrorMessage(ErrorMessage));
            }
            else
            {
                result.Result = DeserializeContent<TModel>();
            }
            return result;
        }

    }
}
