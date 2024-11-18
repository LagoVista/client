using LagoVista.XPlat.Core.Services;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.FormControls
{
    public class FormFieldValidationMessage : Label
    {
        public FormFieldValidationMessage() : base()
        {
            TextColor = ResourceSupport.GetColor("InvalidColor");
            IsVisible = false;
        }

        public FormFieldValidationMessage(string value) : base()
        {
            TextColor = ResourceSupport.GetColor("InvalidColor");
            IsVisible = false;
            Text = value;
        }
    }
}