using LagoVista.Client.Core.Interfaces;
using LagoVista.Client.Core.ViewModels.DeviceAccess;
using LagoVista.Core.Commanding;
using LagoVista.Core.IOC;
using LagoVista.Core.ViewModels;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceSetup
{
    public class MyDevicesViewModel : ListViewModelBase<DeviceSummary>
    {
        IEnumerable<DeviceSummary> _originalDeviceList;

        public IDeviceManagementClient DeviceManagementClient => SLWIOC.Get<IDeviceManagementClient>();

        public MyDevicesViewModel()
        {
            SearchNowCommand = RelayCommand.Create(SearchNow);
            FilterClearedCommand = new RelayCommand(FilterCleared);
        }

        public async override Task InitAsync()
        {
            await PerformNetworkOperation(async () =>
            {
                var response = await DeviceManagementClient.GetDevicesForUserAsync(AppConfig.DeviceRepoId, AuthManager.User.Id);
                if (response.Successful)
                {
                    _originalDeviceList = response.Model;
                    ListItems = new ObservableCollection<DeviceSummary>(response.Model);
                }

                return response.ToInvokeResult();
            });

            await base.InitAsync();
        }

        public void FilterCleared()
        {
            ListItems = new ObservableCollection<DeviceSummary>(_originalDeviceList);
        }

        protected override async void ItemSelected(DeviceSummary summary)
        {
            base.ItemSelected(summary);

            await DeviceSelectedAsync(summary);
        }


        protected virtual async Task DeviceSelectedAsync(DeviceSummary summary)
        {
            await PerformNetworkOperation( async () => {
                var response = await DeviceManagementClient.GetDeviceAsync(AppConfig.DeviceRepoId, summary.Id);
                var launchArgs = new ViewModelLaunchArgs()
                {
                    ViewModelType = typeof(MyDeviceMenuViewModel),
                    LaunchType = LaunchTypes.View
                };

                launchArgs.Parameters.Add(nameof(Device), response.Model);
                await ViewModelNavigation.NavigateAsync(launchArgs);

                return response.ToInvokeResult();
            });
        }


        protected override string GetListURI()
        {
            return $"/api/devices/{AppConfig.DeviceRepoId}/{AuthManager.User.Id}";
        }


        public void SearchNow(object arg)
        {
            if (arg != null)
                ListItems = new ObservableCollection<DeviceSummary>(_originalDeviceList.Where(itm => itm.DeviceName.ToLower().Contains(arg.ToString()) ||
                                                                                                     itm.DeviceId.ToLower().Contains(arg.ToString())));
        }

        public RelayCommand SearchNowCommand { get; }

        public RelayCommand FilterClearedCommand { get; }

    }
}
