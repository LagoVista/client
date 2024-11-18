using LagoVista.Core.Models.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista
{
    public static class ColorExtension
    {
        public static Xamarin.Forms.Color ToXamFormsColor(this LagoVista.Core.Models.Drawing.Color color)
        {
            return Xamarin.Forms.Color.FromRgba(color.R, color.G, color.B, color.A);
        }

        public static void RegisterStyle(this Xamarin.Forms.Application app, LagoVista.Core.Interfaces.IAppStyle style)
        {
            var properties = style.GetType().GetProperties();
            foreach (var prop in properties)
            {
                if (!app.Resources.ContainsKey(prop.Name))
                {
                    if (prop.GetValue(style) is Color color)
                    {
                        app.Resources.Add(prop.Name, color.ToXamFormsColor());
                    }
                    else
                    {
                        app.Resources.Add(prop.Name, prop.GetValue(style));
                    }
                }
            }
        }
    }
}
