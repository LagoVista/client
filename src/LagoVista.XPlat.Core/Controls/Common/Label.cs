using LagoVista.XPlat.Core.Services;

namespace LagoVista.XPlat.Core
{
    public class Label : Xamarin.Forms.Label
    {
        public Label()
        {
            if (ResourceSupport.UseCustomfonts)
            {
                FontFamily = ResourceSupport.GetString("LabelFont");
                FontSize = ResourceSupport.GetNumber("LabelFontSize");
            }

            if (ResourceSupport.UseCustomColors)
            {
                TextColor = ResourceSupport.GetColor("LabelText");
            }
        }
    }
}
