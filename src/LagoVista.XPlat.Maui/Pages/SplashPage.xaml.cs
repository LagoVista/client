using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.ViewModels;
using LagoVista.XPlat.Maui.Services;

namespace LagoVista.XPlat.Maui.Pages;

public partial class SplashPage : PageBase
{
	public SplashPage(SplashViewModel vm, IViewModelNavigation vmNav)
	{
		InitializeComponent();
		BindingContext = vm;
		var navProvider =vmNav as ViewModelNavigation;
		navProvider.SetHost(this.Navigation);
	}
}