using LagoVista.Core.IOC;
using LagoVista.Core.ViewModels;
using LagoVista.XPlat.Maui.Pages;

namespace LagoVista.XPlat.Maui.Services
{
    public class ViewModelNavigation : IViewModelNavigation
    {
        private INavigation _host;
        private readonly Dictionary<Type, Type> _viewModelLookup = new Dictionary<Type, Type>();

        public void RegisterView<TViewModel, TView>() where TViewModel : ViewModelBase where TView : PageBase 
        {
            _viewModelLookup.Add(typeof(TViewModel), typeof(TView));
        }

        public void SetHost(INavigation navHost)
        {
            _host = navHost;
        }

        public bool CanGoBack()
        {
            throw new NotImplementedException();
        }

        public Task GoBackAsync()
        {
            throw new NotImplementedException();
        }

        public Task GoBackAsync(int dropPageCount)
        {
            throw new NotImplementedException();
        }

        public Task NavigateAndCreateAsync<TViewModel>(ViewModelBase parentViewModel, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task NavigateAndCreateAsync<TViewModel>(ViewModelBase parentViewModel, object parentModel, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task NavigateAndEditAsync<TViewModel>(ViewModelBase parentViewModel, object parentModel, object child, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task NavigateAndEditAsync<TViewModel>(ViewModelBase parentViewModel, object parentModel, string id, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task NavigateAndEditAsync<TViewModel>(ViewModelBase parentViewModel, string id, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task NavigateAndPickAsync<TViewModel>(ViewModelBase parentViewModel, Action<object> selectedAction, Action cancelledAction = null, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task NavigateAndViewAsync<TViewModel>(ViewModelBase parentViewModel, object parentModel, object child, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task NavigateAndViewAsync<TViewModel>(ViewModelBase parentViewModel, object parentModel, string id, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            throw new NotImplementedException();
        }

        public async Task NavigateAsync(ViewModelLaunchArgs args)
        {
            var viewModel = SLWIOC.CreateForType(args.ViewModelType) as ViewModelBase;
            if (viewModel != null)
            {
                viewModel.LaunchArgs = args;
            }

            var viewType = _viewModelLookup[args.ViewModelType];
            var pageInstance = SLWIOC.CreateForType(viewType);
            var page = pageInstance as PageBase;
            page.BindingContext = viewModel;
            await _host.PushAsync(page);
        }

        public async Task NavigateAsync(ViewModelBase parentViewModel, Type viewModelType, params KeyValuePair<string, object>[] args)
        {
            var viewType = _viewModelLookup[viewModelType];
            var viewModel = SLWIOC.CreateForType(viewModelType) as ViewModelBase;
            if (viewModel != null)
            {
                viewModel.LaunchArgs = new ViewModelLaunchArgs()
                {
                    Parent = parentViewModel,
                    LaunchType = LaunchTypes.Other
                };

                foreach (var arg in args)
                {
                    viewModel.LaunchArgs.Parameters.Add(arg.Key, arg.Value);
                }
            }

            var pageInstance = SLWIOC.CreateForType(viewType);
            var page = pageInstance as PageBase;
            page.BindingContext = viewModel;
            await _host.PushAsync(page);
        }

        public async Task NavigateAsync<TViewModel>(ViewModelBase parentViewModel, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var viewModelType = typeof(TViewModel);
            var viewType = _viewModelLookup[viewModelType];
            var viewModel = SLWIOC.CreateForType(viewModelType) as ViewModelBase;
            if (viewModel != null)
            {
                viewModel.LaunchArgs = new ViewModelLaunchArgs()
                {
                    Parent = parentViewModel,
                    LaunchType = LaunchTypes.Other
                };

                foreach (var arg in args)
                {
                    viewModel.LaunchArgs.Parameters.Add(arg.Key, arg.Value);
                }
            }

            var pageInstance = SLWIOC.CreateForType(viewType);
            var page = pageInstance as PageBase;
            page.BindingContext = viewModel;
            await _host.PushAsync(page);
        }

        public async Task SetAsNewRootAsync<TViewModel>(params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var viewModelType = typeof(TViewModel);
            var viewType = _viewModelLookup[viewModelType];
            var viewModel = SLWIOC.CreateForType(viewModelType) as ViewModelBase;
            if (viewModel != null)
            {
                viewModel.LaunchArgs = new ViewModelLaunchArgs()
                {
                    IsNewRoot = true,
                    LaunchType = LaunchTypes.Other
                };

                foreach (var arg in args)
                {
                    viewModel.LaunchArgs.Parameters.Add(arg.Key, arg.Value);
                }
            }

            var pageInstance = SLWIOC.CreateForType(viewType);
            var page = pageInstance as PageBase;
            page.BindingContext = viewModel;
            var navPage = new NavigationPage(page);
            Application.Current.MainPage = navPage;
            _host = navPage.Navigation;
        }

        public async Task SetAsNewRootAsync(Type viewModelType, params KeyValuePair<string, object>[] args)
        {
            var viewType = _viewModelLookup[viewModelType];
            var pageInstance = SLWIOC.CreateForType(viewType);
            var viewModel = SLWIOC.CreateForType(viewModelType) as ViewModelBase;
            if(viewModel != null)
            {
                viewModel.LaunchArgs = new ViewModelLaunchArgs()
                {
                    IsNewRoot = true,
                    LaunchType = LaunchTypes.Other
                };

                foreach (var arg in args)
                {
                    viewModel.LaunchArgs.Parameters.Add(arg.Key, arg.Value);
                }
            }

            var page = pageInstance as PageBase;
            page.BindingContext = viewModel;
            var navPage = new NavigationPage(page);
            Application.Current.MainPage = navPage;
            _host = navPage.Navigation;
        }
    }
}
