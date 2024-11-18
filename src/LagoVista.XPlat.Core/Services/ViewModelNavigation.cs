
using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.IOC;
using LagoVista.Core.ViewModels;
using LagoVista.XPlat.Core.Views;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;


namespace LagoVista.XPlat.Core.Services
{
    public class ViewModelNavigation : IViewModelNavigation
    {
        private readonly Dictionary<Type, Type> _viewModelLookup = new Dictionary<Type, Type>();

        public Stack<ViewModelBase> ViewModelBackStack { get; private set; }

        private readonly global::Xamarin.Forms.Application _app;
        global::Xamarin.Forms.INavigation _navigation;

        public ViewModelNavigation(global::Xamarin.Forms.Application app)
        {
            _app = app ?? throw new ArgumentNullException(nameof(app));
            ViewModelBackStack = new Stack<ViewModelBase>();
        }

        public void Start<V>() where V : XPlatViewModel
        {

            if (!_viewModelLookup.ContainsKey(typeof(V)))
            {
                throw new Exception($"Could not find initial view for view model type [{typeof(V).Name}");
            }

            var primaryViewType = _viewModelLookup[typeof(V)];

            var view = Activator.CreateInstance(primaryViewType) as LagoVistaContentPage;

            _navigation = view.Navigation;

            var viewModel = SLWIOC.CreateForType<V>();
            view.ViewModel = viewModel;
            _app.MainPage = new LagoVistaNavigationPage(view);
        }

        public void Add<T, V>() where T : ViewModelBase where V : ILagoVistaPage
        {
            _viewModelLookup.Add(typeof(T), typeof(V));
        }

        private Task ShowViewModelAsync(ViewModelLaunchArgs args)
        {
            if (args.ParentViewModel == null)
            {
                args.ParentViewModel = ViewModelBackStack.FirstOrDefault();
            }

            var view = Activator.CreateInstance(_viewModelLookup[args.ViewModelType]) as LagoVistaContentPage;
            var viewModel = SLWIOC.CreateForType(args.ViewModelType) as XPlatViewModel;
            ViewModelBackStack.Push(viewModel);

            viewModel.LaunchArgs = args;
            view.ViewModel = viewModel;
            return _navigation.PushAsync(view);

        }

