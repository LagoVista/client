using LagoVista.Client.Core;
using LagoVista.Client.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.IoT.Deployment.Admin.Models;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.DeviceManagement;
using LagoVista.IoT.DeviceManagement.Core.Models;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.UserAdmin.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LagoVista.Client.Devices
{
    public class DeviceManagementClient : IDeviceManagementClient
    {

        private readonly IRestClient _restClient;
        public DeviceManagementClient(IRestClient restClient, LagoVista.Core.PlatformSupport.IStorageService storageService)
        {
            _restClient = restClient;
        }

        public Task<ListResponse<DeviceTypeSummary>> GetDeviceTypesAsync(ListRequest listRequest = null)
        {
            return _restClient.GetListResponseAsync<DeviceTypeSummary>("/api/devicetypes", listRequest);
        }

        public Task<ListResponse<DeviceConfigurationSummary>> GetDeviceConfigsAsync(ListRequest listRequest = null)
        {
            return _restClient.GetListResponseAsync<DeviceConfigurationSummary>("/api/deviceconfigs", listRequest);
        }

        public Task<ListResponse<DeviceRepositorySummary>> GetDeviceReposAsync(ListRequest listRequest = null)
        {
            return _restClient.GetListResponseAsync<DeviceRepositorySummary>("/api/devicerepos", listRequest);
        }

        public Task<InvokeResult> AddDeviceAsync(String deviceRepoId, Device device)
        {
            return _restClient.PostAsync($"/api/device/{deviceRepoId}", device);
        }

        public async Task<InvokeResult<ListenerConfiguration>> GetListenerConfigurationAsync(String instanceId)
        {
            var result = await _restClient.GetAsync<InvokeResult<ListenerConfiguration>>($"/api/deployment/instance/{instanceId}/defaultlistener");
            return result.Result;
        }

        public Task<ListResponse<DeviceTypeSummary>> GetDeviceTypesForInstanceAsync(String instanceId, ListRequest request = null)
        {
            return _restClient.GetListResponseAsync<DeviceTypeSummary>($"/api/deployment/instance/{instanceId}/devicetypes", request);
        }

        public Task<InvokeResult> UpdateDeviceAsync(String deviceRepoId, Device device)
        {
            return _restClient.PutAsync($"/api/device/{deviceRepoId}", device);
        }

        public async Task<DetailResponse<Device>> GetDeviceAsync(String deviceRepoId, String deviceId)
        {
            var result = await _restClient.GetAsync<DetailResponse<Device>>($"/api/device/{deviceRepoId}/{deviceId}");
            if (result.Successful)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public async Task<InvokeResult<Device>> GetDeviceByMacAddressAsync(string deviceRepoId, string macAddress)
        {
            var url = $"/api/device/{deviceRepoId}/macaddress/{WebUtility.UrlEncode(macAddress)}";

            var result = await _restClient.GetAsync<InvokeResult<Device>>(url);
            return result.Result;
        }

        public async Task<DetailResponse<Device>> GetDeviceAsync(String deviceId)
        {
            var result = await _restClient.GetAsync<DetailResponse<Device>>($"/clientapi/device/{deviceId}");
            if (result.Successful)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public Task<ListResponse<ClientAppSummary>> GetClientAppsAsync(ListRequest request = null)
        {
            return _restClient.GetListResponseAsync<ClientAppSummary>($"/api/clientapps", request);
        }

        public async Task<ClientApp> GetClientAppAsync(String appId)
        {
            var result = await _restClient.GetAsync<DetailResponse<ClientApp>>($"/api/clientapp/{appId}");
            if (result.Successful)
            {
                return result.Result.Model;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public async Task<DeploymentInstance> GetDeploymentInstanceAsync(string instanceId)
        {
            var result = await _restClient.GetAsync<DetailResponse<DeploymentInstance>>($"/api/deployment/instance/{instanceId}");
            if (result.Successful)
            {
                return result.Result.Model;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public async Task<ListResponse<Models.DataStreamResult>> GetDataStreamValues(string dataStreamId, string deviceId)
        {
            var result = await _restClient.GetAsync<ListResponse<Models.DataStreamResult>>($"/api/datastream/{dataStreamId}/data/{deviceId}");
            if (result.Successful)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public async Task<ListResponse<DeviceSummary>> GetDevicesByDeviceTypeIdAsync(String appId)
        {
            var devices = new List<DeviceSummary>();

            var clientApp = await GetClientAppAsync(appId);

            var instance = await GetDeploymentInstanceAsync(clientApp.DeploymentInstance.Id);

            if (EntityHeader.IsNullOrEmpty(instance.DeviceRepository))
            {
                return ListResponse<DeviceSummary>.FromError("Application is not deployed or does not have a deployment repository.");
            }

            var repoId = instance.DeviceRepository.Id;

            foreach (var config in clientApp.DeviceConfigurations)
            {
                var configDevices = await GetDevicesByDeviceConfigIdAsync(repoId, config.Id);
                if (!configDevices.Successful)
                {
                    return configDevices;
                }

                devices.AddRange(configDevices.Model);
            }

            foreach (var deviceType in clientApp.DeviceConfigurations)
            {
                var deviceTypeDevices = await GetDevicesByDeviceTypeIdAsync(repoId, deviceType.Id);
                if (!deviceTypeDevices.Successful)
                {
                    return deviceTypeDevices;
                }

                devices.AddRange(deviceTypeDevices.Model);
            }

            return ListResponse<DeviceSummary>.Create(devices);
        }

        public Task<ListResponse<DeviceSummary>> GetDevicesByDeviceTypeIdAsync(String deviceRepoId, String deviceTypeId, ListRequest request = null)
        {
            return _restClient.GetListResponseAsync<DeviceSummary>($"/api/devices/{deviceRepoId}/devicetype/{deviceTypeId}", request);
        }

        public Task<ListResponse<DeviceSummary>> GetDevicesByDeviceConfigIdAsync(String deviceRepoId, String deviceConfigId, ListRequest request = null)
        {
            return _restClient.GetListResponseAsync<DeviceSummary>($"/api/devices/{deviceRepoId}/deviceconfig/{deviceConfigId}", request);
        }

        public Task<ListResponse<DeploymentInstanceSummary>> GetDeploymentInstancesAsync(ListRequest listRequest = null)
        {
            return _restClient.GetListResponseAsync<DeploymentInstanceSummary>("/api/deployment/instances", listRequest);
        }

        public Task<ListResponse<DeviceSummary>> GetChildDevicesAsync(string deviceRepoId, string deviceId, ListRequest request = null)
        {
            return _restClient.GetListResponseAsync<DeviceSummary>($"/api/devices/{deviceRepoId}/{deviceId}/children", request);
        }

        public async Task<InvokeResult> AttachChildDeviceAsync(string deviceRepoId, string parentDeviceId, string chidlDeviceId)
        {
            var result = await _restClient.GetAsync<InvokeResult>($"/api/devices/{parentDeviceId}/{parentDeviceId}/attachchild/{chidlDeviceId}");
            return result.Successful ? result.Result : result.ToInvokeResult();
        }

        public async Task<InvokeResult> RemoveChildDevice(string deviceRepoId, string parentDeviceId, string chidlDeviceId)
        {
            var result = await _restClient.GetAsync<InvokeResult>($"/api/devices/{parentDeviceId}/{parentDeviceId}/removechild/{chidlDeviceId}");
            return result.Successful ? result.Result : result.ToInvokeResult();
        }

        public async Task<DetailResponse<Device>> CreateNewDeviceAsync(string deviceRepoId)
        {
            var uri = $"/api/device/{deviceRepoId}/factory";
            var result = await _restClient.GetAsync<DetailResponse<Device>>(uri);
            if (result.Successful)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public async Task<InvokeResult<Device>> CreateNewDeviceAsync(string deviceRepoId, string deviceTypeId)
        {
            var uri = $"/api/device/{deviceRepoId}/{deviceTypeId}/create";
            var result = await _restClient.GetAsync<InvokeResult<Device>>(uri);
            if (result.Successful)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public async Task<AppUser> AddDeviceUser(string devicerepoid, DeviceUserRegistrationRequest newUser)
        {
            var uri = $"/api/device/{devicerepoid}/userdevice";
            var result = await _restClient.PostAsync<DeviceUserRegistrationRequest, AppUser>(uri, newUser);
            if (result.Successful)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }

        }

        public Task<ListResponse<DeviceSummary>> GetDevicesForUserAsync(string repoId, string userId, ListRequest request = null )
        { 
            var uri = $"/api/devices/{repoId}/{userId}";
            return _restClient.GetListResponseAsync<DeviceSummary>(uri, request);
        }

        public async Task<DeviceConfiguration> GetDeviceConfigurationAsync(string deviceConfigId)
        {
            var uri = $"/api/deviceconfig/{deviceConfigId}";
            var result = await _restClient.GetAsync<DetailResponse<DeviceConfiguration>>(uri);
            if (result.Successful)
            {
                return result.Result.Model;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public Task<ListResponse<DeviceSummary>> GetDeviceSummaryByGroupAsync(string groupid)
        {
            throw new NotImplementedException();
        }

        public async Task<ListResponse<DeviceSummaryData>> GetDeviceSummaryDataForGroupAsync(string devicerepoid, string groupid)
        {
            var uri = $"/api/repo/{devicerepoid}/group/{groupid}/devices/summarydata";
            var result = await _restClient.GetAsync<ListResponse<DeviceSummaryData>>(uri);
            if (result.Successful)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public async Task<ListResponse<EntityHeader>> GetDevicesForGroupAsync(string devicerepoid, string groupid)
        {
            var uri = $"/api/repo/{devicerepoid}/group/{groupid}/devices";
            var result = await _restClient.GetAsync<ListResponse<EntityHeader>>(uri);
            if (result.Successful)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public async Task<InvokeResult<DeviceGroupEntry>> AddDeviceToGroupAsync(string devicerepoid, string groupid, string deviceid)
        {
            var uri = $"/api/repo/{devicerepoid}/group/{groupid}/add/{deviceid}";
            var result = await _restClient.GetAsync<InvokeResult<DeviceGroupEntry>>(uri);
            if (result.Successful)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public async Task<InvokeResult> RemoveDeviceToGroupAsync(string devicerepoid, string groupid, string deviceid)
        {
            var uri = $"/api/repo/{devicerepoid}/group/{groupid}/add/{deviceid}";
            var result = await _restClient.GetAsync<InvokeResult>(uri);
            if (result.Successful)
            {
                return result.Result;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }

        public async Task<ListResponse<DeviceGroupSummary>> GetDeviceGroupsForOrgAsync(string devicerepoid, ListRequest listRequest)
        {
            var uri = $"/api/repo/{devicerepoid}/groups";
            var result = await _restClient.GetListResponseAsync<DeviceGroupSummary>(uri, listRequest);
            if (result.Successful)
            {
                return result;
            }
            else
            {
                throw new Exception(result.Errors.First().Message);
            }
        }
      

        public async Task<InvokeResult> SetDeviceMacAddressAsync(string deviceRepoId, string id, string macAddress)
        {
            var url = $"/api/device/{deviceRepoId}/{id}/macaddress/{WebUtility.UrlEncode(macAddress)}/set";
            
            var result = await _restClient.GetAsync<InvokeResult>(url);
            if(result.Successful)
            {
                return result.Result;
            }
            else
            {
                return result.ToInvokeResult();
            }
        }

        public async Task<InvokeResult> SetDeviceiOSBLEAddressAsync(string deviceRepoId, string id, string iosBLEAddress)
        {
            var url = $"/api/device/{deviceRepoId}/{id}/iosbleaddess/{WebUtility.UrlEncode(iosBLEAddress)}/set";

            var result = await _restClient.GetAsync<InvokeResult>(url);
            if (result.Successful)
            {
                return result.Result;
            }
            else
            {
                return result.ToInvokeResult();
            }
        }

        public async Task<InvokeResult<Device>> GetDeviceByiOSBLEAddressAsync(string deviceRepoId, string iosBLEAddress)
        {
            var url = $"/api/device/{deviceRepoId}/iosbleaddress/{WebUtility.UrlEncode(iosBLEAddress)}";

            var result = await _restClient.GetAsync<InvokeResult<Device>>(url);
            return result.Result;
        }
    }
}
