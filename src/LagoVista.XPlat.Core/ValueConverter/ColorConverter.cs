using LagoVista.Core.Models;
using LagoVista.IoT.DeviceManagement.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.ValueConverter
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Xamarin.Forms.Color.Black;
            }

            switch (value.ToString().ToLower())
            {
                case "red": return Xamarin.Forms.Color.Red;
                case "brown": return Xamarin.Forms.Color.Brown;
                case "green": return Xamarin.Forms.Color.Green;
                case "blue": return Xamarin.Forms.Color.Blue;
                case "yellow": return Xamarin.Forms.Color.Yellow;
            }

            return Xamarin.Forms.Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RedGreenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (System.Convert.ToBoolean(value))
            {
                return Xamarin.Forms.Color.Green;
            }
            else
            {
                return Xamarin.Forms.Color.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SensorStateForegroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var eh = value as EntityHeader<SensorStates>;
            var sensorState = SensorStates.Nominal;
            if (eh != null)
            {
                sensorState = eh.Value;
                return Xamarin.Forms.Color.FromRgb(0x55, 0xA9, 0xF2);
            }
            else
            {
                sensorState = (SensorStates)value;
            }

            switch (sensorState)
            {
                case SensorStates.Error:
                    return Xamarin.Forms.Color.White;
                case SensorStates.Warning:
                    return Xamarin.Forms.Color.White;
                case SensorStates.Nominal:
                    return Xamarin.Forms.Color.FromRgb(0x21, 0x21, 0x21);
                case SensorStates.Offline:
                    return Xamarin.Forms.Color.FromRgb(0x21, 0x21, 0x21);
                default:
                    return Xamarin.Forms.Color.FromRgb(0x21, 0x21, 0x21);

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

public class SensorStateBackgroundColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var eh = value as EntityHeader<SensorStates>;
        var sensorState = SensorStates.Nominal;
        if (eh != null)
        {
            sensorState = eh.Value;
            return Xamarin.Forms.Color.FromRgb(0x55, 0xA9, 0xF2);
        }
        else
        {
            sensorState = (SensorStates)value;
        }
        switch (sensorState)
        {
            case SensorStates.Error:
                return Xamarin.Forms.Color.FromRgb(0xE9, 0x5C, 0x5D);
            case SensorStates.Warning:
                return Xamarin.Forms.Color.FromRgb(0xFF, 0xC8, 0x7F);
            case SensorStates.Nominal:
                return Xamarin.Forms.Color.FromRgb(0x55, 0xA9, 0xF2);
            case SensorStates.Offline:
                return Xamarin.Forms.Color.FromRgb(0x55, 0xA9, 0xF2);
            default:
                return Xamarin.Forms.Color.FromRgb(0x55, 0xA9, 0xF2);
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
}
