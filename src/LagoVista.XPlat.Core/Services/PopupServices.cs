using LagoVista.Client.Core.Resources;
using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Services
{
    public class PopupServices : IPopupServices
    {
        public async Task<bool> ConfirmAsync(string title, string prompt)
        {
            return await Application.Current.MainPage.DisplayAlert(String.Empty, prompt, ClientResources.Common_Yes, ClientResources.Common_No);

        }

        public Task<double?> PromptForDoubleAsync(string label, double? defaultvalue = default(double?), string help = "", bool isRequired = false)
        {
            throw new NotImplementedException();
        }

        public Task<int?> PromptForIntAsync(string label, int? defaultvalue = default(int?), string help = "", bool isRequired = false)
        {
            throw new NotImplementedException();
        }

        public Task<string> PromptForStringAsync(string label, string defaultvalue = null, string help = "", bool isRequired = false)
        {
            throw new NotImplementedException();
        }

        public Task ShowAsync(string title, string message)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, ClientResources.Common_OK);
        }

        public Task ShowAsync(string message)
        {
            return Application.Current.MainPage.DisplayAlert(String.Empty, message, ClientResources.Common_OK);
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
