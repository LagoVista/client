using LagoVista.Client.Core.ViewModels.DeviceAccess;
using LagoVista.Core.Commanding;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.ViewModels.DeviceSetup
{
    public class ConnectHardwareViewModel: DeviceViewModelBase
    {
        public ConnectHardwareViewModel()
        {
            NextCommand = new RelayCommand(Next);
        }

        private bool _hardwareConnected;
        public bool HardwareConnected
        {
            get { return _hardwareConnected ; }
            set { Set(ref _hardwareConnected, value); }
        }


        private bool _powerLightOn;
        public bool PowerLightOn
        {
            get { return _powerLightOn; }
            set { Set(ref _powerLightOn, value); }
        }

        private bool _connectionLightOn;
        public bool ConnecctionLightOn
        {
            get { return _connectionLightOn; }
            set { Set(ref _connectionLightOn, value); }
        }


        private bool _slowBeeping;
        public bool SlowBeeping
        {
            get { return _slowBeeping; }
            set { Set(ref _slowBeeping, value); }
        }

        public void Next()
        {
            ViewModelNavigation.NavigateAsync<PairHardwareViewModel>(this);
        }

        public RelayCommand NextCommand { get; }
    }
}
