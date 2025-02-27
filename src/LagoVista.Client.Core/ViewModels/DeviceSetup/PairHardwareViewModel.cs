using LagoVista.Client.Core.Models;
using LagoVista.Client.Core.ViewModels.DeviceAccess;
using LagoVista.Core.Validation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceSetup
{
    public class PairHardwareViewModel : DeviceViewModelBase
    {
        public PairHardwareViewModel()
        {
            DiscoveredDevices = GattConnection.DiscoveredDevices;
        }

        public ObservableCollection<BLEDevice> DiscoveredDevices { get; private set; }

        public async override Task InitAsync()
        {
            await GattConnection.StartScanAsync();
            GattConnection.DeviceConnected += GattConnection_DeviceConnected;
            await base.InitAsync();
        }

        private async void GattConnection_DeviceConnected(object sender, BLEDevice device)
        {
            base.OnBLEDevice_Connected(device);
            var service = NuvIoTGATTProfile.GetNuvIoTGATT().Services.Find(srvc => srvc.Id == NuvIoTGATTProfile.SVC_UUID_NUVIOT);
            var characteristics = service.Characteristics.Find(chr => chr.Id == NuvIoTGATTProfile.CHAR_UUID_SYS_CONFIG);
            var result = await GattConnection.ReadCharacteristicAsync(device, service, characteristics);
            var str = System.Text.ASCIIEncoding.ASCII.GetString(result);
            var parts = str.Split(',');
            var deviceModelId = parts[1];
            if (deviceModelId.Length == 32)
            {
                await PerformNetworkOperation(async () =>
                {                    
                    var existingDevice = await DeviceManagementClient.GetDeviceByMacAddressAsync(AppConfig.DeviceRepoId, device.DeviceAddress);
                    if (existingDevice.Successful)
                    {
                        return InvokeResult.FromError($"This device is already registered as {existingDevice.Result.Name}");
                    }
                    else
                    {
                        var nuvIoTDevice = await DeviceManagementClient.CreateNewDeviceAsync(AppConfig.DeviceRepoId, deviceModelId);
                        if (nuvIoTDevice.Successful)
                        {
                            var setMacAddressResul = await DeviceManagementClient.SetDeviceMacAddressAsync(AppConfig.DeviceRepoId, nuvIoTDevice.Result.Id, device.DeviceAddress);
                            return setMacAddressResul.ToInvokeResult();
                        }
                        else
                        {
                            return nuvIoTDevice.ToInvokeResult();
                        }
                    }
                });
            }
        }

        public async override Task IsClosingAsync()
        {
            GattConnection.DeviceConnected -= GattConnection_DeviceConnected;
            await GattConnection.StopScanAsync();
            await base.IsClosingAsync();
        }

        public async void ConnectAsync(BLEDevice device)
        {
            await GattConnection.ConnectAsync(device);
        }

        BLEDevice _selectedDevice;
        public BLEDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                Set(ref _selectedDevice, value);
                if (value != null)
                {
                    ConnectAsync(value);
                }
            }
        }
    }
}
