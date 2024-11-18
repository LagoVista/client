using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LagoVista.XPlat.WPF.Services
{
    public class DebugLogger : ILogger
    {
        public bool DebugMode { get; set; }
        private void WriteTags(KeyValuePair<string, string>[] args)
        {
            foreach(var arg in  args)
            {
                Debug.WriteLine($"\t{arg.Key} - {arg.Value}");
            }
        }


        public void AddCustomEvent(LogLevel level, string tag, string customEvent, params KeyValuePair<string, string>[] args)
        {
            Debug.WriteLine($"{level} - {tag} {customEvent}");
            WriteTags(args);
        }

        public void AddException(string tag, Exception ex, params KeyValuePair<string, string>[] args)
        {
            Debug.WriteLine($"{tag} {ex.Message}");
            Debug.WriteLine(ex.StackTrace);
            WriteTags(args);
        }

        public void AddKVPs(params KeyValuePair<string, string>[] args)
        {
        }

        public void EndTimedEvent(TimedEvent evt)
        {
        }

        public TimedEvent StartTimedEvent(string area, string description)
        {
            return new TimedEvent(area, description);
        }

        public void TrackEvent(string message, Dictionary<string, string> parameters)
        {
        }

        public void TrackMetric(string kind, string name, MetricType metricType, double count, params KeyValuePair<string, string>[] args)
        {
            throw new NotImplementedException();
        }

        public void TrackMetric(string kind, string name, MetricType metricType, int count, params KeyValuePair<string, string>[] args)
        {
            throw new NotImplementedException();
        }

        public void Trace(string message, params KeyValuePair<string, string>[] args)
        {
            throw new NotImplementedException();
        }
    }
}
