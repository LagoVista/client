using LagoVista.Client.Core.Interfaces;
using LagoVista.Client.Core.Models;
using LagoVista.Client.Core.Net;
using LagoVista.Core;
using LagoVista.Core.Commanding;
using LagoVista.Core.IOC;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess
{
    public class PairBTDeviceViewModel : DeviceViewModelBase
    {
        private IBluetoothSerial _btSerial;

        public PairBTDeviceViewModel()
        {
            _btSerial = SLWIOC.Get<IBluetoothSerial>();
        }

        public override async Task InitAsync()
        {
            await base.InitAsync();            

            IsBusy = true;

            var devices = await _btSerial.SearchAsync();
            _btSerial.DeviceDiscovered += _btSerial_DeviceDiscovered;
            ConnectedDevices.Clear();
            foreach (var device in devices)
            {
                if (device.DeviceName.StartsWith("NuvIoT"))
                {
                    ConnectedDevices.Add(device);
                }
            }

            IsBusy = false;
        }

        private void _btSerial_DeviceDiscovered(object sender, BTDevice e)
        {
            ConnectedDevices.Add(e);
        }

        public async Task<InvokeResult> SetBTDeviceIdAsync()
        {
            return await PerformNetworkOperation(async () =>
            {
                var restClient = new FormRestClient<Device>(base.RestClient);

                var result = await restClient.GetAsync(GetRequestUri());
                if (result.Successful)
                {
                    var existingProperty = result.Result.Model.Properties.FirstOrDefault(prop => prop.Key == "BT_MAC_ADDRESS");
                    if (existingProperty != null)
                    {
                        existingProperty.Value = SelectedDevice.DeviceId;
                    }
                    else
                    {
                        var newProperty = new AttributeValue()
                        {
                            AttributeType = LagoVista.Core.Models.EntityHeader<LagoVista.IoT.DeviceAdmin.Models.ParameterTypes>.Create(LagoVista.IoT.DeviceAdmin.Models.ParameterTypes.String),
                            Name = "Bluetooth MAC Address",
                            Key = "BT_MAC_ADDRESS",
                            LastUpdated = DateTime.UtcNow.ToJSONString(),
                            LastUpdatedBy = AuthManager.User.Name,
                            Value = SelectedDevice.DeviceId
                        };

                        result.Result.Model.Properties.Add(newProperty);
                    }

                    return await restClient.UpdateAsync($"/api/device/{DeviceRepoId}", result.Result.Model);
                }
                else
                {
                    return result.ToInvokeResult();
                }
            });
        }

        public override Task IsClosingAsync()
        {
            _btSerial.DeviceDiscovered -= _btSerial_DeviceDiscovered;
            _btSerial.StopSearchingAsync();
            return base.IsClosingAsync();
        }

        private BTDevice _selectedDevice;
        public BTDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set { ConnectDeviceAsync(value); }
        }

        protected string GetRequestUri()
        {
            return $"/api/device/{DeviceRepoId}/{DeviceId}";
        }

        private async void ConnectDeviceAsync(BTDevice device)
        {
            try
            {
                await _btSerial.ConnectAsync(device);

                Set(ref _selectedDevice, device);

                var updateResult = await SetBTDeviceIdAsync();
                if (!updateResult.Successful)
                {
                    throw new Exception(updateResult.Errors.First().Message);
                }

                await Storage.StoreKVP(ResolveBTDeviceIdKey(DeviceRepoId, DeviceId), device.DeviceId);
                await _btSerial.DisconnectAsync();

                CloseScreen();
            }
            catch (Exception ex)
            {
                if (_btSerial.CurrentDevice != null)
                {
                    await _btSerial.DisconnectAsync();
                }

                await Popups.ShowAsync($"Could not connect to {device.DeviceName} - {ex.Message}");
            }
        }

        public ObservableCollection<BTDevice> ConnectedDevices { get; private set; } = new ObservableCollection<BTDevice>();

        public RelayCommand PairDeviceCommand { get; private set; }

        public static string ResolveBTDeviceIdKey(String repoId, String deviceId)
        {
            return $"BT_DEVICE_ID_{repoId}_{deviceId}";
        }
    }
}