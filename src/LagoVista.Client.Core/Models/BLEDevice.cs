using LagoVista.Core.Models;
using System;
using System.Collections.ObjectModel;

namespace LagoVista.Client.Core.Models
{
    public class BLEDevice : ModelBase
    {
        public string DeviceName { get; set; }
        public string DeviceAddress { get; set; }

        public bool _connected;
        public bool Connected
        {
            get => _connected;
            set
            {
                Set(ref _connected, value);
                ConnectionLog.Add(new BLEConnectionLog(value));
            }
        }

        private DateTime _lastSeen;
        public DateTime LastSeen
        {
            get => _lastSeen;
            set => Set(ref _lastSeen, value);
        }

        private DateTime? _connectingTimeStamp;
        public DateTime? ConnectingTimeStamp
        {
            get => _connectingTimeStamp;
            set => Set(ref _connectingTimeStamp, value);
        }

        private DateTime? _connectionTimeStamp;
        public DateTime? ConnectionTimeStamp
        {
            get => _connectionTimeStamp;
            set => Set(ref _connectionTimeStamp, value);
        }

        private DateTime? _disconnectTimeStamp;
        public DateTime? DisconnectTimeStamp
        {
            get => _disconnectTimeStamp;
            set => Set(ref _disconnectTimeStamp, value);
        }

        public ObservableCollection<BLEConnectionLog> ConnectionLog { get; } = new ObservableCollection<BLEConnectionLog>();

        public ObservableCollection<BLEService> Services { get; } = new ObservableCollection<BLEService>();

        public ObservableCollection<BLECharacteristic> AllCharacteristics { get; } = new ObservableCollection<BLECharacteristic>();
    }

    public class BLEConnectionLog
    {
        public BLEConnectionLog(bool connected)
        {
            Connected = connected;
            TimeStamp = DateTime.Now;
        }

        public bool Connected { get; }
        public DateTime TimeStamp { get; }
    }

    public class BLECharacteristicsValue
    {
        public string Uid { get; set; }
        public string Value { get; set; }
    }


}
