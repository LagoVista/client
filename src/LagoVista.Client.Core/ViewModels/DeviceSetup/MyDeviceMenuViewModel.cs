using LagoVista.Client.Core.ViewModels.DeviceAccess;
using LagoVista.Core.Commanding;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.ViewModels.DeviceSetup
{
    public class MyDeviceMenuViewModel : DeviceViewModelBase
    {
        public MyDeviceMenuViewModel()
        {
            MenuOptions = new List<MenuItem>()
            {
                new MenuItem() { FontIconKey = "fa-gear", Name = "Details", Command = new RelayCommand(() => 
                            ViewModelNavigation.NavigateAsync<MyDeviceViewModel>(this, DeviceLaunchArgsParam)), 
                            Help = "Add a new boar with SeaWolf Marine" },
                
                new MenuItem() { FontIconKey = "fa-gear", Name = "Sensors", Command = new RelayCommand(() => 
                            ViewModelNavigation.NavigateAsync<SensorsViewModel>(this, DeviceLaunchArgsParam)), 
                            Help = "My Boats" },

                new MenuItem() { FontIconKey = "fa-gear", Name = "Claim Brain", Command = new RelayCommand(() =>
                            ViewModelNavigation.NavigateAsync<ClaimHardwareViewModel>(this, DeviceLaunchArgsParam)),
                            Help = "If you can't connect to your device, you can repair it here." },

                new MenuItem() { FontIconKey = "fa-gear", Name = "Connectivity", Command = new RelayCommand(() => 
                            ViewModelNavigation.NavigateAsync<ConnectivityViewModel>(this, DeviceLaunchArgsParam)), 
                            Help = "My Boats" },
                
                new MenuItem() { FontIconKey = "fa-gear", Name = "Billing", Command = new RelayCommand(() => 
                            ViewModelNavigation.NavigateAsync<BillingViewModel>(this, DeviceLaunchArgsParam)), 
                            Help = "My Boats" },
            };
        }

        public List<MenuItem> MenuOptions { get; }
    }
}

