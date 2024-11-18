using LagoVista.Client.Core.Models;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.Devices
{
    public class GATTAttributeEditor : ViewCell
    {
        Label _label;
        Label _readOnly;
        Xamarin.Forms.Entry _entry;
        CheckBox _checkBox;

        public GATTAttributeEditor()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(64, GridUnitType.Absolute) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(64, GridUnitType.Absolute) });
            grid.HeightRequest = 128;
            this.Height = 128;

            _entry = new Xamarin.Forms.Entry();// { FontSize = 14, Margin = new Thickness(4, 0, 0, 0), TextColor = Color.White };
            _entry.HorizontalOptions = LayoutOptions.FillAndExpand;
            _entry.TextColor = Color.White;
            _label = new Label();// { FontSize = 14, Margin = new Thickness(4, 0, 0, 0), TextColor = Color.White };
            _readOnly = new Label();// { FontSize = 14, Margin = new Thickness(4, 0, 0, 0), TextColor = Color.White };
      
            _label.SetValue(Grid.RowProperty, 0);
      //      _label.Margin = new Thickness(5);

            _entry.SetValue(Grid.RowProperty, 1);
            //_entry.Margin = new Thickness(5);
            _entry.TextChanged += _entry_TextChanged;

            _checkBox = new CheckBox();
            _checkBox.SetValue(Grid.RowProperty, 1);
            _checkBox.Margin = new Thickness(5);
            _checkBox.CheckedChanged += _checkBox_CheckedChanged;

            _readOnly.SetValue(Grid.RowProperty, 1);

            grid.Children.Add(_label);            
            grid.Children.Add(_checkBox);
            grid.Children.Add(_readOnly);
            grid.Children.Add(_entry);

            View = grid;
        }

        private void _entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Value = e.NewTextValue;
        }

        private void _checkBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Value = (e.Value ? 1 : 0).ToString();
        }

        public static BindableProperty AttributeTypeProperty = BindableProperty.Create(nameof(AttributeType), typeof(BLECharacteristicType), typeof(GATTAttributeEditor), null, BindingMode.TwoWay, null,
           (obj, oldValue, newValue) =>
           {
               var ctl = obj as GATTAttributeEditor;
               ctl._checkBox.IsVisible = false;
               ctl._entry.IsVisible = false;
               ctl._readOnly.IsVisible = false;

               switch (ctl.AttributeType)
               {
                   case BLECharacteristicType.Boolean:
                       ctl._checkBox.IsVisible = true;
                       break;
                   case BLECharacteristicType.String:
                   case BLECharacteristicType.Integer:
                   case BLECharacteristicType.Real:
                       ctl._entry.IsVisible = true;
                       break;
                   case BLECharacteristicType.Enum:
                   case BLECharacteristicType.StringArray:
                   case BLECharacteristicType.IntegerArray:
                   case BLECharacteristicType.RealArray:
                   case BLECharacteristicType.ByteArray:
                   case BLECharacteristicType.Command:
                       ctl._readOnly.IsVisible = true;
                       break;
               }
           });

        public static BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(GATTAttributeEditor), null, BindingMode.TwoWay, null,
           (obj, oldValue, newValue) =>
           {
               var ctl = obj as GATTAttributeEditor;

               switch (ctl.AttributeType)
               {
                   case BLECharacteristicType.Boolean:
                       ctl._checkBox.IsChecked = (ctl.Value == "1");

                       break;
                   case BLECharacteristicType.String:
                   case BLECharacteristicType.Integer:
                   case BLECharacteristicType.Real:
                       ctl._entry.Text = ctl.Value;
                       break;
                   case BLECharacteristicType.Enum:
                   case BLECharacteristicType.StringArray:
                   case BLECharacteristicType.IntegerArray:
                   case BLECharacteristicType.RealArray:
                   case BLECharacteristicType.ByteArray:
                   case BLECharacteristicType.Command:
                       ctl._readOnly.Text = $"{ctl.AttributeType} is not supported.";
                       break;
               }

           });

        public static BindableProperty LabelProperty = BindableProperty.Create(nameof(Label), typeof(string), typeof(GATTAttributeEditor), null, BindingMode.TwoWay, null,
           (obj, oldValue, newValue) =>
           {
               var ctl = obj as GATTAttributeEditor;
               ctl._label.Text = (string)newValue;
           });

        public BLECharacteristicType AttributeType
        {
            set { SetValue(AttributeTypeProperty, value); }
            get => (BLECharacteristicType)GetValue(AttributeTypeProperty);
        }

        public string Label
        {
            get => (string)GetValue(GATTAttributeEditor.LabelProperty);
            set => SetValue(GATTAttributeEditor.LabelProperty, value);
        }

        public string Value
        {
            get => (string)GetValue(GATTAttributeEditor.ValueProperty);
            set => SetValue(GATTAttributeEditor.ValueProperty, value);
        }
    }
}
