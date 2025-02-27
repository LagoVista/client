using LagoVista.Core.Commanding;
using LagoVista.Core.ViewModels;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess
{
    public class DevicesViewModel : ListViewModelBase<DeviceSummary>
    {
        IEnumerable<DeviceSummary> _originalDeviceList;
        public DevicesViewModel()
        {
            SearchNowCommand = RelayCommand.Create(SearchNow);
            FilterClearedCommand = new RelayCommand(FilterCleared);
        }

        public void SearchNow(object arg)
        {
            if (arg != null)
                ListItems = new ObservableCollection<DeviceSummary>(_originalDeviceList.Where(itm => (itm.DeviceName.ToLower().Contains(arg.ToString())) ||
                                                                                                     (itm.DeviceId.ToLower().Contains(arg.ToString()))));
        }

        public void FilterCleared()
        {
            ListItems = new ObservableCollection<DeviceSummary>(_originalDeviceList);
        }

        public async override Task InitAsync()
        {
            await base.InitAsync();
            _originalDeviceList = ListItems;
        }

        protected override async void ItemSelected(DeviceSummary summary)
        {
            base.ItemSelected(summary);

            await DeviceSelectedAsync(summary);
        }

        private string _deviceFilter;
        public string DeviceFilter
        {
            get { return _deviceFilter; }
            set
            {
                Set(ref _deviceFilter, value);
            }
        }

        protected override string GetListURI()
        {
            return $"/api/devices/{LaunchArgs.ChildId}";
        }

        protected virtual async Task DeviceSelectedAsync(DeviceSummary summary)
        {
            var launchArgs = new ViewModelLaunchArgs()
            {
                ViewModelType = typeof(DeviceViewModel),
                LaunchType = LaunchTypes.View
            };

            launchArgs.Parameters.Add(DeviceViewModelBase.DEVICE_ID, summary.Id);
            launchArgs.Parameters.Add(DeviceViewModelBase.DEVICE_REPO_ID, LaunchArgs.ChildId);

            SelectedItem = null;

            await ViewModelNavigation.NavigateAsync(launchArgs);
        }

        public RelayCommand SearchNowCommand { get; }

        public RelayCommand FilterClearedCommand { get; }
    }
}
