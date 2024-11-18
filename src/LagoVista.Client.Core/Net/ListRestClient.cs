using System;
using System.Threading;
using System.Threading.Tasks;
using LagoVista.Core.Models.UIMetaData;

namespace LagoVista.Client.Core.Net
{
    public class ListRestClient<TSummaryModel> : IListRestClient<TSummaryModel> where TSummaryModel : class
    {
        IRestClient _rawRestClient;

        public ListRestClient(IRestClient rawRestClient)
        {
            _rawRestClient = rawRestClient;
        }

        public async Task<ListResponse<TSummaryModel>> GetForOrgAsync(string path, CancellationTokenSource cancellationTokenSource = null)
        {
            if (cancellationTokenSource == null) cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            var response = await _rawRestClient.GetAsync(path, cancellationTokenSource);
            return response.ToListResponse<TSummaryModel>();
        }
    }
}