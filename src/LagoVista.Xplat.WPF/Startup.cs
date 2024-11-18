using LagoVista.Client.Core.Interfaces;
using LagoVista.Core.Interfaces;
using LagoVista.Core.IOC;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.WPF.PlatformSupport;
using LagoVista.XPlat.WPF.Services;

namespace LagoVista.Xplat.WPF
{
    public class Startup
    {
        public static void Init(IAppConfig appConfig)
        {
            SLWIOC.RegisterSingleton<IStorageService>(new StorageService());
            SLWIOC.RegisterSingleton<INetworkService>(new NetworkService());
            SLWIOC.RegisterSingleton<ILogger>(new DebugLogger());
        }
    }
}
