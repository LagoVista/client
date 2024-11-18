using LagoVista.Client.Core.ViewModels.DeviceAccess;
using LagoVista.IoT.DeviceManagement.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceSetup
{
    public class SensorLibraryViewModel : DeviceViewModelBase
    {
        public override async Task InitAsync()
        {
            var deviceConfig = await GetDeviceConfigurationAsync();
            SensorTypes = new ObservableCollection<SensorDefinition>(deviceConfig.SensorDefinitions);
            
            await base.InitAsync();
        }

        private async void AddSensorAsync(SensorDefinition definition)
        {

            await ViewModelNavigation.NavigateAndCreateAsync<SensorDetailViewModel>(this, DeviceLaunchArgsParam, CreateKVP(SensorDetailViewModel.SENSOR_DEFINITION_ID, definition.Id));
        }

        SensorDefinition _selectedSensor;
        public SensorDefinition SelectedSensor
        {
            get { return _selectedSensor; }
            set
            {
                Set(ref _selectedSensor, value);
                AddSensorAsync(value);
            }
        }

        private ObservableCollection<SensorDefinition> _sensorTypes;
        public ObservableCollection<SensorDefinition> SensorTypes
        {
            get => _sensorTypes;
            private set
            {
                Set(ref _sensorTypes, value);
            }
        }
    }
}
