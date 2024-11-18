using LagoVista.Client.Core.Net;
using LagoVista.Client.Core.Resources;
using LagoVista.Core.Commanding;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Validation;
using LagoVista.UserAdmin.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.Auth
{
    public class ChangePasswordViewModel : AppViewModelBase
    {
        public ChangePasswordViewModel(IRestClient rawRestClient)
        {
            ChangePasswordCommand = new RelayCommand(ChangePassword);
            CancelCommand = new RelayCommand(() => ViewModelNavigation.GoBackAsync());
            Model = new UserAdmin.Models.DTOs.ChangePassword();
        }

    
        public async Task<InvokeResult> SendResetPassword()
        {
            return await RestClient.PostAsync("/api/auth/changepassword", Model, new System.Threading.CancellationTokenSource());
        }

        public async void ChangePassword()
        {
            Model.UserId = AuthManager.User.Id;

            var validationResult = Model.Validate(ConfirmPassword);
            if (validationResult.Successful)
            {
                if ((await PerformNetworkOperation(SendResetPassword)).Successful)
                {
                    await Popups.ShowAsync(ClientResources.ChangePassword_Success);
                    await ViewModelNavigation.GoBackAsync();
                }
            }
            else
            {
                await ShowServerErrorMessageAsync(validationResult);
            }
        }

        private String _confirmPassword;
        public String ConfirmPassword
        {
            get { return _confirmPassword; }
            set { Set(ref _confirmPassword, value); }
        }

        public ChangePassword Model { get; set; }

        public RelayCommand ChangePasswordCommand { get; private set; }
        public RelayCommand CancelCommand { get; set; }
    }
}
