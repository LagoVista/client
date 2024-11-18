namespace LagoVista.Client.Core.Models
{
    public class NuvIoTGATTProfile
    {
        public const string SVC_UUID_NUVIOT = "d804b639-6ce7-4e80-9f8a-ce0f699085eb";
        public const string CHAR_UUID_STATE = "d804b639-6ce7-5e81-9f8a-ce0f699085eb";


        public const string CHAR_UUID_SYS_CONFIG = "d804b639-6ce7-5e82-9f8a-ce0f699085eb";
        /* 
          * Sys Config characteristic
          * Read/Write
          * xxxxx, Device Id <= =>
          * xxxxx, B64 Device Key (128 characters) =>
          * (0/1) Cell Enable <= =>
          * (0/1) WiFi Enable <= =>
          * xxxxxx WiFi SSID <= =>
          * xxxxxx WiFi Password =>
          * xxxx Ping Rate (sec)
          * xxxx Send Rate (sec)
          * (0/1) GPS Enable
          * xxxx GPS Rate (sec),
          */

        public const string CHAR_UUID_IOCONFIG = "d804b639-6ce7-5e83-9f8a-ce0f699085eb";
        /* IO Config
           * 
           * 8 Slots
           * 3 Params per slot
           * x = Configuration
           * xxx = scale
           * xxx = zero
           *
           */

        public const string CHAR_UUID_ADC_IOCONFIG = "d804b639-6ce7-5e84-9f8a-ce0f699085eb";
        /* ADC Config
           * 
           * 8 Slots
           * 3 Params per slot
           * x = Configuration
           * xxx = scale
           * xxx = zero
           *
           */

        public const string CHAR_UUID_IO_VALUE = "d804b639-6ce7-5e85-9f8a-ce0f699085eb";
        /* IO Config
           * 
           * 8 Slots
           * 3 Params per slot
           * x = Configuration
           * xxx = scale
           * xxx = zero
           *
           */

        public const string CHAR_UUID_ADC_VALUE = "d804b639-6ce7-5e86-9f8a-ce0f699085eb";
        /* ADC Config
           * 
           * 8 Slots
           * 3 Params per slot
           * x = Configuration
           * xxx = scale
           * xxx = zero
           *
           */

        public const string CHAR_UUID_RELAY = "d804b639-6ce7-5e87-9f87-ce0f699085eb";
        /* RELAY Config
           * 
           * 16 slots
           * (1,0) <= => Relay State
           *
           */

        public const string CHAR_UUID_CONSOLE = "d804b639-6ce7-5e88-9f88-ce0f699085eb";

        public static GATTProfile GetNuvIoTGATT()
        {
            var profile = new GATTProfile();

            var sysSerivce = new BLEService(SVC_UUID_NUVIOT, "System Service");
            sysSerivce.Characteristics.Add(new BLECharacteristic(CHAR_UUID_STATE, "System State", BLECharacteristicType.Enum));
            sysSerivce.Characteristics.Add(new BLECharacteristic(CHAR_UUID_SYS_CONFIG, "Sys Config", BLECharacteristicType.IntegerArray));
            sysSerivce.Characteristics.Add(new BLECharacteristic(CHAR_UUID_IOCONFIG, "IO Config", BLECharacteristicType.IntegerArray));
            sysSerivce.Characteristics.Add(new BLECharacteristic(CHAR_UUID_ADC_IOCONFIG, "ADC Config", BLECharacteristicType.IntegerArray));
            sysSerivce.Characteristics.Add(new BLECharacteristic(CHAR_UUID_IO_VALUE, "GPIO Values", BLECharacteristicType.RealArray));
            sysSerivce.Characteristics.Add(new BLECharacteristic(CHAR_UUID_ADC_VALUE, "ADC Values", BLECharacteristicType.RealArray));
            sysSerivce.Characteristics.Add(new BLECharacteristic(CHAR_UUID_RELAY, "Relay", BLECharacteristicType.IntegerArray));
            sysSerivce.Characteristics.Add(new BLECharacteristic(CHAR_UUID_CONSOLE, "Console", BLECharacteristicType.ByteArray));
            profile.Services.Add(sysSerivce);

            return profile;
        }
    }
}
