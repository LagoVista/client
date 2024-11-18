using LagoVista.XPlat.Core.Services;
using System;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.Common
{
    public class SideMenuItem : Grid
    {
        Label _menuText;
        Icon _icon;

        LagoVista.Client.Core.ViewModels.MenuItem _menuItem;

        public SideMenuItem(LagoVista.Client.Core.ViewModels.MenuItem menuItem)
        {
            HeightRequest = 32;

            _icon = new Icon();
            _icon.HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false);
            _icon.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false);
            _icon.FontSize = ResourceSupport.GetNumber("MenuFontSize");
          
            if(Device.RuntimePlatform != Device.Android)
                _icon.Margin = new Thickness(8, 4, 0, 0);

            _icon.TextColor = ResourceSupport.AccentColor;
            _icon.IconKey = menuItem.FontIconKey;
            _menuItem = menuItem;

            _menuItem.Command.CanExecuteChanged += Command_CanExecuteChanged;

            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(36) });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            _menuText = new Label();
            _menuText.VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false);
            _menuText.FontSize = ResourceSupport.GetNumber("MenuFontSize");
            _menuText.SetValue(Grid.ColumnProperty, 1);
            _menuText.Text = menuItem.Name;

            Children.Add(_icon);
            Children.Add(_menuText);

            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += TapRecognizer_Tapped;
            this.GestureRecognizers.Add(tapRecognizer);

            if (!_menuItem.Command.CanExecute(null))
            {
                _icon.Opacity = 0.5;
                _menuText.Opacity = 0.5;
            }
            else
            {
                _icon.Opacity = 1;
                _menuText.Opacity = 1;
            }

            if (ResourceSupport.UseCustomfonts)
            {
                _menuText.FontFamily = ResourceSupport.GetString("MenuFont");
            }

            if (ResourceSupport.UseCustomColors)
            {
                _icon.TextColor = ResourceSupport.GetColor("MenuBarForeground");
                _menuText.TextColor = ResourceSupport.GetColor("MenuBarForeground");
            }
        }

        private void TapRecognizer_Tapped(object sender, EventArgs e)
        {
            if (_menuItem.Command.CanExecute(_menuItem.CommandParameter))
            {
                _menuItem.RaiseMenuItemTapped();
                _menuItem.Command.Execute(_menuItem.CommandParameter);
            }
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            if(!_menuItem.Command.CanExecute(_menuItem.CommandParameter))
            {
                _icon.Opacity = 0.5;
                _menuText.Opacity = 0.5;
            }
            else
            {
                _icon.Opacity = 1;
                _menuText.Opacity = 1;
            }
        }
    }
}

