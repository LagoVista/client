using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.Net
{
    public class HeadlessAuthManager : IAuthManager
    {
        private const string AUTH_MGR_SETTINGS = "AUTHSETTINGS.JSON";
        readonly IStorageService _storage;
        readonly IDeviceInfo _deviceInfo;

        public HeadlessAuthManager(IStorageService storage, IDeviceInfo deviceInfo)
        {
            _deviceInfo = deviceInfo;
            _storage = storage;
        }

        public string AccessToken { get; set; }
        public string AccessTokenExpirationUTC { get; set; }
        public string DeviceId { get; set; }
        public String DeviceType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenExpirationUTC { get; set; }
        public UserInfo User { get; set; }
        public List<EntityHeader> Roles { get; set; }
        public string AppInstanceId { get; set; }

        public event EventHandler<EntityHeader> OrgChanged;
        public event EventHandler<List<EntityHeader>> RolesChanged;

        public async Task LoadAsync()
        {
            //TODO: Might move to automapper if we have more stuff like this.
            var storedAuthmanager = await _storage.GetAsync<AuthManager>(AUTH_MGR_SETTINGS);
            if (storedAuthmanager == null)
            {
                DeviceId = _deviceInfo.DeviceUniqueId;
                DeviceType = _deviceInfo.DeviceType;
                IsAuthenticated = false;
                AccessToken = null;
                RefreshToken = null;
                User = null;
                AccessTokenExpirationUTC = null;
                RefreshTokenExpirationUTC = null;
                IsAuthenticated = false;
                Roles = new List<EntityHeader>();
            }
            else
            {
                AccessToken = storedAuthmanager.AccessToken;
                AppInstanceId = storedAuthmanager.AppInstanceId;
                AccessTokenExpirationUTC = storedAuthmanager.AccessTokenExpirationUTC;
                DeviceId = storedAuthmanager.DeviceId;
                DeviceType = storedAuthmanager.DeviceType;
                IsAuthenticated = storedAuthmanager.IsAuthenticated;
                RefreshToken = storedAuthmanager.RefreshToken;
                RefreshTokenExpirationUTC = storedAuthmanager.RefreshTokenExpirationUTC;              
                User = storedAuthmanager.User;
                Roles = storedAuthmanager.Roles;
            }
        }

        public async Task LogoutAsync()
        {
            AccessToken = null;
            AccessTokenExpirationUTC = null;
            IsAuthenticated = false;
            RefreshToken = null;
            RefreshTokenExpirationUTC = null;
            User = null;
            Roles = new List<EntityHeader>();
        }

        public Task PersistAsync()
        {
            return _storage.StoreAsync<HeadlessAuthManager>(this, AUTH_MGR_SETTINGS);
        }

        public void RaiseOrgChanged(EntityHeader newOrg)
        {
            OrgChanged?.Invoke(this, newOrg);
        }

        public void RaiseRolesChanged(List<EntityHeader> newRoles)
        {
            RolesChanged?.Invoke(this, newRoles);
        }
    }
}
