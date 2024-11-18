using LagoVista.Client.Core.Models;
using LagoVista.Client.Core.ViewModels.DeviceAccess;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceSetup
{
    public class ClaimHardwareViewModel : DeviceViewModelBase
    {
        public ClaimHardwareViewModel()
        {
            DiscoveredDevices = GattConnection.DiscoveredDevices;
        }

        public ObservableCollection<BLEDevice> DiscoveredDevices { get; private set; }

        public async override Task InitAsync()
        {
            if (IsBLEConnected)
            {
                await GattConnection.DisconnectAsync(BLEDevice);
            }

            await GattConnection.StartScanAsync();
            await base.InitAsync();
        }

        protected async override void OnBLEDevice_Connected(BLEDevice device)
        {
            base.OnBLEDevice_Connected(device);

            var result = await PerformNetworkOperation(async () =>
            {
                Debug.WriteLine("Trying --> " + AppConfig.PlatformType);

                if (AppConfig.PlatformType == LagoVista.Core.Interfaces.PlatformTypes.iPhone)
                {
                    Debug.WriteLine("Calling iOS");

                    var existingDevice = await DeviceManagementClient.GetDeviceByiOSBLEAddressAsync(AppConfig.DeviceRepoId, device.DeviceAddress);
                    if (existingDevice.Successful && !EntityHeader.IsNullOrEmpty(existingDevice.Result.OwnerUser) && existingDevice.Result.OwnerUser.Id != AuthManager.User.Id)
                    {
                        Debug.WriteLine("Not My Device");

                        return InvokeResult.FromError($"This device is already registered as {existingDevice.Result.Name} to another user.");
                    }
                    else
                    {
                        Debug.WriteLine("Here Goes");

                        var setiOSBLEResult = await DeviceManagementClient.SetDeviceiOSBLEAddressAsync(AppConfig.DeviceRepoId, existingDevice.Result.Id, device.DeviceAddress);
                        Debug.WriteLine("Here Goes" + setiOSBLEResult.Successful.ToString());
                        return setiOSBLEResult.ToInvokeResult();
                    }
                }
                else
                {
                    Debug.WriteLine("Calling Android"); ;

                    var existingDevice = await DeviceManagementClient.GetDeviceByMacAddressAsync(AppConfig.DeviceRepoId, device.DeviceAddress);
                    if (existingDevice.Successful && !EntityHeader.IsNullOrEmpty(existingDevice.Result.OwnerUser) && existingDevice.Result.OwnerUser.Id != AuthManager.User.Id)
                    {
                        Debug.WriteLine("Not My Device");

                        return InvokeResult.FromError($"This device is already registered as {existingDevice.Result.Name} to another user.");
                    }
                    else
                    {
                        var setMacAddressResult = await DeviceManagementClient.SetDeviceMacAddressAsync(AppConfig.DeviceRepoId, existingDevice.Result.Id, device.DeviceAddress);
                        Debug.WriteLine("Here Goes" + setMacAddressResult.Successful.ToString());
                        return setMacAddressResult.ToInvokeResult();
                    }
                }
            });

            Debug.WriteLine("All Done!");

            if (result.Successful)
            {
                await Popups.ShowAsync("Device claimed.");
            }
            else
            {
                Debug.WriteLine(result.Errors.First().Message);
            }
        }

        public async override Task IsClosingAsync()
        {
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
                    Debug.WriteLine("Going into COnnect....");
                    ConnectAsync(value);
                    
                }
                else
                {
                    Debug.WriteLine("Not trying...");
                }
            }
        }
    }
}
