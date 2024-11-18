using LagoVista.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Models
{
    public class GATTProfile
    {
        public List<BLEService> Services { get; } = new List<BLEService>();
    }

    public class BLEService
    {
        public BLEService(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
        public List<BLECharacteristic> Characteristics { get; } = new List<BLECharacteristic>();
    }

    public enum BLECharacteristicPropertyTypes
    {
        Broadcast,
        Read,
        WriteNoResponse,
        Write,
        Notify,
        Indicate,
        SignedWrite,
        ExtendedProperties
    }

    public enum BLECharacteristicType
    {
        Enum,
        String,
        StringArray,
        Integer,
        Real,
        IntegerArray,
        RealArray,
        Boolean,
        ByteArray,
        Command
    }

    public class BLECharacteristic : ModelBase
    {
        public BLECharacteristic(BLEService service, string id, string name, BLECharacteristicType type, IEnumerable<BLECharacteristicPropertyTypes> properties)
        {
            Id = id;
            Name = name;
            Type = type;
            Properties = properties ?? throw new ArgumentException(nameof(properties));
            Service = service;
        }

        public BLECharacteristic(string id, string name, BLECharacteristicType type)
        {
            Id = id;
            Name = name;
            Type = type;
        }

        public string Id { get; }
        public string Name { get; }

        private string _value;
        public string Value 
        {
            get => _value;
            set
            {
                Set(ref _value, value);
                RaisePropertyChanged(nameof(Value));
                if(String.IsNullOrEmpty(value))
                {
                    _buffer = null;
                }
                else
                {
                    _buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(value);
                }
            }
        }

        private byte[] _buffer;

        public byte[] Buffer
        {
            get => _buffer;
            set
            {
                Set(ref _buffer, value);
                if(_buffer != null)
                {
                    Set(ref _value, System.Text.ASCIIEncoding.ASCII.GetString(_buffer)); 
                }
                else
                {
                    Set(ref _value, null);
                }
            }
        }

        public BLEService Service { get;  }

        public BLECharacteristicType Type { get; }

        public IEnumerable<BLECharacteristicPropertyTypes> Properties { get; }
    }
}
