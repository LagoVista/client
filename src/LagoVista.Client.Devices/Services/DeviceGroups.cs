using LagoVista.Client.Core;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Client.Devices.Services
{
    public class DeviceGroups
    {
        IRestClient _restClient;
        public DeviceGroups(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public Task<ListResponse<DeviceGroupSummary>> GetDeviceGroups(String deviceRepoId, ListRequest request = null)
        {
            return _restClient.GetListResponseAsync<DeviceGroupSummary>($"/api/repo/{deviceRepoId}/groups", request);
        }

        public Task<ListResponse<EntityHeader>> GetDevicesByDeviceGroupId(String deviceRepoId, String deviceGroupId, ListRequest request = null)
        {
            return _restClient.GetListResponseAsync<EntityHeader>($"/api/repo/{deviceRepoId}/group/{deviceGroupId}/devices", request);
        }

        public  Task<InvokeResult<DeviceGroupEntry>> AddDeviceToGroupAsync(string devicerepoid, String groupid, string deviceid, ListRequest request = null)
        {
            return  _restClient.GetAsync< DeviceGroupEntry>("/api/repo/{devicerepoid}/group/{groupid}/add/{deviceid}");
        }

        public async Task<InvokeResult> RemoveDeviceToGroupAsync(string devicerepoid, String groupid, string deviceid, ListRequest request = null)
        {
            return (await _restClient.DeleteAsync($"/api/repo/{devicerepoid}/group/{groupid}/remove/{deviceid}")).ToInvokeResult();
        }

        public Task<ListResponse<DeviceSummaryData>> GetGroupDevicesSummaryData(string devicerepoid, string groupid, ListRequest request = null)
        {
            return _restClient.GetListResponseAsync<DeviceSummaryData>($"/api/repo/{devicerepoid}/group/{groupid}/devices/summarydata", request);
        }
    }
}
