using LagoVista.Core.Models.UIMetaData;
using System;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.FormControls
{
    public class CheckBoxRow : FormControl
    {
        FormFieldHeader _header;
        Switch _switch;

        public event EventHandler<OptionSelectedEventArgs> OptionSelected;

        public CheckBoxRow(FormViewer formViewer, FormField field) : base(formViewer, field)
        {
            Orientation = StackOrientation.Horizontal;

            _header = new FormFieldHeader(field.Label);

            _switch = new Switch();
            _switch.Margin = new Thickness(10, 0, 0, 0);

            _switch.Toggled += _switch_Toggled;

            bool _isToggled = false;
            if (bool.TryParse(field.Value, out _isToggled))
            {
                _switch.IsToggled = _isToggled;
            }

            Children.Add(_header);
            Children.Add(_switch);
            Margin = RowMargin;
        }

        private void _switch_Toggled(object sender, ToggledEventArgs e)
        {
            Field.Value = e.Value ? "true" : "false";
            IsDirty = OriginalValue != Field.Value;

            OptionSelected?.Invoke(this, new OptionSelectedEventArgs() { Key = Field.Name.ToPropertyName(), Value = Field.Value });
        }

        public override bool Validate()
        {
            return true;
        }
    }
}
