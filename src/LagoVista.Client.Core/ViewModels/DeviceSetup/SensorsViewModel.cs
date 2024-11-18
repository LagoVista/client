using LagoVista.Client.Core.ViewModels.DeviceAccess;
using LagoVista.Core.Commanding;
using LagoVista.IoT.DeviceManagement.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceSetup
{
    public class SensorsViewModel : DeviceViewModelBase
    {
        public SensorsViewModel()
        {
            AddSensorCommand = new RelayCommand(ShowAddSensorView);
        }

        public async void ShowAddSensorView()
        {
            await ViewModelNavigation.NavigateAsync<SensorLibraryViewModel>(this, DeviceLaunchArgsParam);
        }

        public override Task InitAsync()
        {
            return base.InitAsync();
        }
        
        public async void ShowSensorView(Sensor sensor)
        {
            await ViewModelNavigation.NavigateAsync<SensorDetailViewModel>(this, DeviceLaunchArgsParam,
                new KeyValuePair<string, object>(SensorDetailViewModel.SENSOR, sensor));
        }

        public Sensor SelectedSensor
        {
            get => null;
            set
            {
                RaisePropertyChanged();
     
                if (value != null)
                {
                    ShowSensorView(value);
                }
            }
        }
        public RelayCommand AddSensorCommand { get; }
    }
}
