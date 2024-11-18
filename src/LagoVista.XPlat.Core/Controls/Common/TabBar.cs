using LagoVista.Client.Core.Resources;
using LagoVista.Core.Interfaces;
using LagoVista.XPlat.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core
{
    public class TabBar : Grid
    {
        public event EventHandler<Tab> SelectedTabChanged;

        readonly ObservableCollection<Tab> _tabs;
        public TabBar()
        {
            ChildAdded += TabBar_ChildAdded;
            _tabs = new ObservableCollection<Tab>();

            if (ResourceSupport.UseCustomColors)
            {
                BackgroundColor = ResourceSupport.GetColor(nameof(IAppStyle.TabBackground));
            }
        }
        private void TabBar_ChildAdded(object sender, ElementEventArgs e)
        {
            IsVisible = true;
            var tab = Children.Last() as Tab;
            if (tab == null)
            {
                throw new ArgumentException("Should only add tabs to tab bar.");
            }

            tab.TabTapped += Tab_TabTapped;

            _tabs.Add(tab);

            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            tab.SetValue(Grid.ColumnProperty, _tabs.Count() - 1);
        }

        private void Tab_TabTapped(object sender, EventArgs e)
        {
            var tappedTab = sender as Tab;
            if (!tappedTab.Selected)
            {
                foreach (var child in Children)
                {
                    var tab = child as Tab;
                    tab.Selected = false;
                }

                tappedTab.Selected = true;

                SelectedTabChanged?.Invoke(this, tappedTab);
            }
        }

        public ObservableCollection<Tab> Tabs
        {
            get => _tabs;
        }
    }
}
