using LagoVista.Client.Core.Interfaces;
using LagoVista.Client.Core.Models;
using LagoVista.Core.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess.Settings
{
    public class DeviceViewModel : DeviceViewModelBase
    {
        BLEDevice _device;
        public ObservableCollection<BLECharacteristic> Characteristics { get; } = new ObservableCollection<BLECharacteristic>();

        public const string VIEW_TYPE = "viewtype";
        public const string VIEW_TYPE_DEVICE = "viewtypedevice";
        public const string VIEW_TYPE_CONNECTION = "viewtypeconnectiontype";
        

        private void TryAddCharacteristics(string id)
        {
            var characteristics = Device.AllCharacteristics.Where(chr => chr.Id == id).FirstOrDefault();
            if (characteristics != null)
            {
                Characteristics.Add(characteristics);
                Debug.WriteLine(" ---> " + characteristics.Name + "=" + characteristics.Value + "<====");

            }
        }

        public async override Task InitAsync()
        {
            if (!GattConnection.ConnectedDevices.Any())
            {
                await Popups.ShowAsync("There are not connected devices.");
            }
            else if (GattConnection.ConnectedDevices.Count > 1)
            {
                await Popups.ShowAsync("There are more then one connected device, should only have one device.");
            }
            else
            {
                Device = GattConnection.ConnectedDevices.First();
                if (this.LaunchArgs.Parameters.ContainsKey(VIEW_TYPE))
                {
                    switch(this.LaunchArgs.Parameters[VIEW_TYPE])
                    {
                        case VIEW_TYPE_DEVICE:
                            TryAddCharacteristics(NuvIoTGATTProfile.CHAR_UUID_SYS_CONFIG);
                            FormTitle = "Device Information";
                            break;
                        case VIEW_TYPE_CONNECTION:
                            TryAddCharacteristics(NuvIoTGATTProfile.CHAR_UUID_SYS_CONFIG);
                            FormTitle = "Connectivity";
                            break;
                    }
                }

                foreach(var characteristics in Characteristics)
                {
                    await GattConnection.ReadCharacteristicAsync(Device, characteristics.Service, characteristics);                    
                }
            }

            await base.InitAsync();
        }

        public async override void Save()
        {
            foreach (var characteristics in Characteristics)
            {
                await GattConnection.UpdateCharacteristic(Device, characteristics.Service, characteristics);
            }
        }

        public BLEDevice Device
        {
            get => _device;
            set => Set(ref _device, value);
        }

        public string _formTitle;
        public string FormTitle
        {
            get => _formTitle;
            set => Set(ref _formTitle, value);
        }

    }
}
