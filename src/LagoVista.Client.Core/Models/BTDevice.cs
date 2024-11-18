using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Models
{
    public class BTDevice
    {
        public string DeviceName { get; set; }
        public string DeviceId { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
