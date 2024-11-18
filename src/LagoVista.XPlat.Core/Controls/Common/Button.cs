using LagoVista.XPlat.Core.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core
{
    public class Button : Xamarin.Forms.Button
    {
        public Button()
        {
            if (ResourceSupport.UseCustomColors)
            {
                BackgroundColor = ResourceSupport.GetColor("ButtonBackground");
                TextColor = ResourceSupport.GetColor("ButtonForeground");
            }

            if (ResourceSupport.UseCustomfonts)
            {
                FontSize = ResourceSupport.GetNumber("ButtonFontSize");
            }
        }
    }
}
