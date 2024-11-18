using LagoVista.Core.IOC;
using LagoVista.Core.PlatformSupport;
using LagoVista.XPlat.Core.Services;
using System;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Views
{
    public class LagoVistaNavigationPage : NavigationPage
    {
        public LagoVistaNavigationPage(Page root) : base(root)
        {
            if (Device.RuntimePlatform != Device.UWP)
            {
                if (Device.RuntimePlatform != Device.UWP)
                {
                    BarBackgroundColor = ResourceSupport.GetColor("TitleBarBackground");
                    BarTextColor = ResourceSupport.GetColor("TitleBarText");
                    BackgroundColor = ResourceSupport.GetColor("PageBackground");
                }
            }
        }

        public void HandleURIActivation(Uri uri)
        {
            var logger = SLWIOC.Get<ILogger>();
            if (this.CurrentPage == null)
            {
                logger.AddCustomEvent(LogLevel.Error, "LagoVistaNavigationPage_HandleURIActivation", "Main Page Null");
            }
            else
            {
                var contentPage = this.CurrentPage as LagoVistaContentPage;
                if (contentPage != null)
                {
                    contentPage.HandleURIActivation(uri);
                }
                else
                {
                    logger.AddCustomEvent(LogLevel.Error, "App_OnActivated", "EventArgs Not ProtocolActivatedEventArgs", new System.Collections.Generic.KeyValuePair<string, string>("type", this.CurrentPage.GetType().Name));
                }
            }
        }
    }
}
