using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Core.WPF.PlatformSupport
{
    public class TimerFactory : ITimerFactory
    {
        public ITimer Create(TimeSpan interval)
        {
            return new Timer() { Interval = interval, InvokeOnUIThread = false };
        }
    }
}
