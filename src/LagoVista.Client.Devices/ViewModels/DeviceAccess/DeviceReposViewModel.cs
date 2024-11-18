using LagoVista.Core.Commanding;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess
{
    public class DeviceReposViewModel : ListViewModelBase<DeviceRepositorySummary>
    {

        private bool _hasRepos;
        public bool HasRepos
        {
            get { return _hasRepos; }
            set { Set(ref _hasRepos, value); }
        }

        private bool _emptyRepos;
        public bool EmptyRepos
        {
            get { return _emptyRepos; }
            set { Set(ref _emptyRepos, value); }
        }

        public async override Task InitAsync()
        {
            await base.InitAsync();

            if (ListItems != null)
            {
                HasRepos = ListItems.Any();
                EmptyRepos = !HasRepos;
            }
        }

        public RelayCommand ShowIoTAppStudioCommand { get; private set; }

        protected override void ItemSelected(DeviceRepositorySummary model)
        {
            DeviceRepoSelectedAsync(model);
        }

        protected virtual async void DeviceRepoSelectedAsync(DeviceRepositorySummary model)
        {
            await NavigateAndViewAsync<DevicesViewModel>(model.Id);
            SelectedItem = null;
        }

        protected override string GetListURI()
        {
            return $"/api/devicerepos";
        }

        public RelayCommand MapDeviceRepoCommand { get; private set; }

    }
}
