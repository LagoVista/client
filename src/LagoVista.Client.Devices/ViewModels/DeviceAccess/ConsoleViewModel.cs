using LagoVista.Client.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess
{
    public class ConsoleViewModel : DeviceViewModelBase
    {

        public ConsoleViewModel()
        {

        }

        protected override void OnBLEDevice_Disconnected(BLEDevice device)
        {
            Lines.Insert(0, "Device Disconnected");
        }

        
        public override async Task InitAsync()
        {
            await base.InitAsync();

            if (DeviceConnected)
            {
                Lines.Insert(0, "device connected.");
            }

        }

        public ObservableCollection<string> Lines { get; } = new ObservableCollection<string>();
    }
}
