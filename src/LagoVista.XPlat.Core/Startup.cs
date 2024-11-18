using LagoVista.Client.Core.ViewModels.Auth;
using LagoVista.Client.Core.ViewModels.DeviceAccess;
using LagoVista.Client.Core.ViewModels.DeviceAccess.Settings;
using LagoVista.Client.Core.ViewModels.DeviceSetup;
using LagoVista.Client.Core.ViewModels.Orgs;
using LagoVista.Client.Core.ViewModels.Other;
using LagoVista.Client.Core.ViewModels.Users;
using LagoVista.XPlat.Core.Services;
using LagoVista.XPlat.Core.Views.Auth;
using LagoVista.XPlat.Core.Views.DeviceAccess;
using LagoVista.XPlat.Core.Views.DeviceAccess.Settings;
using LagoVista.XPlat.Core.Views.DeviceSetup;
using LagoVista.XPlat.Core.Views.Orgs;
using LagoVista.XPlat.Core.Views.Other;
using LagoVista.XPlat.Core.Views.Users;

namespace LagoVista.XPlat.Core
{
    public static class Startup
    {

        public static void Init(Xamarin.Forms.Application app, ViewModelNavigation nav)
        {
            nav.Add<LoginViewModel, LoginView>();
            nav.Add<ChangePasswordViewModel, ChangePasswordView>();
            nav.Add<SendResetPasswordLinkViewModel, SendResetPasswordView>();
            nav.Add<ResetPasswordViewModel, ResetPasswordView>();
            nav.Add<InviteUserViewModel, InviteUserView>();

            nav.Add<BTDeviceScanViewModel, BTDeviceScanView>();
            nav.Add<DeviceSettingsViewModel, DeviceSettingsView>();
            nav.Add<WiFiViewModel, WiFiView>();
            nav.Add<ServerViewModel, ServerView>();
            nav.Add<Client.Core.ViewModels.DeviceAccess.Settings.IOConfigViewModel, Views.DeviceAccess.Settings.IOConfigView>();
            nav.Add<IOConfigChannelViewModel, IOConfigChannelView>();

            nav.Add<ConsoleViewModel, ConsoleView>();
            nav.Add<DeviceReposViewModel, DeviceReposView>();
            nav.Add<Client.Core.ViewModels.DeviceAccess.DeviceViewModel, Views.DeviceAccess.DeviceView>();
            nav.Add<Client.Core.ViewModels.DeviceAccess.Settings.DeviceViewModel, Views.DeviceAccess.Settings.DeviceView>();
            nav.Add<DevicesViewModel, DevicesView>();
            nav.Add<DeviceSerialPortAccessViewModel, DeviceSerialPortAccessView>();
            nav.Add<DFUViewModel, DFUView>();
            nav.Add<EditDeviceViewModel, EditDeviceView>();
            nav.Add<Client.Core.ViewModels.DeviceAccess.IOConfigViewModel, Views.DeviceAccess.IOConfigView>();
            nav.Add<LiveDataStreamViewModel, LiveDataView>();
            nav.Add<LiveDataViewModel, LiveDataView>();
            nav.Add<LiveMessagesViewModel, LiveMessagesView>();
            nav.Add<PairBTDeviceViewModel, PairBTDeviceView>();
            nav.Add<ProvisionDeviceViewModel, ProvisionDevicewView>();

            nav.Add<RegisterUserViewModel, RegisterView>();
            nav.Add<VerifyUserViewModel, VerifyUserView>();
            nav.Add<OrgEditorViewModel, OrgEditorView>();
            nav.Add<UserOrgsViewModel, UserOrgsView>();
            nav.Add<AboutViewModel, AboutView>();
            nav.Add<AcceptInviteViewModel, AcceptInviteView>();

            nav.Add<ConnectHardwareViewModel, ConnectHardwareView>();
            nav.Add<ConnectivityViewModel, ConnectivityView>();
            nav.Add<DiagnosticsViewModel, DiagnosticsView>();
            nav.Add<MyDevicesViewModel, MyDevicesView>();
            nav.Add<MyDeviceViewModel, MyDeviceView>();
            nav.Add<MyDeviceMenuViewModel, MyDeviceMenuView>();
            nav.Add<PairHardwareViewModel, PairHardwareView>();
            nav.Add<ClaimHardwareViewModel, PairHardwareView>();
            nav.Add<SensorDetailViewModel, SensorDetailView>();
            nav.Add<SensorLibraryViewModel, SensorLibraryView>();
            nav.Add<SensorsViewModel, SensorsView>();
            nav.Add<BillingViewModel, BillingView>();
            nav.Add<DeviceDetailViewModel, DeviceDetailView>();
            nav.Add<OTADFUViewModel, OTADFUView>();
        }
    }
}
