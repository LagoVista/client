using LagoVista.Client.Core.Resources;
using LagoVista.Core.Commanding;
using LagoVista.Core.Models;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.Validation;
using LagoVista.UserAdmin.Models.Auth;
using LagoVista.UserAdmin.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.Users
{
    public class RegisterUserViewModel : AppViewModelBase
    {
        public RegisterUserViewModel(IDeviceInfo deviceInfo)
        {
            RegisterModel = new RegisterUser
            {
                AppId = AppConfig.AppName,
                DeviceId = deviceInfo.DeviceUniqueId,
                ClientType = "mobileapp",
            };

            if(!EntityHeader.IsNullOrEmpty(AppConfig.SystemOwnerOrg))
            {
                RegisterModel.OrgId = AppConfig.SystemOwnerOrg.Id;
            }

            RegisterCommand = new RelayCommand(Register);

            CancelCommand = new RelayCommand(() => ViewModelNavigation.GoBackAsync());
        }

        public async Task<InvokeResult> SendRegistrationAsync()
        {
            var result = await RestClient.PostAsync<RegisterUser,AuthResponse>("/api/user/register", RegisterModel);
            if (!result.Successful) return result.ToInvokeResult();

            var authResult = result.Result;
            /* Make sure our Access Token is saved so the REST service can use it */
            AuthManager.AccessToken = authResult.AccessToken;
            AuthManager.AccessTokenExpirationUTC = authResult.AccessTokenExpiresUTC;

            AuthManager.RefreshToken = authResult.RefreshToken;
            AuthManager.RefreshTokenExpirationUTC = authResult.RefreshTokenExpiresUTC;

            AuthManager.AppInstanceId = authResult.AppInstanceId;
            AuthManager.IsAuthenticated = true;

            var refrehResult = await RefreshUserFromServerAsync();
            if (!refrehResult.Successful) return refrehResult;

            Logger.AddKVPs(new KeyValuePair<string, string>("Email", AuthManager.User.Email));

            if(LaunchArgs.Parameters.ContainsKey("inviteid"))
            {
                return (await RestClient.GetAsync<InvokeResult>($"/api/org/inviteuser/accept/{LaunchArgs.Parameters["inviteId"]}")).Result;
            }

            await ViewModelNavigation.NavigateAsync<VerifyUserViewModel>(this);

            return InvokeResult.Success;
        }

        public async void Register()
        {
            if (String.IsNullOrEmpty(RegisterModel.FirstName))
            {
                await Popups.ShowAsync(ClientResources.Register_FirstName_Required);
                return;
            }

            if (String.IsNullOrEmpty(RegisterModel.LastName))
            {
                await Popups.ShowAsync(ClientResources.Register_LastName_Required);
                return;
            }

            if (String.IsNullOrEmpty(RegisterModel.Email))
            {
                await Popups.ShowAsync(ClientResources.Register_Email_Required);
                return;
            }

            var emailRegEx = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!emailRegEx.Match(RegisterModel.Email).Success)
            {
                await Popups.ShowAsync(ClientResources.Register_Email_Invalid);
                return;
            }

            if (String.IsNullOrEmpty(RegisterModel.Password))
            {
                await Popups.ShowAsync(ClientResources.Register_Password_Required);
                return;
            }

            var passwordRegEx = new Regex(@"^(?=.*[A-Za-z!@#$%^&])(?=.*\d)[A-Za-z\d]{8,}$");
            if (!passwordRegEx.Match(RegisterModel.Password).Success)
            {
                await Popups.ShowAsync(ClientResources.Password_Requirements);
                return;
            }

            if (RegisterModel.Password != ConfirmPassword)
            {
                await Popups.ShowAsync(ClientResources.Register_Password_Confirm_NoMatch);
                return;
            }

            await PerformNetworkOperation(SendRegistrationAsync);
        }


        public RegisterUser RegisterModel { get; private set; }


        private String _confirmPassword;
        public String ConfirmPassword
        {
            get { return _confirmPassword; }
            set { Set(ref _confirmPassword, value); }
        }

        public RelayCommand RegisterCommand { get; private set; }

        public RelayCommand CancelCommand { get; set; }

    }
}
