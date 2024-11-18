using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Models
{
    public class RemoteDeviceState
    {
        public string FirmwareSKU { get; private set; }
        public string FirmwareVersion { get; private set; }
        public string HardareSKU { get; private set; }
        public string HardwareRevision { get; private set; }
        public bool Commissioned { get; private set; }
        public string OTAState { get; private set; }
        public string OTAParameter { get; private set; }

        public bool WANConnectivity { get; private set; }
        public bool ServerConnectivity { get; private set; }

        public static RemoteDeviceState FromGATTCharacteristic(string characterisic)
        {
            var parts = characterisic.Split(',');
            if (parts.Length != 8)
            {
                throw new InvalidOperationException($"Device state should be 8 parts but was {parts.Length}");
            }

            var state = new RemoteDeviceState()
            {
                FirmwareSKU = parts[0],
                FirmwareVersion = parts[1],
                HardareSKU = parts[2],
                HardwareRevision = parts[3],
                Commissioned = parts[5] == "1",
                WANConnectivity = parts[6]== "1",
                ServerConnectivity = parts[7] == "1",
                OTAState = parts[8],
                OTAParameter = parts[9]
            };

            return state;
        }

        public static string ToRebootCharacteristic()
        {
            return ",,,,1,,,,";
        }

        public static string ToRCommissionedCharacteristic()
        {
            return ",,,,,1,,,";
        }
    }
}
