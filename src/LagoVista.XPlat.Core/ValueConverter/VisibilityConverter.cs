using System;
using System.Globalization;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.ValueConverter
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }

            if(value is System.Int32)
            {
                if((System.Int32)value == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return System.Convert.ToBoolean(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default(object);
        }
    }
}
