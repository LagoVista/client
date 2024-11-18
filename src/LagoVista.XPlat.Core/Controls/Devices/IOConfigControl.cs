using System;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.Devices
{
    public class IOConfigControl : Grid
    {
        public enum ConfigControlTypes
        {
            NotSpecified,
            ADC,
            IO
        }

        private Picker _configType;
        private Entry _configLabel;
        private Entry _configName;
        private Entry _configScaler;

        private Label _configTypeLabel = new Label() { Text = "Type" };
        private Label _configLabelLabel = new Label() { Text = "Label" };
        private Label _configNameLabel = new Label() { Text = "Name" };
        private Label _configScalerLabel = new Label() { Text = "Scaler" };

        public IOConfigControl()
        {
            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

            RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            _configType = new Picker();
            _configLabel = new Entry();
            _configName = new Entry();
            _configScaler = new Entry() { HorizontalTextAlignment = TextAlignment.End };

            _configType.SetValue(Grid.ColumnProperty, 0);
            _configType.SelectedIndexChanged += _configType_SelectedIndexChanged;
            _configLabel.SetValue(Grid.ColumnProperty, 1);
            _configName.SetValue(Grid.ColumnProperty, 2);
            _configScaler.SetValue(Grid.ColumnProperty, 3);

            _configLabel.SetValue(Grid.RowProperty, 1);
            _configLabel.TextChanged += _configLabel_TextChanged;
            _configScaler.SetValue(Grid.RowProperty, 1);
            _configScaler.TextChanged += _configScaler_TextChanged;
            _configScaler.Focused += _configScaler_Focused;
            _configName.SetValue(Grid.RowProperty, 1);
            _configName.TextChanged += _configName_TextChanged;
            _configType.SetValue(Grid.RowProperty, 1);

            _configLabelLabel.SetValue(Grid.ColumnProperty, 1);
            _configNameLabel.SetValue(Grid.ColumnProperty, 2);
            _configScalerLabel.SetValue(Grid.ColumnProperty, 3);

            _configType.Margin = new Thickness(5);
            _configLabel.Margin = new Thickness(5);
            _configName.Margin = new Thickness(5);
            _configScaler.Margin = new Thickness(5);

            Children.Add(_configTypeLabel);
            Children.Add(_configLabelLabel);
            Children.Add(_configNameLabel);
            Children.Add(_configScalerLabel);

            Children.Add(_configType);
            Children.Add(_configLabel);
            Children.Add(_configName);
            Children.Add(_configScaler);
        }

        private void _configScaler_Focused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused && String.IsNullOrEmpty(_configScaler.Text))
            {
                _configScaler.Text = "1";
                SetValue(ScalerProperty, 1);
            }
        }

        private void _configName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetValue(NameProperty, _configName.Text);
        }

        private void _configScaler_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(_configScaler.Text))
            {
                if (float.TryParse(_configScaler.Text, out float newScalerValue))
                {
                    SetValue(ScalerProperty, newScalerValue);
                }
                else
                {
                    _configScaler.Text = Scaler.ToString();
                }
            }
        }

        private void _configLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetValue(LabelProperty, _configLabel.Text);
        }

        private void _configType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetValue(ConfigTypeProperty, _configType.SelectedIndex);
        }

        public static BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(IOConfigControl), null, BindingMode.TwoWay, null,
            (obj, oldValue, newValue) =>
            {
                var ctl = obj as IOConfigControl;

                if (newValue != null)
                {
                    ctl._configName.Text = newValue.ToString();
                }

            });


        public static BindableProperty LabelProperty = BindableProperty.Create("Label", typeof(string), typeof(IOConfigControl), null, BindingMode.TwoWay, null,
            (obj, oldValue, newValue) =>
            {
                var ctl = obj as IOConfigControl;

                if (newValue != null)
                {
                    ctl._configLabel.Text = newValue.ToString();
                }
            });

        public static BindableProperty ConfigTypeProperty = BindableProperty.Create("ConfigType", typeof(int), typeof(IOConfigControl), null, BindingMode.TwoWay, null,
            (obj, oldValue, newValue) =>
            {
                var ctl = obj as IOConfigControl;
                ctl.ConfigType = (int)newValue;

            });

        public static BindableProperty ScalerProperty = BindableProperty.Create("Scaler", typeof(float), typeof(IOConfigControl), null, BindingMode.TwoWay, null,
           (obj, oldValue, newValue) =>
           {
               var ctl = obj as IOConfigControl;
               ctl._configScaler.Text = newValue.ToString();
           });

        public static BindableProperty ConfigControlTypeProperty = BindableProperty.Create("ConfigControlType", typeof(ConfigControlTypes), typeof(IOConfigControl), null, BindingMode.TwoWay, null,
           (obj, oldValue, newValue) =>
           {
               var ctl = obj as IOConfigControl;
               ctl.ConfigControlType = (ConfigControlTypes)newValue;
           });


        public string Name
        {
            set { SetValue(NameProperty, value); }
            get => GetValue(NameProperty) as string;
        }

        public string Label
        {
            set { SetValue(LabelProperty, value); }
            get => GetValue(LabelProperty) as string;
        }


        public float Scaler
        {
            set
            {
                SetValue(ScalerProperty, value);
            }
            get => (float)GetValue(ScalerProperty);
        }

        public int ConfigType
        {
            set
            {
                SetValue(ConfigTypeProperty, (int)value);
                _configType.SelectedIndex = value;
            }
            get => (int)GetValue(ConfigTypeProperty);
        }

        private ConfigControlTypes _configControlType;
        public ConfigControlTypes ConfigControlType
        {
            get { return (ConfigControlTypes)GetValue(ScalerProperty); }
            set
            {

                SetValue(ScalerProperty, value);

                _configControlType = value;
                _configType.Items.Clear();
                _configType.Items.Add("None");

                if (value == ConfigControlTypes.ADC)
                {
                    _configTypeLabel.Text = "ADC Type";
                    _configType.Items.Add("I2C ADC Voltage");
                    _configType.Items.Add("Current Transformer");
                    _configType.Items.Add("Onboard ADC Voltage");
                }
                else
                {
                    _configTypeLabel.Text = "IO Type";
                    _configType.Items.Add("Input");
                    _configType.Items.Add("Output");
                    _configType.Items.Add("Pulse Counter");
                    _configType.Items.Add("DS18B");
                    _configType.Items.Add("DHT11");
                    _configType.Items.Add("DHT22");
                }

                _configType.SelectedIndex = 0;
            }
        }
    }
}
