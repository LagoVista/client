using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LagoVista.XPlat.Core.Views.Other
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutView : LagoVistaContentPage
    {
		public AboutView ()
		{
			InitializeComponent ();
		}
	}
}