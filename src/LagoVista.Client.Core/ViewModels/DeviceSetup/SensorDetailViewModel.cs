using LagoVista.Client.Core.Models;
using LagoVista.Client.Core.ViewModels.DeviceAccess;
using LagoVista.Core.Commanding;
using LagoVista.Core.Models;
using LagoVista.IoT.DeviceManagement.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceSetup
{
    public class SensorDetailViewModel : DeviceViewModelBase
    {
        public const string SENSOR_ID = "sensorid";
        public const string SENSOR_DEFINITION_ID = "sensordefinitionid";
        public const string SENSOR = "sensor";

        Sensor _sensor;

        public List<EntityHeader> Ports { get; } = new List<EntityHeader>()
        {
            new EntityHeader() { Id = "-1", Key="-1", Text = "-select port-"},
            new EntityHeader() { Id = "0", Key="0", Text = "Port 1"},
            new EntityHeader() { Id = "1", Key="1", Text = "Port 2"},
            new EntityHeader() { Id = "2", Key="2", Text = "Port 3"},
            new EntityHeader() { Id = "3", Key="3", Text = "Port 4"},
            new EntityHeader() { Id = "4", Key="4", Text = "Port 5"},
            new EntityHeader() { Id = "5", Key="5", Text = "Port 6"},
            new EntityHeader() { Id = "6", Key="6", Text = "Port 7"},
            new EntityHeader() { Id = "7", Key="7", Text = "Port 8"},
        };

        public SensorDetailViewModel()
        {
            ReadCommand = new RelayCommand(Read);
            WriteCommand = new RelayCommand(Write);
            RemoveSensorCommand = new RelayCommand(RemoveSensor);
        }

        public override async Task InitAsync()
        {
            await base.InitAsync();

            if (LaunchArgs.LaunchType == LagoVista.Core.ViewModels.LaunchTypes.Create)
            {
                if (LaunchArgs.Parameters.ContainsKey(SENSOR_DEFINITION_ID))
                {
                    var deviceConfig = await GetDeviceConfigurationAsync();
                    var definition = deviceConfig.SensorDefinitions.SingleOrDefault(def => def.Id == LaunchArgs.GetParam<string>(SENSOR_DEFINITION_ID));

                    Sensor = new Sensor(definition);
                    Port = Ports[0];
                }
                else
                {
                    throw new ArgumentNullException(SENSOR_DEFINITION_ID);
                }
            }
            else
            {
                if (LaunchArgs.Parameters.ContainsKey(SENSOR))
                {
                    Sensor = LaunchArgs.GetParam<Sensor>(SENSOR);
                    if (Sensor.PortIndex.HasValue)
                        Port = Ports.Where(prt => prt.Id == Sensor.PortIndex.Value.ToString()).First();
                    else
                        Port = Ports[0];
                }
                else
                {
                    throw new ArgumentNullException(SENSOR);
                }
            }

            if (BLEDevice != null)
            {
                var service = NuvIoTGATTProfile.GetNuvIoTGATT().Services.Find(srvc => srvc.Id == NuvIoTGATTProfile.SVC_UUID_NUVIOT);
                var characteristics = service.Characteristics.First(chr => chr.Id == NuvIoTGATTProfile.CHAR_UUID_IO_VALUE);

                await GattConnection.SubscribeAsync(BLEDevice, service, characteristics);
            }
        }

        public override async Task IsClosingAsync()
        {
            if (IsBLEConnected)
            {
                var service = NuvIoTGATTProfile.GetNuvIoTGATT().Services.Find(srvc => srvc.Id == NuvIoTGATTProfile.SVC_UUID_NUVIOT);
                var characteristics = service.Characteristics.First(chr => chr.Id == NuvIoTGATTProfile.CHAR_UUID_IO_VALUE);
                await GattConnection.UnsubscribeAsync(BLEDevice, service, characteristics);
                await base.IsClosingAsync();
            }
        }

        public void RemoveSensor()
        {
            CurrentDevice.SensorCollection.Remove(Sensor);
            Save();
        }

        protected override void BLECharacteristicRead(BLECharacteristicsValue characteristic)
        {
            if (characteristic.Uid == NuvIoTGATTProfile.CHAR_UUID_IO_VALUE)
            {
                var parts = characteristic.Value.Split(',');
                if (Sensor.Technology.Value == SensorTechnology.ADC)
                {
                    LiveValue = parts[Sensor.PortIndex.Value].ToString();
                }
            }

            base.BLECharacteristicRead(characteristic);
        }

        public override async void Save()
        {
            if (this.LaunchArgs.LaunchType == LagoVista.Core.ViewModels.LaunchTypes.Create)
            {
                CurrentDevice.SensorCollection.Add(Sensor);
            }

            var result = await PerformNetworkOperation(async () =>
            {
                return await DeviceManagementClient.UpdateDeviceAsync(CurrentDevice.DeviceRepository.Id, CurrentDevice);
            });

            if (result.Successful)
            {
                if (this.LaunchArgs.LaunchType == LagoVista.Core.ViewModels.LaunchTypes.Create)
                {
                    await this.ViewModelNavigation.GoBackAsync(1);
                }
                else
                {
                    await this.ViewModelNavigation.GoBackAsync();
                }
            }
        }

        public async void Read()
        {
            var service = NuvIoTGATTProfile.GetNuvIoTGATT().Services.Find(srvc => srvc.Id == NuvIoTGATTProfile.SVC_UUID_NUVIOT);
            var characteristics = service.Characteristics.First(chr => chr.Id == NuvIoTGATTProfile.CHAR_UUID_IOCONFIG);
            await GattConnection.WriteCharacteristic(BLEDevice, service, characteristics, $"AT+READCONFIG," + (Sensor.Technology.Value == SensorTechnology.ADC ? "ADC" : "IO") + "," + Sensor.PortIndex);
            var result = await GattConnection.ReadCharacteristicAsync(BLEDevice, service, characteristics);
            Debug.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(result));
        }

        public async void Write()
        {
            var service = NuvIoTGATTProfile.GetNuvIoTGATT().Services.Find(srvc => srvc.Id == NuvIoTGATTProfile.SVC_UUID_NUVIOT);
            var characteristics = service.Characteristics.First(chr => chr.Id == NuvIoTGATTProfile.CHAR_UUID_IOCONFIG);
            var cfgValues = $"{Sensor.PortIndex.Value},{Sensor.Calibration},{Sensor.Zero},{Sensor.DeviceScaler},{Sensor.Key}";
            await GattConnection.WriteCharacteristic(BLEDevice, service, characteristics, $"AT+WRITECONFIG," + (Sensor.Technology.Value == SensorTechnology.ADC ? "ADC" : "IO") + "," + cfgValues);
            }

        private EntityHeader _port;
        public EntityHeader Port
        {
            get { return _port; }
            set
            {
                Set(ref _port, value);
                if (value != null && value.Id != "-1")
                {
                    var port = int.Parse(value.Id);
                    Sensor.PortIndex = port;
                }
                else
                {
                    Sensor.PortIndex = null;
                }
            }
        }

        public Sensor Sensor
        {
            get => _sensor;
            set { Set(ref _sensor, value); }
        }

        string _liveValue;
        public string LiveValue
        {
            get { return _liveValue; }
            set { Set(ref _liveValue, value); }
        }

        public RelayCommand ReadCommand { get; }
        public RelayCommand WriteCommand { get; }

        public RelayCommand RemoveSensorCommand { get; }
    }
}
