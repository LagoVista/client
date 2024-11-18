using LagoVista.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LagoVista.Core.WPF.PlatformSupport
{
    public class DispatcherServices : IDispatcherServices
    {
        Dispatcher _dispatcher;
        public DispatcherServices(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Invoke(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }
    }
}
