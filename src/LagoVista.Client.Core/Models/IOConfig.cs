using Newtonsoft.Json;
using System;

namespace LagoVista.Client.Core.Models
{
    /* This version maps to what is stored on the server,
     * there is a slightly different structure when used with BLE */
    public class IOConfig
    {
        [JsonProperty("adc1c")]
        public byte ADC1Config { get; set; }

        [JsonProperty("adc2c")]
        public byte ADC2Config { get; set; }

        [JsonProperty("adc3c")]
        public byte ADC3Config { get; set; }

        [JsonProperty("adc4c")]
        public byte ADC4Config { get; set; }

        [JsonProperty("adc5c")]
        public byte ADC5Config { get; set; }

        [JsonProperty("adc6c")]
        public byte ADC6Config { get; set; }

        [JsonProperty("adc7c")]
        public byte ADC7Config { get; set; }

        [JsonProperty("adc8c")]
        public byte ADC8Config { get; set; }


        [JsonProperty("adc1l")]
        public string ADC1Label { get; set; }

        [JsonProperty("adc2l")]
        public string ADC2Label { get; set; }

        [JsonProperty("adc3l")]
        public string ADC3Label { get; set; }

        [JsonProperty("adc4l")]
        public string ADC4Label { get; set; }

        [JsonProperty("adc5l")]
        public string ADC5Label { get; set; }

        [JsonProperty("adc6l")]
        public string ADC6Label { get; set; }

        [JsonProperty("adc7l")]
        public string ADC7Label { get; set; }

        [JsonProperty("adc8l")]
        public string ADC8Label { get; set; }


        [JsonProperty("adc1n")]
        public string ADC1Name { get; set; }

        [JsonProperty("adc2n")]
        public string ADC2Name { get; set; }

        [JsonProperty("adc3n")]
        public string ADC3Name { get; set; }

        [JsonProperty("adc4n")]
        public string ADC4Name { get; set; }

        [JsonProperty("adc5n")]
        public string ADC5Name { get; set; }

        [JsonProperty("adc6n")]
        public string ADC6Name { get; set; }

        [JsonProperty("adc7n")]
        public string ADC7Name { get; set; }

        [JsonProperty("adc8n")]
        public string ADC8Name { get; set; }



        [JsonProperty("adc1s")]
        public float ADC1Scaler { get; set; }

        [JsonProperty("adc2s")]
        public float ADC2Scaler { get; set; }

        [JsonProperty("adc3s")]
        public float ADC3Scaler { get; set; }

        [JsonProperty("adc4s")]
        public float ADC4Scaler { get; set; }

        [JsonProperty("adc5s")]
        public float ADC5Scaler { get; set; }

        [JsonProperty("adc6s")]
        public float ADC6Scaler { get; set; }

        [JsonProperty("adc7s")]
        public float ADC7Scaler { get; set; }

        [JsonProperty("adc8s")]
        public float ADC8Scaler { get; set; }



        [JsonProperty("io1c")]
        public byte IO1Config { get; set; }

        [JsonProperty("io2c")]
        public byte IO2Config { get; set; }

        [JsonProperty("io3c")]
        public byte IO3Config { get; set; }

        [JsonProperty("io4c")]
        public byte IO4Config { get; set; }

        [JsonProperty("io5c")]
        public byte IO5Config { get; set; }

        [JsonProperty("io6c")]
        public byte IO6Config { get; set; }

        [JsonProperty("io7c")]
        public byte IO7Config { get; set; }

        [JsonProperty("io8c")]
        public byte IO8Config { get; set; }


        [JsonProperty("io1l")]
        public string IO1Label { get; set; }

        [JsonProperty("io2l")]
        public string IO2Label { get; set; }

        [JsonProperty("io3l")]
        public string IO3Label { get; set; }

        [JsonProperty("io4l")]
        public string IO4Label { get; set; }

        [JsonProperty("io5l")]
        public string IO5Label { get; set; }

        [JsonProperty("io6l")]
        public string IO6Label { get; set; }

        [JsonProperty("io7l")]
        public string IO7Label { get; set; }

        [JsonProperty("io8l")]
        public string IO8Label { get; set; }


        [JsonProperty("io1n")]
        public string IO1Name { get; set; }

        [JsonProperty("io2n")]
        public string IO2Name { get; set; }

        [JsonProperty("io3n")]
        public string IO3Name { get; set; }

        [JsonProperty("io4n")]
        public string IO4Name { get; set; }

        [JsonProperty("io5n")]
        public string IO5Name { get; set; }

        [JsonProperty("io6n")]
        public string IO6Name { get; set; }

        [JsonProperty("io7n")]
        public string IO7Name { get; set; }

        [JsonProperty("io8n")]
        public string IO8Name { get; set; }



        [JsonProperty("io1s")]
        public float IO1Scaler { get; set; }

        [JsonProperty("io2s")]
        public float IO2Scaler { get; set; }

        [JsonProperty("io3s")]
        public float IO3Scaler { get; set; }

        [JsonProperty("io4s")]
        public float IO4Scaler { get; set; }

        [JsonProperty("io5s")]
        public float IO5Scaler { get; set; }

        [JsonProperty("io6s")]
        public float IO6Scaler { get; set; }

        [JsonProperty("io7s")]
        public float IO7Scaler { get; set; }

        [JsonProperty("io8s")]
        public float IO8Scaler { get; set; }

        public String ResolveLabel(string name)
        {
            if (name == ADC1Name) return ADC1Label;
            if (name == ADC2Name) return ADC2Label;
            if (name == ADC3Name) return ADC3Label;
            if (name == ADC4Name) return ADC4Label;
            if (name == ADC5Name) return ADC5Label;
            if (name == ADC6Name) return ADC6Label;
            if (name == ADC7Name) return ADC7Label;
            if (name == ADC8Name) return ADC8Label;
            if (name == IO1Name) return IO1Label;
            if (name == IO2Name) return IO2Label;
            if (name == IO3Name) return IO3Label;
            if (name == IO4Name) return IO4Label;
            if (name == IO5Name) return IO5Label;
            if (name == IO6Name) return IO6Label;
            if (name == IO7Name) return IO7Label;
            if (name == IO8Name) return IO8Label;

            if (name == "transmit") return "Transmit";

            return name;
        }
    }
}
