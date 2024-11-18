using LagoVista.Client.Core.Resources;
using LagoVista.Core.Commanding;
using LagoVista.Core.Validation;
using LagoVista.UserAdmin.Models.DTOs;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.Auth
{
    public class ResetPasswordViewModel : AppViewModelBase
    {
        public ResetPasswordViewModel()
        {
         
            Model = new ResetPassword();

            ResetPasswordCommand = new RelayCommand(ResetPassword);
            CancelCommand = new RelayCommand(() => ViewModelNavigation.GoBackAsync());
        }

        public Task<InvokeResult> SendResetPasswordAsync()
        {
            return RestClient.PostAsync("/api/auth/resetpassword", Model);
        }

        public async void ResetPassword()
        {
            if (String.IsNullOrEmpty(Model.Email))
            {
                await Popups.ShowAsync(ClientResources.Register_Email_Required);
                return;
            }

            var emailRegEx = new Regex(EMAIL_REGEX);
            if (!emailRegEx.Match(Model.Email).Success)
            {
                await Popups.ShowAsync(ClientResources.Register_Email_Invalid);
                return;
            }

            var passwordRegEx = new Regex(PASSWORD_REGEX);
            if (!passwordRegEx.Match(Model.NewPassword).Success)
            {
                await Popups.ShowAsync(ClientResources.Password_Requirements);
                return;
            }

            if (Model.NewPassword != ConfirmPassword)
            {
                await Popups.ShowAsync(ClientResources.Register_Password_Confirm_NoMatch);
                return;
            }

            Model.Token = LaunchArgs.Parameters["code"].ToString();

            if ((await PerformNetworkOperation(SendResetPasswordAsync)).Successful)
            {
                await Popups.ShowAsync(ClientResources.ResetPassword_SuccessMessage);
                await ViewModelNavigation.GoBackAsync();
            }
        }

        public ResetPassword Model
        {
            get; set;
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { Set(ref _confirmPassword, value); }
        }


        public RelayCommand ResetPasswordCommand { get; set; }
       

        public RelayCommand CancelCommand { get; set; }
    }
}
