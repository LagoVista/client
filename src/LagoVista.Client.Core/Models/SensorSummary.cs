using LagoVista.Core.Models;
using LagoVista.Core.Models.Drawing;
using LagoVista.IoT.DeviceManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Models
{
    public class SensorSummaryX : ModelBase
    {
        public SensorSummaryX(Sensor sensor)
        {
            State = sensor.State.Value;
            Display = $"{sensor.Value} {sensor.UnitsLabel}";
            Name = sensor.Name;
        }

        SensorStates _sensorState;
        public SensorStates State
        {
            set { Set(ref _sensorState, value); }
            get => _sensorState;
        }

        private string _display;
        public string Display
        {
            get => _display;
            set => Set(ref _display, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
    }
}