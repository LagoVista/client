using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LagoVista.Client.Core
{
    public interface IFormRestClient<TModel> where TModel : new()
    {
        Task<InvokeResult<DetailResponse<TModel>>> CreateNewAsync(String path, CancellationTokenSource cancellationTokenSource = null);

        Task<InvokeResult> AddAsync(String path, TModel model, CancellationTokenSource cancellationTokenSource = null);
        Task<InvokeResult> UpdateAsync(String path, TModel model, CancellationTokenSource cancellationTokenSource = null);

        Task<InvokeResult<DetailResponse<TModel>>> GetAsync(String path, CancellationTokenSource cancellationTokenSource = null);

        Task<InvokeResult> DeleteAsync(String path, TModel model, CancellationTokenSource cancellationTokenSource = null);
    }
}