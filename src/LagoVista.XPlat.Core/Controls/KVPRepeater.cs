using LagoVista.XPlat.Core.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core
{
    public class KVPRepeater : StackLayout
    {
        public KVPRepeater()
        {
                LabelStyle = ResourceSupport.GetStyle("LabelContent");
                ValueStyle = ResourceSupport.GetStyle("ValueContent");
        }

        public static BindableProperty LabelStyleProperty = BindableProperty.Create(propertyName: nameof(LabelStyle), returnType: typeof(Style),
               declaringType: typeof(KVPRepeater), defaultValue: null, defaultBindingMode: BindingMode.Default,
               propertyChanged: (ctl, oldValue, newValue) => { (ctl as KVPRepeater).LabelStyle = newValue as Style; });

        public Style LabelStyle
        {
            get => GetValue(KVPRepeater.LabelStyleProperty) as Style;
            set => SetValue(KVPRepeater.LabelStyleProperty, value);
        }

        public static BindableProperty ValueStyleProperty = BindableProperty.Create(propertyName: nameof(ValueStyle), returnType: typeof(Style),
               declaringType: typeof(KVPRepeater), defaultValue: null, defaultBindingMode: BindingMode.Default,
               propertyChanged: (ctl, oldValue, newValue) => { (ctl as KVPRepeater).ValueStyle = newValue as Style; });

        public Style ValueStyle
        {
            get => GetValue(KVPRepeater.ValueStyleProperty) as Style;
            set => SetValue(KVPRepeater.ValueStyleProperty, value);
        }

        public static BindableProperty ItemSourceProperty = BindableProperty.Create(propertyName: nameof(ItemSource), returnType: typeof(IEnumerable<KeyValuePair<string, object>>),
               declaringType: typeof(FormViewer), defaultValue: null, defaultBindingMode: BindingMode.OneWay,
               propertyChanged:(ctl, oldValue, newValue) => { (ctl as KVPRepeater).Populate(newValue as IEnumerable<KeyValuePair<string, object>>); });

        private void Populate(IEnumerable<KeyValuePair<string, object>> value)
        {
            this.Children.Clear();
            if (value != null)
            {
                var grid = new Grid();
                for (var idx = 0; idx < value.Count(); ++idx)
                {
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                }

                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(33, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(66, GridUnitType.Star) });

                var rowIdx = 0;
                foreach (var pair in value)
                {
                    var label = new Label();
                    label.Style = LabelStyle;
                    label.SetValue(Grid.ColumnProperty, 0);
                    label.SetValue(Grid.RowProperty, rowIdx);

                    var valueLabel = new Label();
                    ValueStyle = ValueStyle;
                    valueLabel.SetValue(Grid.ColumnProperty, 1);
                    valueLabel.SetValue(Grid.RowProperty, rowIdx);

                    label.Text = pair.Key;
                    valueLabel.Text = (string.IsNullOrEmpty((string)pair.Value)) ? "?" : pair.Value.ToString();

                    grid.Children.Add(label);
                    grid.Children.Add(valueLabel);

                    ++rowIdx;
                }

                Children.Add(grid);
                Debug.WriteLine("X");
            }
        }

        public IEnumerable<KeyValuePair<string, object>> ItemSource
        {
            get { return (IEnumerable<KeyValuePair<string, object>>)base.GetValue(ItemSourceProperty); }
            set
            {
                base.SetValue(ItemSourceProperty, value);
                Populate(value);
            }
        }
    }
}
