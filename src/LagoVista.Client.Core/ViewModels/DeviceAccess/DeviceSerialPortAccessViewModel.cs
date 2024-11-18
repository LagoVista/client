using LagoVista.Core.Commanding;
using LagoVista.Core.Models;
using LagoVista.Core.PlatformSupport;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess
{
    public class DeviceSerialPortAccessViewModel : AppViewModelBase
    {
        ISerialPort _currentPort;
        private CancellationTokenSource _listenCancelTokenSource;

        public DeviceSerialPortAccessViewModel()
        {
            Lines = new ObservableCollection<string>();
            ClosePortCommand = new RelayCommand(() => ClosePort());
            OpenPortCommand = new RelayCommand(() => OpenPort());
            DoneCommand = new RelayCommand(() => Done());
            SendCommand = new RelayCommand(() => Send(), () => CanSend());
        }

        async void ReadFromSerialPort(SerialPortInfo portInfo)
        {
            portInfo.BaudRate = 115200;
            portInfo.Parity = false;
            portInfo.DataBits = 8;

            try
            {
                _currentPort = DeviceManager.CreateSerialPort(portInfo);
                _listenCancelTokenSource = new CancellationTokenSource();
                await _currentPort.OpenAsync();
                Lines.Clear();
                IsPortOpen = true;
                IsPortClosed = false;

                var _running = true;
                var buffer = new byte[1024];
                while (_running)
                {
                    try
                    {
                        var bytesRead = await _currentPort.ReadAsync(buffer, 0, buffer.Length, _listenCancelTokenSource.Token);
                        if (bytesRead > 0)
                        {
                            var text = ASCIIEncoding.ASCII.GetString(buffer, 0, bytesRead);
                            var lines = text.Split('\n');
                            foreach (var line in lines)
                            {
                                if (!String.IsNullOrEmpty(line.Trim()))
                                {
                                    Lines.Add("<<<< " + DateTime.Now + ": " + line.Trim());
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        _running = false;
                        Lines.Add("DISCONNECTED");
                        Lines.Add("");
                        _listenCancelTokenSource = null;
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                await Popups.ShowAsync(ex.Message);
            }
        }

        private bool CanSend()
        {
            return !String.IsNullOrEmpty(SendText) && _currentPort != null;
        }

        public async void Send()
        {
            if (!String.IsNullOrEmpty(SendText))
            {
                await _currentPort.WriteAsync(SendText);

                Lines.Add($">>>> {DateTime.Now} {SendText}");
                if(AppendCR)
                    await _currentPort.WriteAsync("\r");

                if (AppendLF)
                    await _currentPort.WriteAsync("\n");
            }
        }


        public void OpenPort()
        {
            Lines.Add("RECONNECTING");
            ReadFromSerialPort(SelectedPort);
        }

        public void Done()
        {
            if (IsPortOpen)
            {
                ClosePort();
                SelectedPort = null;
            }
        }

        public void ClosePort()
        {
            if (_selectedPort != null)
            {
                if (_listenCancelTokenSource != null)
                    _listenCancelTokenSource.Cancel();

                _currentPort.CloseAsync();
                IsPortOpen = false;
                IsPortClosed = true;
                SendCommand.RaiseCanExecuteChanged();
            }
        }


        public async override Task InitAsync()
        {
            SerialPorts = await DeviceManager.GetSerialPortsAsync();

            await base.InitAsync();
        }

        ObservableCollection<SerialPortInfo> _serialPorts;

        public ObservableCollection<SerialPortInfo> SerialPorts
        {
            get { return _serialPorts; }
            set { Set(ref _serialPorts, value); }
        }

        SerialPortInfo _selectedPort;
        public SerialPortInfo SelectedPort
        {
            get { return _selectedPort; }
            set
            {
                Set(ref _selectedPort, value);
                if (value != null)
                {
                    ReadFromSerialPort(value);
                }
            }
        }


        public ObservableCollection<string> Lines
        {
            get;
        }

        public async override Task IsClosingAsync()
        {
            if (_currentPort != null)
            {
                if (_listenCancelTokenSource != null)
                    _listenCancelTokenSource.Cancel();

                await _currentPort.CloseAsync();
            }
            await base.IsClosingAsync();
        }

        private bool _isPortOpen = false;
        public bool IsPortOpen
        {
            get { return _isPortOpen; }
            set { Set(ref _isPortOpen, value); }
        }

        private bool _isPortClosed = false;
        public bool IsPortClosed
        {
            get { return _isPortClosed; }
            set { Set(ref _isPortClosed, value); }
        }

        private bool _appendCR;
        public bool AppendCR
        {
            get { return _appendCR; }
            set { Set(ref _appendCR, value); }
        }

        private bool _appendLF;
        public bool AppendLF
        {
            get { return _appendLF; }
            set { Set(ref _appendLF, value); }
        }

        private string _sendText;
        public String SendText
        {
            get { return _sendText; }
            set
            {
                Set(ref _sendText, value);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand ClosePortCommand { get; }
        public ICommand OpenPortCommand { get; }
        public ICommand DoneCommand { get; }
        public RelayCommand SendCommand { get; }

    }
}
