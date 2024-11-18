using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.Devices
{
    public class ComponentView : Grid
    {
        Label _label;
        Label _name;
        Label _dateStamp;
        Label _value;

        Frame _frame;

        BoxView _updated;

        // https://devblogs.microsoft.com/xamarin/xamarin-forms-shapes-and-paths/

        public ComponentView()
        {
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            _updated = new BoxView() { WidthRequest = 24, HeightRequest = 4, Opacity = 0, Margin = new Thickness(0, 0, 0, 4), BackgroundColor = Color.Green };

            Margin = 10;

            _label = new Label() { FontSize = 24, Margin = new Thickness(4, 0, 0, 0), TextColor = Color.Black };
            _name = new Label();
            _dateStamp = new Label() { VerticalTextAlignment = TextAlignment.End, Margin = new Thickness(4, 0, 0, 4), };
            _value = new Label() { FontSize = 24, VerticalTextAlignment = TextAlignment.Center, Margin = new Thickness(0, 0, 4, 0), HorizontalTextAlignment = TextAlignment.End, TextColor = Color.Black };

            _frame = new Frame() { BorderColor = Color.LightBlue, BackgroundColor = Color.LightGray, CornerRadius = 10 };

            _label.SetValue(Grid.RowProperty, 0);

            var updatedLayout = new StackLayout() { Orientation = StackOrientation.Horizontal };
            updatedLayout.Children.Add(_dateStamp);
            updatedLayout.Children.Add(_updated);

            _value.SetValue(Grid.ColumnProperty, 1);
            _value.SetValue(Grid.RowSpanProperty, 2);

            _frame.SetValue(Grid.RowSpanProperty, 2);
            _frame.SetValue(Grid.ColumnSpanProperty, 2);

            updatedLayout.SetValue(Grid.RowProperty, 1);

            Children.Add(_frame);
            Children.Add(_label);
            Children.Add(updatedLayout);
            Children.Add(_value);
        }

        public static BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(string), typeof(ComponentView), null, BindingMode.TwoWay, null,
           (obj, oldValue, newValue) =>
           {
               var ctl = obj as ComponentView;
               ctl.Value = newValue.ToString();
           });

        public static BindableProperty DateStampProperty = BindableProperty.Create(nameof(DateStamp), typeof(string), typeof(ComponentView), null, BindingMode.TwoWay, null,
           (obj, oldValue, newValue) =>
           {
               var ctl = obj as ComponentView;
               ctl.DateStamp = newValue.ToString();
           });



        public static BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(ComponentView), null, BindingMode.TwoWay, null,
           (obj, oldValue, newValue) =>
           {
               var ctl = obj as ComponentView;
               ctl.Name = newValue.ToString();
           });


        public static BindableProperty LabelProperty = BindableProperty.Create(nameof(Label), typeof(string), typeof(ComponentView), null, BindingMode.TwoWay, null,
           (obj, oldValue, newValue) =>
           {
               var ctl = obj as ComponentView;
               ctl.Label = newValue.ToString();
           });

        public string Value
        {
            get => (string)GetValue(ComponentView.ValueProperty);
            set
            {
                SetValue(ComponentView.ValueProperty, value);
                _value.Text = value.ToString();
            }
        }

        public string DateStamp
        {
            get => (string)GetValue(ComponentView.DateStampProperty);
            set
            {
                SetValue(ComponentView.DateStampProperty, value);
                _dateStamp.Text = DateTime.Now.ToString();
                _updated.Opacity = 0;
                _updated.FadeTo(1, 1000);
            }
        }

        public String Name
        {
            get => (string)GetValue(ComponentView.NameProperty);
            set
            {
                SetValue(ComponentView.NameProperty, value);
                _name.Text = value;
            }
        }

        public string Label
        {
            get => (string)GetValue(ComponentView.LabelProperty);
            set
            {
                SetValue(ComponentView.LabelProperty, value);
                _label.Text = value;
            }
        }
    }
}
