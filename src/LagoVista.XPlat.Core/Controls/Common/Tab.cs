using LagoVista.Core.Commanding;
using LagoVista.Core.Interfaces;
using LagoVista.XPlat.Core.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core
{
    public class Tab : Grid
    {
        Icon _icon;
        Label _label;
        BoxView _background;
        public event EventHandler TabTapped;

        public Tab()
        {
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

            Margin = 6;

            _label = new Label()
            {
                FontSize = ResourceSupport.GetNumber("TabBarFontSize"),
                Margin = 4,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            _icon = new Icon()
            {
                FontSize = ResourceSupport.GetNumber("TabBarIconFontSize"),
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalTextAlignment = TextAlignment.Center,
            };

            _icon.SetValue(Grid.RowProperty, 0);
            _label.SetValue(Grid.RowProperty, 1);

            _background = new BoxView();
            _background.SetValue(Grid.RowSpanProperty, 2);
            _background.CornerRadius = new CornerRadius(4);
            _background.WidthRequest = 64;

            Children.Add(_background);
            Children.Add(_icon);
            Children.Add(_label);

            var tapGestureRecognizer = new TapGestureRecognizer() { Command = new RelayCommand(Tapped) };
            _background.GestureRecognizers.Add(tapGestureRecognizer);
            _icon.GestureRecognizers.Add(tapGestureRecognizer);
            _label.GestureRecognizers.Add(tapGestureRecognizer);
            this.GestureRecognizers.Add(tapGestureRecognizer);


            if (ResourceSupport.UseCustomColors)
            {
                _label.TextColor = ResourceSupport.GetColor(nameof(IAppStyle.TabForground));
                _icon.TextColor = ResourceSupport.GetColor(nameof(IAppStyle.TabForground));
            }

            if (ResourceSupport.UseCustomfonts)
            {
                _label.FontFamily = ResourceSupport.GetString("TabBarFont");
            }
        }
            void Tapped()
        {
            TabTapped?.Invoke(this, null);
            if (TappedCommand != null && TappedCommand.CanExecute(null))
            {
                TappedCommand.Execute(null);
            }
        }

        public static readonly BindableProperty TappedCommandProperty = BindableProperty.Create(nameof(TappedCommand), typeof(ICommand), typeof(Tab), null, BindingMode.TwoWay, null,
                (view, oldValue, newValue) => (view as Tab).TappedCommand = (ICommand)newValue, null, null, null);

        public ICommand TappedCommand
        {
            get { return GetValue(TappedCommandProperty) as ICommand; }
            set { SetValue(TappedCommandProperty, value); }
        }

        public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(Tab), String.Empty, BindingMode.TwoWay, null,
                (view, oldValue, newValue) => (view as Tab)._icon.IconKey = (string)newValue, null, null, null);

        public string Icon
        {
            get { return GetValue(IconProperty) as string; }
            set { SetValue(IconProperty, value); }
        }

        public static readonly BindableProperty LabelProperty = BindableProperty.Create(nameof(Label), typeof(string), typeof(Tab), String.Empty, BindingMode.TwoWay, null,
                (view, oldValue, newValue) => (view as Tab)._label.Text = (string)newValue, null, null, null);

        public string Label
        {
            get { return GetValue(LabelProperty) as String; }
            set { SetValue(LabelProperty, value); }
        }


        public static readonly BindableProperty SelectedProperty = BindableProperty.Create(nameof(Label), typeof(bool), typeof(Tab), false, BindingMode.TwoWay, null,
            (view, oldValue, newValue) => (view as Tab).Selected = (bool)newValue, null, null, null);

        public bool Selected
        {
            get => (bool)GetValue(SelectedProperty);
            set 
            {
                SetValue(SelectedProperty, value);

                _label.FontAttributes = value ? FontAttributes.Bold : FontAttributes.None;
                _icon.FontAttributes = value ? FontAttributes.Bold : FontAttributes.None;

                if (ResourceSupport.UseCustomColors)
                {
                    _background.BackgroundColor = value ? ResourceSupport.GetColor(nameof(IAppStyle.TabBackgroundActive)) : ResourceSupport.GetColor(nameof(IAppStyle.TabBackground));
                    _background.BackgroundColor = value ? ResourceSupport.GetColor(nameof(IAppStyle.TabBackgroundActive)) : ResourceSupport.GetColor(nameof(IAppStyle.TabBackground));
                    _label.TextColor = value ? ResourceSupport.GetColor(nameof(IAppStyle.TabForgroundActive)) : ResourceSupport.GetColor(nameof(IAppStyle.TabForground));
                    _icon.TextColor = _label.TextColor;
                }
            }
        }
    }
}
