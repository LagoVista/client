using LagoVista.Core.IOC;
using LagoVista.Core.PlatformSupport;
using System.Diagnostics;

namespace LagoVista.XPlat.WPF.Services
{
    public class DeviceInfo : IDeviceInfo
    {
        public string DeviceUniqueId { get; private set; }

        public string DeviceType { get; private set; }

        public static void Register(string uniqueId)
        {
            var deviceInfo = new DeviceInfo();

            deviceInfo.DeviceType = "Windows - UWP";

            deviceInfo.DeviceUniqueId = uniqueId;

            SLWIOC.Register<IDeviceInfo>(deviceInfo);
        }
    }
}
