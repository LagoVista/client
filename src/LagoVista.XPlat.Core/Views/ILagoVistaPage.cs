using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.XPlat.Core.Views
{
    public interface ILagoVistaPage
    {
        XPlatViewModel ViewModel { get; set; }
    }
}
