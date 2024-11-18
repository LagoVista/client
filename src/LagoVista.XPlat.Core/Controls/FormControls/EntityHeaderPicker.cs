using LagoVista.Core.Models.UIMetaData;
using System;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.FormControls
{
    public class EntityHeaderPicker : FormControl
    {
        public event EventHandler<string> PickerTapped;

        FormFieldHeader _header;
        HyperLinkLabel _linkLabel;

        public EntityHeaderPicker(FormViewer formViewer, FormField field) : base(formViewer, field)
        {
            _header = new FormFieldHeader(field.Label);

            _linkLabel = new HyperLinkLabel();
            Children.Add(_header);

            Children.Add(_header);
            Children.Add(_linkLabel);

            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += TapRecognizer_Tapped;
            _linkLabel.GestureRecognizers.Add(tapRecognizer);
            
            Margin = RowMargin;

            Refresh();
        }

        public override void Refresh()
        {
            if (string.IsNullOrEmpty(Field.Value))
            {
                _linkLabel.Text = Field.Watermark;
            }
            else
            {
                _linkLabel.Text = Field.Display;
            }
        }

        private void TapRecognizer_Tapped(object sender, System.EventArgs e)
        {
            PickerTapped?.Invoke(this, Field.Name.ToPropertyName());
        }

        public override bool Validate()
        {
            return !(String.IsNullOrEmpty(Field.Value)) || !Field.IsRequired;
        }
    }
}