using LagoVista.Client.Core.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LagoVista.XPlat.Core.Extensions
{
    [ContentProperty("Text")]
    public class ClientTranslateExtension : IMarkupExtension
    {

        // Look at: poeditor.com 

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;

            return ClientResources.ResourceManager.GetString(Text, CultureInfo.CurrentCulture);
        }
    }
}
