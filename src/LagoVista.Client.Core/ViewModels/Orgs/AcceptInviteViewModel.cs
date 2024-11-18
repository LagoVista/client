using LagoVista.Client.Core.Resources;
using LagoVista.Client.Core.ViewModels.Auth;
using LagoVista.Client.Core.ViewModels.Users;
using LagoVista.Core.Commanding;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Validation;
using LagoVista.Core.ViewModels;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.Orgs
{
    public class AcceptInviteViewModel : AppViewModelBase
    {
        private string _inviteid;

        public AcceptInviteViewModel()
        {
            AcceptInviteCommnad = new RelayCommand(AcceptInvite);
            AcceptAndRegisterCommand = new RelayCommand(AcceptInvite);
            AcceptAndLoginCommand = new RelayCommand(AcceptInvite);
        }
        private async Task<InvokeResult> SendAcceptInvite()
        {
            return (await RestClient.GetAsync<InvokeResult>($"/api/org/inviteuser/accept/{_inviteid}")).Result;
        }

        public async void AcceptInvite()
        {
            if ((await PerformNetworkOperation(SendAcceptInvite)).Successful)
            {
                await Popups.ShowAsync(ClientResources.Accept_AcceptLoggedInSuccessful);
                await ViewModelNavigation.GoBackAsync();
            }
        }

        public override Task InitAsync()
        {
            if (!LaunchArgs.Parameters.ContainsKey("inviteId"))
            {
                throw new System.Exception("Must pass in InviteID as a parameter for AcceptInviteViewmodel");
            }

            _inviteid = LaunchArgs.Parameters["inviteId"].ToString();

            return base.InitAsync();
        }

        public void AcceptInviteAndLogin()
        {
            var args = new ViewModelLaunchArgs()
            {
                ViewModelType = typeof(LoginViewModel),
                LaunchType = LaunchTypes.Other
            };

            args.Parameters.Add("inviteId", _inviteid);
            ViewModelNavigation.NavigateAsync(args);
        }

        public void AcceptInviteAndRegister()
        {
            var args = new ViewModelLaunchArgs()
            {
                ViewModelType = typeof(RegisterUserViewModel),
                LaunchType = LaunchTypes.Other
            };

            args.Parameters.Add("inviteId", _inviteid);
            ViewModelNavigation.NavigateAsync(args);
        }

        public RelayCommand AcceptInviteCommnad { get; private set; }
        public RelayCommand AcceptAndRegisterCommand { get; private set; }
        public RelayCommand AcceptAndLoginCommand { get; private set; }

        public bool IsLoggedIn
        {
            get { return AuthManager.IsAuthenticated; }
        }

        public bool IsNotLoggedIn
        {
            get { return !AuthManager.IsAuthenticated; }
        }
    }
}
