using LagoVista.Core.Models;
using Newtonsoft.Json;
using System;

namespace LagoVista.Client.Core.Models
{
    public class SysConfig : ModelBase
    {
        bool _commissioned;
        [JsonProperty("commissioned")]
        public bool Commissioned
        {
            get => _commissioned;
            set => Set(ref _commissioned, value);
        }

        string _deviceId;
        [JsonProperty("deviceId")]
        public String DeviceId
        {
            get => _deviceId;
            set => Set(ref _deviceId, value);
        }

        string _deviceAuthToken;
        [JsonProperty("deviceAuthToken")]
        public String DeviceAuthToken
        {
            get => _deviceAuthToken;
            set => Set(ref _deviceAuthToken, value);
        }

        string _wifiSSID;
        [JsonProperty("wifissid")]
        public String WiFiSSID
        {
            get => _wifiSSID;
            set => Set(ref _wifiSSID, value);
        }

        string _wifiPWD;
        [JsonProperty("wifipwd")]
        public String WiFiPWD
        {
            get => _wifiPWD;
            set => Set(ref _wifiPWD, value);
        }

        string _srvrHostName;
        [JsonProperty("srvrHostName")]
        public String SrvrHostName
        {
            get => _srvrHostName;
            set => Set(ref _srvrHostName, value);
        }

        bool _anonymous;
        [JsonProperty("anonymous")]
        public bool Anonymous
        {
            get => _anonymous;
            set => Set(ref _anonymous, value);
        }

        string _srvrUID;
        [JsonProperty("srvrUid")]
        public String SrvrUID
        {
            get => _srvrUID;
            set => Set(ref _srvrUID, value);
        }

        string _srvrPWD;
        [JsonProperty("srvrPwd")]
        public String SrvrPWD
        {
            get => _srvrPWD;
            set => Set(ref _srvrPWD, value);
        }

        bool _gpsEnabled;
        [JsonProperty("gpsEnabled")]
        public bool GPSEnabled
        {
            get => _gpsEnabled;
            set => Set(ref _gpsEnabled, value);
        }

        bool _cellEnabled;
        [JsonProperty("cellEnabled")]
        public bool CellEnabled
        {
            get => _cellEnabled;
            set => Set(ref _cellEnabled, value);
        }

        bool _wifiEnabled;
        [JsonProperty("wifiEnabled")]
        public bool WiFiEnabled
        {
            get => _wifiEnabled;
            set => Set(ref _wifiEnabled, value);
        }

        bool _verboseLogging;
        [JsonProperty("verboseLogging")]
        public bool VerboseLogging
        {
            get => _verboseLogging;
            set => Set(ref _verboseLogging, value);
        }

        double _pingRate;
        [JsonProperty("pingRate")]
        public double PingRate
        {
            get => _pingRate;
            set => Set(ref _pingRate, value);
        }

        double _sendUpdateRate;
        [JsonProperty("sendUpdateRate")]
        public double SendUpdateRate
        {
            get => _sendUpdateRate;
            set => Set(ref _sendUpdateRate, value);
        }

        double _gpsUpdateRate;
        [JsonProperty("gpsUpdateRate")]
        public double GPSUpdateRate
        {
            get => _gpsUpdateRate;
            set => Set(ref _gpsUpdateRate, value);
        }

        long _modemBaudRate;
        [JsonProperty("baud")]
        public long GPRSModemBaudRate
        {
            get => _modemBaudRate;
            set => Set(ref _modemBaudRate, value);
        }

        bool _deepSleepEnabled;
        [JsonProperty("DeepSleepEnabled")]
        public bool DeepSleepEnabled
        {
            get => _deepSleepEnabled;
            set => Set(ref _deepSleepEnabled, value);
        }

        double _deepSleepPeriod;
        [JsonProperty("DeepSleepPeriod")]
        public double DeepSleepPeriod
        {
            get => _deepSleepPeriod;
            set => Set(ref _deepSleepPeriod, value);
        }

