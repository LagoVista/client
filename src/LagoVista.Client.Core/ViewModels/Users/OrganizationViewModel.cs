using System;
using System.Threading.Tasks;
using LagoVista.Client.Core.Net;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using LagoVista.UserAdmin.ViewModels.Organization;

namespace LagoVista.Client.Core.ViewModels.Users
{
    public class OrganizationViewModel : FormViewModelBase<CreateOrganizationViewModel>
    {
        private readonly IClientAppInfo _clientAppInfo;
       
        public OrganizationViewModel(IClientAppInfo clientAppInfo)
        {
            _clientAppInfo = clientAppInfo;
        }

        public override void Save()
        {
            base.Save();
            if (FormAdapter.Validate())
            {
                ViewToModel(FormAdapter, Model);
                PerformNetworkOperation(async () =>
                {
                    await (RestClient as RawRestClient).RenewRefreshToken();
                    await ViewModelNavigation.SetAsNewRootAsync(_clientAppInfo.MainViewModel);
                });
            }
        }

        public override Task<InvokeResult> SaveRecordAsync()
        {
            throw new NotImplementedException();
        }

        protected override void BuildForm(EditFormAdapter form)
        {
            form.AddViewCell(nameof(Model.Name));
            form.AddViewCell(nameof(Model.Namespace));
            form.AddViewCell(nameof(Model.WebSite));
        }

        protected override string GetRequestUri()
        {
            return "/api/org/factory";
        }
    }
}
