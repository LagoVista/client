using LagoVista.Client.Core.Auth;
using LagoVista.Client.Core.ViewModels.Auth;
using LagoVista.Client.Core.ViewModels.Orgs;
using LagoVista.Core.Commanding;
using LagoVista.Core.IOC;
using LagoVista.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels
{
    public enum RightMenuIcon
    {
        None,
        Add,
        Edit,
        Save,
        Delete,
        Next,
        Settings,
        CustomIcon,
        CustomText
    }

    public enum LeftMenuIcon
    {
        None,
        Menu,
        Back,
        Cancel,
        CustomIcon,
        CustomText
    }

    public abstract class XPlatViewModel : ViewModelBase
    {
        private readonly static Dictionary<string, object> _localCache = new Dictionary<string, object>();

        public XPlatViewModel()
        {
            SaveCommand = new RelayCommand(Save, CanSave);
            EditCommand = new RelayCommand(Edit);
            HelpCommand = new RelayCommand(ShowHelp);

            HasHelp = !String.IsNullOrEmpty(GetHelpLink());
        }

        RightMenuIcon _rightMenuIcon;
        public RightMenuIcon RightMenuIcon
        {
            get { return _rightMenuIcon; }
            set { Set(ref _rightMenuIcon, value); }
        }

        LeftMenuIcon _leftMenuIcon;
        public LeftMenuIcon LeftMenuIcon
        {
            get { return _leftMenuIcon; }
            set { Set(ref _leftMenuIcon, value); }
        }

        bool _menuVisible;
        public bool MenuVisible
        {
            get { return _menuVisible; }
            set { Set(ref _menuVisible, value); }
        }

        IEnumerable<MenuItem> _menuItems;
        public IEnumerable<MenuItem> MenuItems
        {
            get { return _menuItems; }
            set { Set(ref _menuItems, value); }
        }

        private bool _hasHelp = false;
        public bool HasHelp
        {
            get { return _hasHelp; }
            set { Set(ref _hasHelp, value); }
        }

        public virtual void HandleURIActivation(Uri uri, Dictionary<string, string> kvps)
        {
            Logger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Message, "HandleURIActivation", uri.Host, uri.Query.ToKVP("queryString"));

            if (uri.Host == "resetpassword")
            {
                if (!kvps.ContainsKey("code"))
                {
                    Logger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Error, "ResetPassword_HandleURIActivation", "Missing Code", uri.Query.ToKVP("queryString"), uri.Host.ToKVP("action"));
                }
                else
                {
                    var launchArgs = new ViewModelLaunchArgs
                    {
                        ViewModelType = typeof(ResetPasswordViewModel)
                    };
                    launchArgs.Parameters.Add("code", kvps["code"]);
                    ViewModelNavigation.NavigateAsync(launchArgs);
                }
            }
            else if (uri.Host == "acceptinvite")
            {
                if (!kvps.ContainsKey("inviteid"))
                {
                    Logger.AddCustomEvent(LagoVista.Core.PlatformSupport.LogLevel.Error, "AcceptInvite_HandleURIActivation", "Missing Inviteid", uri.Query.ToKVP("queryString"), uri.Host.ToKVP("action"));
                }
                else
                {
                    var launchArgs = new ViewModelLaunchArgs
                    {
                        ViewModelType = typeof(AcceptInviteViewModel)
                    };
                    launchArgs.Parameters.Add("inviteId", kvps["inviteid"]);
                    ViewModelNavigation.NavigateAsync(launchArgs);
                }
            }
        }

        protected Task NavigateAndViewAsync<TViewModel>(string id)
        {
            var launchArgs = new ViewModelLaunchArgs()
            {
                ViewModelType = typeof(TViewModel),
                ParentViewModel = this,
                ChildId = id,
                LaunchType = LaunchTypes.View
            };

            return ViewModelNavigation.NavigateAsync(launchArgs);
        }

        protected virtual string GetHelpLink()
        {
            return String.Empty;
        }


        protected Task NavigateAndCreateAsync<TViewModel>(string parentId)
        {
            var launchArgs = new ViewModelLaunchArgs()
            {
                ViewModelType = typeof(TViewModel),
                ParentViewModel = this,
                ParentId = parentId,
                LaunchType = LaunchTypes.Create
            };

            return ViewModelNavigation.NavigateAsync(launchArgs);
        }

        protected ISecureStorage SecureStorage { get { return SLWIOC.Get<ISecureStorage>(); } }

        public virtual void Edit()
        {

        }

        public virtual void Save()
        {

        }

        protected void ShowHelp()
        {
            LagoVista.Core.PlatformSupport.Services.Network.OpenURI(new Uri(GetHelpLink()));
        }

        public virtual bool CanSave()
        {
            return true;
        }

        /// <summary>
        /// Override this on the view model to determine if the data is dirty and we can close the page.
        /// </summary>
        /// <returns></returns>
        public virtual Task<bool> CanCancelAsync()
        {
            return Task.FromResult(true);
        }

        public RelayCommand HelpCommand { get; private set; }

        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand EditCommand { get; private set; }

        protected Task AddToLocalCacheAsync<T>(String key, T item)
        {
            _localCache.Add(key, item);

            // Running this a task in case we need to serialize the state of the app.
            return Task.CompletedTask;
        }

        protected Task<T> GetFromLocalCacheAsync<T>(string key) where T : class
        {
            return Task.FromResult(_localCache[key] as T);
        }

        protected Task<bool> HasLocalCacheItemAsync<T>(string key) where T : class
        {
            if(!_localCache.ContainsKey(key))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult((_localCache[key] as T) != null);
        }

        protected Task ClearLocalCacheItemAsync(string key)
        {
            if(_localCache.ContainsKey(key))
            {
                _localCache.Remove(key);
            }

            return Task.CompletedTask;
        }

    }
}
