using System;
using System.Globalization;
using Xamarin.Forms;
using LagoVista.Core;

namespace LagoVista.XPlat.Core.ValueConverter
{
    public class DatePrinterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                if (!String.IsNullOrEmpty(strValue))
                {

                    try
                    {
                        return strValue.ToDateTime().ToLocalTime().ToString();
                    }
                    catch(Exception)
                    {
                        return $"invalid date format: {strValue}";
                    }
                }
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
