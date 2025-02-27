using LagoVista.Core.Models;
using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.XPlat.Maui.Services
{
    public class NetworkService : INetworkService
    {
        public bool IsInternetConnected => true;

        public ObservableCollection<NetworkDetails> AllConnections => new ObservableCollection<NetworkDetails>();

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
