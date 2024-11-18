using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.FormControls
{
    public class FormFieldHeader : Label
    {
        public FormFieldHeader() : base()
        {
            Margin = new Thickness(8, 5, 0, -5);
        }

        public FormFieldHeader(string value) : this()
        {
            Text = value;
        }
    }
}