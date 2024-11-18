using LagoVista.Client.Core.Models;
using LagoVista.Client.Core.Net;
using LagoVista.Core.IOC;
using LagoVista.Core.Validation;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels
{
    public abstract class MonitoringViewModelBase : AppViewModelBase
    {
        Uri _wsUri;
        IWebSocket _webSocket;

        public async override Task InitAsync()
        {
            await base.InitAsync();

            if (IsNetworkConnected)
            {
                await SubscribeToWebSocketAsync();
            }
        }

        public async Task SubscribeToWebSocketAsync()
        {
            var callResult = await PerformNetworkOperation(async () =>
            {
                if (_webSocket != null)
                {
                    await _webSocket.CloseAsync();
                    Debug.WriteLine("Web Socket is Closed.");
                    _webSocket = null;
                }

                var channelId = GetChannelURI();
                var wsResult = await RestClient.GetAsync<InvokeResult<string>>(channelId);
                if (wsResult.Successful)
                {
                    var url = wsResult.Result.Result;
                    Debug.WriteLine(url);
                    _wsUri = new Uri(url);

                    _webSocket = SLWIOC.Create<IWebSocket>();
                    _webSocket.MessageReceived += _webSocket_MessageReceived;
                    var wsOpenResult = await _webSocket.OpenAsync(_wsUri);
                    if (wsOpenResult.Successful)
                    {
                        Debug.WriteLine("OPENED CHANNEL");
                    }
                    return wsOpenResult;
                }
                else
                {
                    return wsResult.ToInvokeResult();
                }
            });
        }

        public async override Task IsClosingAsync()
        {
            if (_webSocket != null)
            {
                await _webSocket.CloseAsync();
                Debug.WriteLine("Web Socket is Closed.");
                _webSocket = null;
            }
        }

        private void _webSocket_MessageReceived(object sender, string json)
        {
            var notification = JsonConvert.DeserializeObject<Notification>(json);
            HandleMessage(notification);
        }

        public abstract void HandleMessage(Notification notification);

        public abstract string GetChannelURI();
    }
}