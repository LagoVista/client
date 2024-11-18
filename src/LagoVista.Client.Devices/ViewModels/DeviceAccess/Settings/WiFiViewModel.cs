using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess.Settings
{
    public class WiFiViewModel : DeviceViewModelBase
    {
        private string _ssid;
        public string SSID
        {
            get => _ssid;
            set => Set(ref _ssid, value);
        }

        private string _uid;
        public string UserId
        {
            get => _uid;
            set => Set(ref _uid, value);
        }

        private string _pwd;
        public string Password
        {
            get => _pwd;
            set => Set(ref _pwd, value);
        }
    }
}