        public Task NavigateAsync<TViewModel>(ViewModelBase parentVM, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var launchArgs = new ViewModelLaunchArgs
            {
                ParentViewModel = parentVM,
                LaunchType = LaunchTypes.Other,
                ViewModelType = typeof(TViewModel)
            };

            foreach (var arg in args)
            {
                launchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            return ShowViewModelAsync(launchArgs);
        }

        public Task NavigateAsync(ViewModelLaunchArgs args)
        {
            return ShowViewModelAsync(args);
        }

        public async Task GoBackAsync()
        {
            if (ViewModelBackStack.Any())
            {
                ViewModelBackStack.Pop();
                await _navigation.PopAsync();
            }
        }

        public async Task GoBackAsync(int skipPages)
        {
            for (int idx = 0; idx < skipPages; ++idx)
            {
                _navigation.RemovePage(_navigation.NavigationStack[_navigation.NavigationStack.Count - 1]);
                ViewModelBackStack.Pop();
            }

            if (ViewModelBackStack.Any())
            {
                ViewModelBackStack.Pop();
                await _navigation.PopAsync();
            }
        }



        public void PopToRoot()
        {
            while (ViewModelBackStack.Count > 1)
            {
                ViewModelBackStack.Pop();
            }

            _navigation.PopToRootAsync();
        }

        public bool CanGoBack()
        {
            return _navigation.NavigationStack.Count > 1;
        }

        public Task NavigateAndCreateAsync<TViewModel>(ViewModelBase parentVM, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var launchArgs = new ViewModelLaunchArgs
            {
                ParentViewModel = parentVM,
                LaunchType = LaunchTypes.Create,
                ViewModelType = typeof(TViewModel)
            };

            foreach (var arg in args)
            {
                launchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            return ShowViewModelAsync(launchArgs);
        }

        public Task NavigateAndCreateAsync<TViewModel>(ViewModelBase parentVM, object parent, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var launchArgs = new ViewModelLaunchArgs
            {
                LaunchType = LaunchTypes.Create,
                ViewModelType = typeof(TViewModel),
                ParentViewModel = parentVM,
                Parent = parent
            };

            foreach (var arg in args)
            {
                launchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            return ShowViewModelAsync(launchArgs);
        }

        public Task NavigateAndEditAsync<TViewModel>(ViewModelBase parentVM, object parent, object child, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var launchArgs = new ViewModelLaunchArgs
            {
                LaunchType = LaunchTypes.Edit,
                ViewModelType = typeof(TViewModel),
                Parent = parent,
                ParentViewModel = parentVM,
                Child = child
            };


            foreach (var arg in args)
            {
                launchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            return ShowViewModelAsync(launchArgs);
        }

        public Task NavigateAndPickAsync<TViewModel>(ViewModelBase parentVM, Action<Object> selectedAction, Action cancelledAction = null, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var launchArgs = new ViewModelLaunchArgs
            {
                LaunchType = LaunchTypes.Picker,
                ViewModelType = typeof(TViewModel),
                ParentViewModel = parentVM,
                SelectedAction = selectedAction,
                CancelledAction = cancelledAction
            };

            foreach (var arg in args)
            {
                launchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            return ShowViewModelAsync(launchArgs);
        }

        public Task NavigateAndEditAsync<TViewModel>(ViewModelBase parentVM, object parent, string id, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var launchArgs = new ViewModelLaunchArgs
            {
                LaunchType = LaunchTypes.Edit,
                ViewModelType = typeof(TViewModel),
                Parent = parent,
                ParentViewModel = parentVM,
                ChildId = id
            };

            foreach (var arg in args)
            {
                launchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            return ShowViewModelAsync(launchArgs);
        }

        public Task NavigateAndEditAsync<TViewModel>(ViewModelBase parentVM, string id, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var launchArgs = new ViewModelLaunchArgs
            {
                LaunchType = LaunchTypes.Edit,
                ViewModelType = typeof(TViewModel),
                ChildId = id,
                ParentViewModel = parentVM
            };

            foreach (var arg in args)
            {
                launchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            return ShowViewModelAsync(launchArgs);
        }

        public Task NavigateAndViewAsync<TViewModel>(ViewModelBase parentVM, object parent, object child, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var launchArgs = new ViewModelLaunchArgs
            {
                LaunchType = LaunchTypes.View,
                ViewModelType = typeof(TViewModel),
                Parent = parent,
                ParentViewModel = parentVM,
                Child = child
            };

            foreach (var arg in args)
            {
                launchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            return ShowViewModelAsync(launchArgs);
        }

        public Task NavigateAndViewAsync<TViewModel>(ViewModelBase parentVM, object parent, string id, params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var launchArgs = new ViewModelLaunchArgs
            {
                LaunchType = LaunchTypes.View,
                ViewModelType = typeof(TViewModel),
                Parent = parent,
                ParentViewModel = parentVM,
                ChildId = id
            };

            foreach (var arg in args)
            {
                launchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            return ShowViewModelAsync(launchArgs);
        }

        public Task NavigateAsync(ViewModelBase parentVM, Type viewModelType, params KeyValuePair<string, object>[] args)
        {
            var launchArgs = new ViewModelLaunchArgs
            {
                LaunchType = LaunchTypes.View,
                ViewModelType = viewModelType,
                ParentViewModel = parentVM
            };

            foreach (var arg in args)
            {
                launchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            return ShowViewModelAsync(launchArgs);
        }

        public Task SetAsNewRootAsync<TViewModel>(params KeyValuePair<string, object>[] args) where TViewModel : ViewModelBase
        {
            var viewModel = SLWIOC.CreateForType<TViewModel>() as ViewModelBase;
            ViewModelBackStack.Clear();
            ViewModelBackStack.Push(viewModel);
            viewModel.LaunchArgs = new ViewModelLaunchArgs()
            {
                IsNewRoot = true,
                LaunchType = LaunchTypes.Other
            };

            foreach (var arg in args)
            {
                viewModel.LaunchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            var viewModelType = typeof(TViewModel);
            if (!_viewModelLookup.ContainsKey(viewModelType))
            {
                Debug.WriteLine(String.Empty);
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine($"Could not find matching view for: {viewModelType.FullName}.");
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine(String.Empty);
                throw new Exception($"Could not find matching view for: {viewModelType.FullName}.");
            }

            var viewType = _viewModelLookup[viewModelType];
            Debug.Write($"Creating View {viewType.FullName} for {viewModelType.FullName}.");

            try
            {

                var view = Activator.CreateInstance(viewType) as LagoVistaContentPage;
                view.ViewModel = viewModel as XPlatViewModel;
                _navigation = view.Navigation;
                _app.MainPage = new LagoVistaNavigationPage(view);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Empty);
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine($"Error creating instance of {viewType.FullName}.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine(String.Empty);

                throw;
            }

            return Task.FromResult(default(object));
        }

        public Task SetAsNewRootAsync(Type viewModelType, params KeyValuePair<string, object>[] args)
        {
            var viewModel = SLWIOC.CreateForType(viewModelType) as ViewModelBase;
            viewModel.LaunchArgs = new ViewModelLaunchArgs()
            {
                IsNewRoot = true,
                LaunchType = LaunchTypes.Other,
            };

            foreach (var arg in args)
            {
                viewModel.LaunchArgs.Parameters.Add(arg.Key, arg.Value);
            }

            ViewModelBackStack.Clear();
            ViewModelBackStack.Push(viewModel);

            if (!_viewModelLookup.ContainsKey(viewModelType))
            {
                Debug.WriteLine(String.Empty);
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine($"Could not find matching view for: {viewModelType.FullName}.");
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine(String.Empty);
                throw new Exception($"Could not find matching view for: {viewModelType.FullName}.");
            }

            var viewType = _viewModelLookup[viewModelType];

            Debug.Write($"Creating View {viewType.FullName} for {viewModelType.FullName}.");

            try
            {
                var view = Activator.CreateInstance(viewType) as LagoVistaContentPage;
                view.ViewModel = viewModel as XPlatViewModel;
                _navigation = view.Navigation;
                _app.MainPage = new LagoVistaNavigationPage(view);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Empty);
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine($"Error creating instance of {viewType.FullName}.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine(String.Empty);

            }

            return Task.FromResult(default(object));

        }
    }
}