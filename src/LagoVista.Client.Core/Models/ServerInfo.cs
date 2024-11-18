using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Models
{
    public class ServerInfo
    {
        public bool SSL { get; set; }
        public string RootUrl { get; set; }
        public int? Port { get; set; }

        public Uri BaseAddress
        {
            get
            {
                var protocol = "http";
                if (SSL)
                    protocol += "s";

                var path = $"{protocol}://{RootUrl}";
                if (Port.HasValue)
                    path = $"{path}:{Port}";
                
                return new Uri(path);
            }
        }
    }
}
