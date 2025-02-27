using LagoVista.Core.IOC;
using LagoVista.Core.Networking.Services;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Logging;
using LagoVista.XPlat.Maui.Services;

namespace LagoVista.XPlat.Maui
{
    public static class Startup
    {
       public static void Init()
        {
            SLWIOC.RegisterSingleton<ILogger, DebugLogger>();
            SLWIOC.RegisterSingleton<IPopupServices, PopupService>();
            SLWIOC.RegisterSingleton<IStorageService, StorageService>();
            SLWIOC.RegisterSingleton<INetworkService, NetworkService>();
        }
    }
}
