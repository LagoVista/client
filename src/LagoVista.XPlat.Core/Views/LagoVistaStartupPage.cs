using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.ViewModels;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Views
{
    public class LagoVistaStartupPage : ContentPage, ILagoVistaPage
    {
        public XPlatViewModel ViewModel
        {
            get { return BindingContext as XPlatViewModel; }
            set { BindingContext = value; }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel != null)
            {
                await ViewModel.InitAsync();
            }
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            if (ViewModel != null)
            {
                await ViewModel.IsClosingAsync();
            }
        }
    }
}
