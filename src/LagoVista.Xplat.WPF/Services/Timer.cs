using LagoVista.Core.IOC;
using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Core.WPF.PlatformSupport
{
    public class Timer : ITimer
    {
        System.Threading.Timer _timer;

        public String _timerKey;

        public TimeSpan Interval { get; set; }

        public bool InvokeOnUIThread { get; set; }

        public object State { get; set; }

        public event EventHandler Tick;

        public void Dispose()
        {
            lock (this)
            {
                if (_timer != null)
                {
                    _timerKey = null;
                    _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }

        private void Timer_Elapsed(object state)
        {
            if (_timer == null || state == null || state.ToString() != _timerKey)
            {
                if (_timerKey != null)
                    Dispose();

                return;
            }

            if (InvokeOnUIThread)
            {
                Object objDispatcher;

                if (SLWIOC.TryResolve(typeof(IDispatcherServices), out objDispatcher))
                {
                    var dispatcher = objDispatcher as IDispatcherServices;
                    dispatcher.Invoke(() => Tick?.Invoke(this, null));
                }
                else
                {
                    throw new Exception("To invoke timer on UI thread you must register a type for IDispatcherService for your current platform.  This is usually done by calling [Platform]Serivces.Init(Dispatcher) for you platform.");
                }
            }
            else
            {
                Tick?.Invoke(this, null);
            }
        }

        public void Start()
        {
            if (_timer != null)
            {
                Dispose();
            }

            _timerKey = Guid.NewGuid().ToString();
            _timer = new System.Threading.Timer(Timer_Elapsed, _timerKey, Convert.ToInt32(Interval.TotalMilliseconds), Convert.ToInt32(Interval.TotalMilliseconds));
        }

        public void Stop()
        {
            Dispose();
        }
    }
}
