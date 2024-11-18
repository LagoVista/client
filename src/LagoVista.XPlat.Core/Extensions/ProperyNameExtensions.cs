using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.XPlat.Core
{
    public static class ProperyNameExtensions
    {
        public static string ToPropertyName(this string name)
        {
            /* Property Names are mainly used for JSON, so they are in the format "myProperty", the actual name of the property is "MyProperty" this will convert the key name back to a property */

            return $"{name.Substring(0, 1).ToUpper()}{name.Substring(1)}";
        }

        public static string ToJSONName(this string name)
        {
            /* Property Names are mainly used for JSON, so they are in the format "myProperty", the actual name of the property is "MyProperty" this will convert the key name back to a property */

            return $"{name.Substring(0, 1).ToLower()}{name.Substring(1)}";
        }
    }
}
