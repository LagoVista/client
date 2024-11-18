using LagoVista.IoT.DeviceAdmin.Models;
using System;

namespace LagoVista.Client.Core.Models
{
    public class InputCommandParameter
    {
        Parameter _parameter;

        public InputCommandParameter(Parameter parameter)
        {
            _parameter = parameter;
            Label = parameter.Name;
            Value = string.Empty;
        }

        public string Label { get; set; }

        public string Value { get; set; }

        public string ToQueryStringPair()
        {
            if (String.IsNullOrEmpty(Value))
            {
                return String.Empty;
            }
            else
            {
                return $"{_parameter.Key}={System.Net.WebUtility.UrlEncode(Value)}";
            }
        }
    }
}
