using LagoVista.Core.PlatformSupport;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core.IOC;
using System.Diagnostics;

namespace LagoVista.XPlat.Core.Services
{
    public class DeviceInfo : IDeviceInfo
    {
        public string DeviceUniqueId { get; private set; }

        public string DeviceType { get; private set; }

        public static void Register()
        {
            var deviceInfo = new DeviceInfo();

            if (Device.RuntimePlatform == Device.Android)
            {
                deviceInfo.DeviceType = "Android";
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                deviceInfo.DeviceType = "iOS";
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                deviceInfo.DeviceType = "UWP";
            }
            else
            {
                deviceInfo.DeviceType = "Unknown";
            }

            var getidResult = Microsoft.AppCenter.AppCenter.GetInstallIdAsync().Result;
            if(getidResult.HasValue)
            {
                deviceInfo.DeviceUniqueId = getidResult.Value.ToString();
                Debug.WriteLine("Found unique id for xamarin forms app: " + deviceInfo.DeviceUniqueId);
            }
            else
            {
                deviceInfo.DeviceUniqueId = "UNKNOWN - NO ID FROM MOBILE CENTER.";
                Debug.WriteLine("Could not determine unique device id for Xamarin Forms app.");
            }

            SLWIOC.Register<IDeviceInfo>(deviceInfo);
        }
    }
}
