using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.Net
{
    public interface IWebSocket : IDisposable
    {
        event EventHandler<string> MessageReceived;
        event EventHandler Closed;
        Task<InvokeResult> OpenAsync(Uri uri);
        Task<InvokeResult> CloseAsync();
    }
}
