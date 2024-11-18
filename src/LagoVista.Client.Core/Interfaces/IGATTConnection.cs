using LagoVista.Client.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.Interfaces
{
    public interface IGATTConnection
    {
        event EventHandler<BLEDevice> DeviceDiscovered;
        event EventHandler<BLEDevice> DeviceConnected;
        event EventHandler<BLEDevice> DeviceDisconnected;
        event EventHandler<BLECharacteristicsValue> CharacteristicChanged;

        event EventHandler<Models.DFUProgress> DFUProgress;
        event EventHandler<string> DFUFailed;        
        event EventHandler DFUCompleted;

        event EventHandler<string> ReceiveConsoleOut;
        bool IsScanning { get; }
        Task StartScanAsync();

        Task StopScanAsync();

        Task ConnectAsync(BLEDevice device);
        Task DisconnectAsync(BLEDevice device);

        ObservableCollection<BLEDevice> DiscoveredDevices { get; }
        ObservableCollection<BLEDevice> ConnectedDevices { get; }

        void RegisterKnownServices(IEnumerable<BLEService> services);
        Task<bool> SubscribeAsync(BLEDevice device, BLEService service, BLECharacteristic characteristic);
        Task<bool> UnsubscribeAsync(BLEDevice device, BLEService service, BLECharacteristic characteristic);
        Task<bool> WriteCharacteristic(BLEDevice device, BLEService service, BLECharacteristic characteristic, string str);
        Task<bool> UpdateCharacteristic(BLEDevice device, BLEService service, BLECharacteristic characteristic);
        Task<byte[]> ReadCharacteristicAsync(BLEDevice device, BLEService service, BLECharacteristic characteristic);
        Task<bool> WriteCharacteristic(BLEDevice device, BLEService service, BLECharacteristic characteristic, byte[] str);
    }
}
