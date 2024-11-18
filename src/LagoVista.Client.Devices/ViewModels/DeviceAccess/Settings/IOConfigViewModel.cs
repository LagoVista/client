using LagoVista.Core.Commanding;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess.Settings
{
    public class IOConfigViewModel : DeviceViewModelBase
    {
        public IOConfigViewModel()
        {
            MenuOptions = new List<MenuItem>()
            {
                new MenuItem() { FontIconKey = "fa-gear", Name = "ADC 1", Details="Analog to Digital Converter 1 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "ADC 2", Details="Analog to Digital Converter 2 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "ADC 3", Details="Analog to Digital Converter 3 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "ADC 4", Details="Analog to Digital Converter 4 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "ADC 5", Details="Analog to Digital Converter 5 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "ADC 6", Details="Analog to Digital Converter 6 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "ADC 7", Details="Analog to Digital Converter 7 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "ADC 8", Details="Analog to Digital Converter 8 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "GPIO 1", Details="General Purpose Input/Output 1 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "GPIO 2", Details="General Purpose Input/Output 2 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "GPIO 3", Details="General Purpose Input/Output 3 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "GPIO 4", Details="General Purpose Input/Output 4 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "GPIO 5", Details="General Purpose Input/Output 5 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "GPIO 6", Details="General Purpose Input/Output 6 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "GPIO 7", Details="General Purpose Input/Output 7 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
                new MenuItem() { FontIconKey = "fa-gear", Name = "GPIO 8", Details="General Purpose Input/Output 8 Settings", Command = new RelayCommand(() => ViewModelNavigation.NavigateAsync<IOConfigChannelViewModel>(this)) },
            };
        }

        public List<MenuItem> MenuOptions { get; }
    }
}
