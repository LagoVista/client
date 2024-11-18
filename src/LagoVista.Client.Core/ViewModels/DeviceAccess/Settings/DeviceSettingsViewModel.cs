using LagoVista.Client.Core.Models;
using LagoVista.Core.Commanding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess.Settings
{
    public class DeviceSettingsViewModel : DeviceViewModelBase
    {
        public DeviceSettingsViewModel()
        {
            MenuOptions = new List<MenuItem>()
            {
                new MenuItem() { FontIconKey = "fa-gear", Name = "Device Information", Details="Setup your device information", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<DeviceViewModel>(this, new KeyValuePair<string, object>(DeviceViewModel.VIEW_TYPE, DeviceViewModel.VIEW_TYPE_DEVICE))) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "Device Connectivity", Details="Connect to the internet and a server", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<DeviceViewModel>(this, new KeyValuePair<string, object>(DeviceViewModel.VIEW_TYPE, DeviceViewModel.VIEW_TYPE_CONNECTION))) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "Inputs and Outputs", Details="Device Connectivity", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigViewModel>(this, new KeyValuePair<string, object>(DeviceViewModel.VIEW_TYPE, DeviceViewModel.VIEW_TYPE_CONNECTION))) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "WiFi Settings", Details="Setup WiFi Device Connection", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<WiFiViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "Server Settings", Details="Setup connection settings for server.", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<ServerViewModel>(this)) }
            };
        }

        public List<MenuItem> MenuOptions { get; }
    }
}