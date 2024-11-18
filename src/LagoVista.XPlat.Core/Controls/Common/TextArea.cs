using LagoVista.XPlat.Core.Services;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core
{
    public class TextArea : Xamarin.Forms.Editor
    {
        public TextArea()
        {
            if (ResourceSupport.UseCustomfonts)
            {
                FontFamily = ResourceSupport.GetString("EntryFont");
                FontSize = ResourceSupport.GetNumber("EntryFontSize");
            }
           // PlaceholderColor = ResourceSupport.GetColor("EditControlPlaceholder");
           // BackgroundColor = ResourceSupport.GetColor("EditControlBackground");
           // TextColor = ResourceSupport.GetColor("EditControlText");
        }
    }
}
