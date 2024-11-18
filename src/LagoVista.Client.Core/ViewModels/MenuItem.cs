using LagoVista.Core.Commanding;
using LagoVista.Core.ViewModels;
using System;
using System.Windows.Input;

namespace LagoVista.Client.Core.ViewModels
{
    public class MenuItem
    {
        public event EventHandler<MenuItem> MenuItemTapped;

        public String FontIconKey { get; set; }

        public String Name { get; set; }
        public string Details { get; set; }
        public string Help { get; set; }

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public void RaiseMenuItemTapped()
        {
            MenuItemTapped?.Invoke(this, this);
        }
    }

    public class MenuItem<TViewModel> : MenuItem where TViewModel : ViewModelBase
    {
        public MenuItem(IViewModelNavigation nav, ViewModelBase vm)
        {
            Command = new RelayCommand(() => nav.NavigateAsync<TViewModel>(vm));
        }
    }

}
