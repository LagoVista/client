using LagoVista.XPlat.Core.Services;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core
{
    public class Picker : Xamarin.Forms.Picker
    {
        public Picker()
        {
            if (ResourceSupport.UseCustomColors)
            {
                TextColor = ResourceSupport.GetColor("EditControlText");
                BackgroundColor = ResourceSupport.GetColor("EditControlBackground");
            }

            if (ResourceSupport.UseCustomfonts)
            {
                FontFamily = ResourceSupport.GetString("EntryFont");
                FontSize = ResourceSupport.GetNumber("EntryFontSize");
            }
        }
    }
}
