using LagoVista.Client.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.Interfaces
{
    public interface IBluetoothSerial
    {
        event EventHandler<string> ReceivedLine;


        event EventHandler<DFUProgress> DFUProgress;

        event EventHandler DFUCompleted;
        event EventHandler<string> DFUFailed;

        event EventHandler<BTDevice> DeviceConnected;
        event EventHandler<BTDevice> DeviceConnecting;
        event EventHandler<BTDevice> DeviceDisconnected;
        event EventHandler<BTDevice> DeviceDiscovered;

        Task<ObservableCollection<BTDevice>> SearchAsync();
        Task StopSearchingAsync();

        Task ConnectAsync(BTDevice device);

        Task DisconnectAsync();

        Task SendDFUAsync(byte[] firmware);

        Task SendAsync(String msg);

        BTDevice CurrentDevice { get; }

        bool IsConnected { get; }
    }
}
