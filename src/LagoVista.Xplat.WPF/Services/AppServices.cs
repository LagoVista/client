using LagoVista.Client.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace LagoVista.XPlat.WPF.Services
{
    public class AppServices : IAppServices
    {
        public string AppInstallDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
    }
}
