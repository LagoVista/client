using LagoVista.Core.PlatformSupport;
using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using LagoVista.Core.Models;

namespace LagoVista.Core.WPF.PlatformSupport
{
    public class NetworkService : INetworkService
    {
        public ObservableCollection<NetworkDetails> AllConnections
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsInternetConnected
        {
            set
            {
                if (value)
                    NetworkInformationChanged(this, null);
            }
            get
            {
                
                return true;
            }

        }

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
            throw new NotImplementedException();
        }
    }

 
}
