using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Devices.Models
{
    public class DataStreamResult : Dictionary<string, object>
    {
        public DataStreamResult()
        {
        
        }

        public string Timestamp { get; set; }
    }
}
