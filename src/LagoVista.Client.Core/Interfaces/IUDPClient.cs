using System;
using System.Threading.Tasks;

namespace LagoVista.Client.Core
{
    public interface IUDPClient : IDisposable
    {
        Task ConnectAsync(string host, int port);

        Task<byte[]> ReceiveAsync();

        Task<int> WriteAsync(byte[] buffer, int start, int length);

        Task<int> WriteAsync(string output);

        Task<int> WriteAsync<T>(T obj) where T : class;

        Task DisconnectAsync();
    }
}
