using LagoVista.Client.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LagoVista.Client.Core
{
    /* 
         * Methods to encapsulate making authenticated calls with refresh tokens to the server,
         * only working with string payloads (if applicable) and returning string.
         * calling methods are responsible for serialization/deserialization
         * should only really be used with relatively small chunks of data,
         * will need a different mechanism for uploading pictures.
         */
    public interface IRestClient
    {
        event EventHandler BeginCall;
        event EventHandler EndCall;

        Task<InvokeResult> RenewRefreshToken();
        Task<RawResponse> GetAsync(String path, CancellationTokenSource tokenSource = null, bool waitCursor = true);
        Task<RawResponse> PostAsync(String path, String payload, CancellationTokenSource tokenSource = null, bool waitCursor = true);
        Task<RawResponse> PutAsync(String path, String payload, CancellationTokenSource tokenSource = null, bool waitCursor = true);
        Task<RawResponse> DeleteAsync(String path, CancellationTokenSource tokenSource = null, bool waitCursor = true);

        Task<InvokeResult> PostAsync<TModel>(String path, TModel model, CancellationTokenSource cancellationTokenSource = null, bool waitCursor = true) where TModel : class;
        Task<InvokeResult> PutAsync<TModel>(String path, TModel model, CancellationTokenSource cancellationTokenSource = null, bool waitCursor = true) where TModel : class;

        Task<InvokeResult<TResponseModel>> PostAsync<TModel, TResponseModel>(String path, TModel model, CancellationTokenSource cancellationTokenSource = null, bool waitCursor = true) where TModel : class where TResponseModel : class;

        Task<InvokeResult<TResponseModel>> GetAsync<TResponseModel>(String path, CancellationTokenSource cancellationTokenSource = null, bool waitCursor = true) where TResponseModel : class;
        Task<ListResponse<TResponseModel>> GetListResponseAsync<TResponseModel>(String path, ListRequest listRequest = null, CancellationTokenSource cancellationTokenSource = null, bool waitCursor = true) where TResponseModel : class;
    }
}
