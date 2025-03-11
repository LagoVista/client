using LagoVista.Core.Interfaces;
using LagoVista.Core.PlatformSupport;
using LagoVista.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.XPlat.Maui.Services
{
    public class PopupService : IPopupServices
    {
        public PopupService()
        {

        }

        public async Task<bool> ConfirmAsync(string title, string prompt)
        {
            return (await Application.Current.MainPage.DisplayActionSheet(title, prompt, null, "Yes", "No")) == "Yes";
        }

        public Task<double?> PromptForDoubleAsync(string label, double? defaultvalue = null, string help = "", bool isRequired = false)
        {
            throw new NotImplementedException();
        }

        public Task<int?> PromptForIntAsync(string label, int? defaultvalue = null, string help = "", bool isRequired = false)
        {
            throw new NotImplementedException();
        }

        public Task<string> PromptForStringAsync(string label, string defaultvalue = null, string help = "", bool isRequired = false)
        {
            throw new NotImplementedException();
        }

        public Task ShowAsync(string title, string message)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }

        public Task ShowAsync(string message)
        {
            return Application.Current.MainPage.DisplayAlert("Alert", message, "OK");
        }

        public Task<bool> ShowModalAsync<TModalWindow, TViewModel>(TViewModel viewModal)
            where TModalWindow : IModalWindow
            where TViewModel : IViewModel
        {
            throw new NotImplementedException();
        }

        public Task<string> ShowOpenFileAsync(string fileMask = "")
        {
            throw new NotImplementedException();
        }

        public Task<string> ShowSaveFileAsync(string fileMask = "", string defaultFileName = "")
        {
            throw new NotImplementedException();
        }
    }
}
