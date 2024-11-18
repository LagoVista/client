using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LagoVista.Client.Core
{
    public interface IListRestClient<TSummaryModel> where TSummaryModel : class
    {
        Task<ListResponse<TSummaryModel>> GetForOrgAsync(String path, CancellationTokenSource cancellationTokenSource = null);
    }
}