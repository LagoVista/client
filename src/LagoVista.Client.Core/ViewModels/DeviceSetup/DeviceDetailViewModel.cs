using LagoVista.Client.Core.ViewModels.DeviceAccess;
using LagoVista.Core.Commanding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceSetup
{
    public class DeviceDetailViewModel : DeviceViewModelBase
    {
        public override Task InitAsync()
        {
            DeviceNameLabel = CurrentDevice.DeviceNameLabel;
            DeviceName = CurrentDevice.Name;

            return base.InitAsync();
        }

        private string _deviceNameLabel;
        public string DeviceNameLabel
        {
            get { return _deviceNameLabel; }
            set { Set(ref _deviceNameLabel, value); }
        }

        private string _deviceName;
        public string DeviceName
        {
            get { return _deviceName; }
            set { Set(ref _deviceName, value); }
        }

        public async void NextView()
        {
            if (DeviceName.Length < 5)
            {
                await Popups.ShowAsync($"{DeviceNameLabel} must be at least 5 characters.");
                return;
            }

            CurrentDevice.Name = DeviceName;
            var result = await PerformNetworkOperation(async () =>
            {
                await DeviceManagementClient.UpdateDeviceAsync(AppConfig.DeviceRepoId, CurrentDevice);
            });

            if (result)
            {
                await ViewModelNavigation.NavigateAsync<SensorsViewModel>(this, DeviceLaunchArgsParam);
            }
        }


        public RelayCommand NextCommand { get; }
    }
}