        public static SysConfig FromGATTCharacteristics(String config)
        {
            var sysConfig = new SysConfig();

            /*
|0 |Device Id |Read/Write |True |Used to uniquely identify the device within NuvIoT |
|1 |Device Auth Token |Write Only| True| Base64 Encoded authorization token that can be used to authorize the device to NuvIoT|
|2 |Enable Cellular Connection| Read/Write| True| 1=Enabled, 0=Disabled|
|3 |Enable WiFi Connection| Read/Write| True| 1=Enabled, 0=Disabled|
|4 |Perform Station Scan| Write-Only| True| Write 1 to this field to perform a station scan, the results will be returned via the console characteristics|
|5 |WiFi SSID | Read/Write| True| WiFi Station Identifier|
|6 |WiFi Password | Write Only| True | WiFi Password|
|7 |Ping Update Rate| Read/Write| True| Number of seconds between a ping message being sent from the device to NuvIoT (allows decimal numbers)|
|8 |Message Send Rate| Read/Write| True| Number of seconds between sending data from the device to NuvIoT (allows decimal numbers)|
|9 |GPS Enable| Read/Write| True| 1=Enabled, 0=Disabled|
|10 |GPS Send Rate| Read/Write| True| Number of seconds between sending data from the GPS to NuvIoT|
|11 |Sleep Enabled| Read/Write| True| Should the device go into a deep sleep mode to conserve battery power|
|12 |Sleep Interval| Read/Write| True| Number of seconds the device should sleep before waking up and sending data|
             */

            var parts = config.Split(',');
            if(parts.Length != 13)
            {
                throw new InvalidOperationException($"Gatt Characteristic should contain 13 values but contains {13}");
            }

            if (!String.IsNullOrEmpty(parts[0])) sysConfig.DeviceId = parts[0];
            if (!String.IsNullOrEmpty(parts[1])) sysConfig.DeviceAuthToken = parts[1];
            if (!String.IsNullOrEmpty(parts[2])) sysConfig.CellEnabled = parts[2] == "1";
            if (!String.IsNullOrEmpty(parts[3])) sysConfig.WiFiEnabled = parts[3] == "1";
            if (!String.IsNullOrEmpty(parts[5])) sysConfig.WiFiSSID = parts[5];
            if (!String.IsNullOrEmpty(parts[7])) sysConfig.SrvrHostName = parts[7];
            if (!String.IsNullOrEmpty(parts[8])) sysConfig.Anonymous = parts[8] == "1";
            if (!String.IsNullOrEmpty(parts[9])) sysConfig.SrvrUID = parts[9];
            if (!String.IsNullOrEmpty(parts[11])) sysConfig.PingRate = Convert.ToDouble(parts[11]);
            if (!String.IsNullOrEmpty(parts[12])) sysConfig.SendUpdateRate = Convert.ToDouble(parts[12]);
            if (!String.IsNullOrEmpty(parts[13])) sysConfig.GPSEnabled = parts[13] == "1";
            if (!String.IsNullOrEmpty(parts[14])) sysConfig.GPSUpdateRate = Convert.ToDouble(parts[14]);
            if (!String.IsNullOrEmpty(parts[15])) sysConfig.DeepSleepEnabled = parts[15] == "1";
            if (!String.IsNullOrEmpty(parts[16])) sysConfig.DeepSleepPeriod = Convert.ToDouble(parts[16]);

            return sysConfig;
        }

        public string ToGATTCharacteristic()
        {
            return $"{DeviceId},{DeviceAuthToken},{(CellEnabled ? '1': '0')},{(WiFiEnabled ? '1': '0')},,{WiFiSSID},{WiFiPWD},{SrvrHostName},{Anonymous},{SrvrUID},{SrvrPWD},{PingRate},{SendUpdateRate},{(GPSEnabled ? '1': '0')},{GPSUpdateRate},{(DeepSleepEnabled ? '1':'0')},{DeepSleepPeriod}";
        }

        public static string ToGATTWiFiScanCharacteristics()
        {
            return ",,,1,1,,,,,,,,,,,,";
        }

        public static string ToGATTWiFiSettingsCharacteristics(string ssid, string pwd)
        {
            return $",,,1,,{ssid},{pwd},,,,,,,,,,";
        }

        public static string ToGATTServerConnectionCharacteristics(string hostName, bool anonymous, string uid = "", string pwd = "")
        {
            return $",,,,,,,{hostName},{(anonymous ? '1':'0')},{uid},{pwd},,,,,,";
        }
    }
}
