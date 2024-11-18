using LagoVista.Core.Models;
using System;

namespace LagoVista.Client.Core.Models
{
    public class LiveDataItem : ModelBase
    {
        public LiveDataItem(string key, string label, string value)
        {
            Key = key;
            Label = label;
            Value = value;
        }

        public string Key { get; set; }
        public string Label { get; set; }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                LastUpdated = DateTime.Now.ToString();
                Set(ref _value, value);
            }
        }

        private string _lastUpdated;
        public string LastUpdated
        {
            get { return _lastUpdated; }
            set { Set(ref _lastUpdated, value); }
        }
    }
}
