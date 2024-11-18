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
using System.Threading.Tasks;

namespace LagoVista.Client.Core.Interfaces
{
    public interface IDeviceManagementClient
    {
        Task<InvokeResult> AddDeviceAsync(string deviceRepoId, Device device);
        Task<InvokeResult> SetDeviceiOSBLEAddressAsync(string deviceRepoId, string id, string iosBLEAddress);
        Task<InvokeResult> SetDeviceMacAddressAsync(string deviceRepoId, string id, string macAddress);
        Task<InvokeResult<Device>> GetDeviceByMacAddressAsync(string deviceRepoId, string macAddress);
        Task<InvokeResult<Device>> GetDeviceByiOSBLEAddressAsync(string deviceRepoId, string iosBLEAddress);

        Task<DetailResponse<Device>> GetDeviceAsync(string deviceRepoId, string deviceId);
        Task<DetailResponse<Device>> CreateNewDeviceAsync(string deviceRepoId);
        Task<ListResponse<DeviceConfigurationSummary>> GetDeviceConfigsAsync(ListRequest listRequest = null);
        Task<ListResponse<DeviceRepositorySummary>> GetDeviceReposAsync(ListRequest listRequest = null);
        Task<ListResponse<DeviceSummary>> GetDevicesForUserAsync(string orgId, string userId, ListRequest request = null);

        Task<DeviceConfiguration> GetDeviceConfigurationAsync(string deviceConfig);
        Task<ListResponse<DeploymentInstanceSummary>> GetDeploymentInstancesAsync(ListRequest listRequest = null);
        Task<ListResponse<ClientAppSummary>> GetClientAppsAsync(ListRequest request = null);
        Task<ListResponse<DeviceSummary>> GetDeviceSummaryByGroupAsync(string groupid);
        Task<ListResponse<DeviceSummary>> GetDevicesByDeviceTypeIdAsync(String appId);
        Task<DeploymentInstance> GetDeploymentInstanceAsync(string instanceId);
        Task<ClientApp> GetClientAppAsync(String appId);
        Task<ListResponse<DeviceSummary>> GetDevicesByDeviceConfigIdAsync(string deviceRepoId, string deviceConfig, ListRequest request = null);
        Task<ListResponse<DeviceSummary>> GetDevicesByDeviceTypeIdAsync(string deviceRepoId, string appDeviceTypeId, ListRequest request = null);

        Task<ListResponse<DeviceSummary>> GetChildDevicesAsync(string deviceRepoId, string deviceId, ListRequest request = null);
        Task<InvokeResult> AttachChildDeviceAsync(string deviceRepoId, string parentDeviceId, string chidlDeviceId);
        Task<InvokeResult> RemoveChildDevice(string deviceRepoId, string parentDeviceId, string chidlDeviceId);

        Task<InvokeResult<Device>> CreateNewDeviceAsync(string deviceRepoId, string deviceTypeId);

        Task<ListResponse<DeviceTypeSummary>> GetDeviceTypesAsync(ListRequest listRequest = null);
        Task<InvokeResult> UpdateDeviceAsync(string deviceRepoId, Device device);
        Task<AppUser> AddDeviceUser(string devicerepoid, DeviceUserRegistrationRequest newuser);
        Task<ListResponse<DeviceSummaryData>> GetDeviceSummaryDataForGroupAsync(string devicerepoid, string groupid);

        Task<ListResponse<EntityHeader>> GetDevicesForGroupAsync(string devicerepoid, string groupid);
        Task<InvokeResult<DeviceGroupEntry>> AddDeviceToGroupAsync(string devicerepoid, String groupid, string deviceid);
        Task<InvokeResult> RemoveDeviceToGroupAsync(string devicerepoid, String groupid, string deviceid);
        Task<ListResponse<DeviceGroupSummary>> GetDeviceGroupsForOrgAsync(string devicerepoid, ListRequest listRequest);
        Task<InvokeResult<ListenerConfiguration>> GetListenerConfigurationAsync(String instanceId);

        Task<ListResponse<DeviceTypeSummary>> GetDeviceTypesForInstanceAsync(String instanceId, ListRequest request = null);
    }
}