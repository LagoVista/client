using LagoVista.Core.Models;
using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.Net
{
    public class DefaultNetworkService : INetworkService
    {
        public bool IsInternetConnected => true;

        public ObservableCollection<NetworkDetails> AllConnections => throw new NotImplementedException();

        public event EventHandler NetworkInformationChanged;

        public string GetIPV4Address()
        {
            throw new NotImplementedException();
        }

        public void OpenURI(Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task RefreshAysnc()
        {
            throw new NotImplementedException();
        }

        public Task<bool> TestConnectivityAsync()
        {
            return Task.FromResult(true);
        }
    }
}
