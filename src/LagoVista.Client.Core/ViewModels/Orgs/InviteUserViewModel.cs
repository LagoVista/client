using LagoVista.Client.Core.Resources;
using LagoVista.Core.Commanding;
using LagoVista.Core.Validation;
using LagoVista.UserAdmin.Models.DTOs;
using System;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.Orgs
{
    public class InviteUserViewModel : AppViewModelBase
    {
        public InviteUserViewModel()
        {
            InviteUserCommand = new RelayCommand(InviteUser);
            CancelCommand = new RelayCommand(() => ViewModelNavigation.GoBackAsync());
            Model = new UserAdmin.Models.DTOs.InviteUser();
        }

        public Task<InvokeResult> SendUserInvitation()
        {
            return RestClient.PostAsync("/api/org/inviteuser/send", Model);
        }

        public async void InviteUser()
        {
            if((await PerformNetworkOperation(SendUserInvitation)).Successful)
            {
                await Popups.ShowAsync(ClientResources.InviteUser_InvitationSent);
                await ViewModelNavigation.GoBackAsync();

            }
        }

        public String InviteUserHelpMessage
        {
            get;set;
        }

        public InviteUser Model { get; set; }


        public RelayCommand InviteUserCommand { get; private set; }
        public RelayCommand CancelCommand { get; set; }
    }
}
