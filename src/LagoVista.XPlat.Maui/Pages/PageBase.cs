using LagoVista.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.XPlat.Maui.Pages
{
    public class PageBase : ContentPage
    {
        public PageBase()
        {
            NavigationPage.SetHasNavigationBar(this, true);

            this.Loaded += PageBase_Loaded;
        
        }

        private async void PageBase_Loaded(object? sender, EventArgs e)
        {
            var vm = BindingContext as ViewModelBase;
            await vm.InitAsync();
        }
    }
}
