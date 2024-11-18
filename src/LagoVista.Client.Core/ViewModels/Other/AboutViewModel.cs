using LagoVista.Core.Commanding;
using LagoVista.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.ViewModels.Other
{
    public class AboutViewModel : AppViewModelBase
    {
        public AboutViewModel()
        {
            ShowCompanySiteCommand = new RelayCommand(() => LagoVista.Core.PlatformSupport.Services.Network.OpenURI(new Uri(AppConfig.CompanySiteLink)));
            ShowWebAddressCommand = new RelayCommand(() => LagoVista.Core.PlatformSupport.Services.Network.OpenURI(new Uri(AppConfig.WebAddress)));
            ShowTermsAndConditionCommand = new RelayCommand(() => LagoVista.Core.PlatformSupport.Services.Network.OpenURI(new Uri(AppConfig.TermsAndConditionsLink)));
            ShowPrivacyStatementCommand = new RelayCommand(() => LagoVista.Core.PlatformSupport.Services.Network.OpenURI(new Uri(AppConfig.PrivacyStatementLink)));
        }

        public RelayCommand ShowCompanySiteCommand { get; private set; }
        public RelayCommand ShowWebAddressCommand { get; private set; }
        public RelayCommand ShowTermsAndConditionCommand { get; private set; }
        public RelayCommand ShowPrivacyStatementCommand { get; private set; }

        public string Version
        {
            get { return $"{AppConfig.Version.Major}.{AppConfig.Version.Minor}.{AppConfig.Version.Build}.{AppConfig.Version.Revision}"; }
        }

    }
}
