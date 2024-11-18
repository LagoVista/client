using LagoVista.Client.Core.Icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core
{
    public class Icon : Label
    {
        public Icon()
        {

        }

        public static BindableProperty IconKeyProperty = BindableProperty.Create(nameof(IconKey), typeof(string), typeof(Icon), default(string), BindingMode.Default, null,
            (view, oldValue, newValue) => (view as Icon).IconKey = (string)newValue);

        public string IconKey
        {
            get { return (string)GetValue(Icon.IconKeyProperty); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {

                }
                else
                {
                    SetValue(Icon.IconKeyProperty, value);

                    var icon = Iconize.FindIconForKey(value);
                    if (icon == null)
                    {
                        throw new Exception("Could not find icon for: " + value);
                    }

                    switch (Device.RuntimePlatform)
                    {
                        case Device.UWP: FontFamily = $"{Iconize.FindModuleOf(icon).FontPath}#{Iconize.FindModuleOf(icon).FontFamily}"; break;
                        case Device.iOS: FontFamily = Iconize.FindModuleOf(icon).FontName; break;
                        case Device.Android: FontFamily = $"{Iconize.FindModuleOf(icon).FontPath}#{Iconize.FindModuleOf(icon).FontFamily}"; break;
                    }

                    Text = $"{icon.Character}";
                }
            }
        }
    }
}
