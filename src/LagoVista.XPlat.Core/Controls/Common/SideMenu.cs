using LagoVista.Client.Core.Resources;
using LagoVista.Core.Interfaces;
using LagoVista.XPlat.Core.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.Common
{
    public class SideMenu : ScrollView
    {
        StackLayout _container;
        public event EventHandler<Client.Core.ViewModels.MenuItem> MenuItemTapped;
        Label _orgLabel;
        IAuthManager _authManager;

        public SideMenu(IAuthManager authManager)
        {
            _container = new StackLayout();
            authManager.OrgChanged += AuthManager_OrgChanged;
            Content = _container;
            _authManager = authManager;

            if (ResourceSupport.UseCustomColors)
            {
                this.BackgroundColor = ResourceSupport.GetColor(nameof(IAppStyle.MenuBarBackground));
            }
            else
            {
                this.BackgroundColor = Color.FromRgb(0x3f, 0x3F, 0x3f);
            }
     
            //      this.SetOnAppTheme<Color>(SideMenu.BackgroundColorProperty, ResourceSupport.GetColor("MenuBarBackgroundLight"), ResourceSupport.GetColor("MenuBarBackgroundDark"));
        }

        private void AuthManager_OrgChanged(object sender, LagoVista.Core.Models.EntityHeader e)
        {
            _orgLabel.Text = e.Text;
        }

        IEnumerable<LagoVista.Client.Core.ViewModels.MenuItem> _menuItems;
        public IEnumerable<LagoVista.Client.Core.ViewModels.MenuItem> MenuItems
        {
            get { return _menuItems; }
            set
            {
                _container.Children.Clear();

                _orgLabel = new Label
                {
                    FontSize = ResourceSupport.GetNumber("HeaderFontSize"),
                };

                if(ResourceSupport.UseCustomColors)
                {
                    _orgLabel.TextColor = ResourceSupport.GetColor(nameof(IAppStyle.MenuBarForeground));
                }

                if (_authManager.IsAuthenticated)
                {
                    this.IsVisible = true;
                    _orgLabel.Text = (_authManager.User.CurrentOrganization != null) ? _authManager.User.CurrentOrganization.Text : ClientResources.MainMenu_NoOrganization;
                }
                else
                {
                    this.IsVisible = false;
                }

                _orgLabel.Margin = new Thickness(44, 0, 0, 10);
                _container.Children.Add(_orgLabel);

                _menuItems = value;

                if (_menuItems != null)
                {
                    foreach (var menuItem in _menuItems)
                    {
                        menuItem.MenuItemTapped += MenuItem_MenuItemTapped;
                        _container.Children.Add(new SideMenuItem(menuItem));
                    }
                }

                if(ResourceSupport.UseCustomfonts)
                {
                    _orgLabel.FontFamily = ResourceSupport.GetString("HeaderFont");
                }
            }
        }

        private void MenuItem_MenuItemTapped(object sender, Client.Core.ViewModels.MenuItem e)
        {
            MenuItemTapped?.Invoke(sender, e);
        }
    }
}
