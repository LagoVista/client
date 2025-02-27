using LagoVista.Client.Core.Interfaces;
using LagoVista.Client.Core.Models;
using LagoVista.Client.Core.Net;
using LagoVista.Core;
using LagoVista.Core.Commanding;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess
{
    public class DFUViewModel : DeviceViewModelBase
    {
        private string _firmwareUrl;
        private readonly IAppServices _appServices;
        private readonly IProcessOutputWriter _processsOutputWriter;

        public DFUViewModel(IProcessOutputWriter processOutputWriter, IAppServices appServices)
        {
            _appServices = appServices;
            _processsOutputWriter = processOutputWriter;
            UpdateDeviceFirmwareCommand = new RelayCommand(UpdateDeviceFirmware);
            CancelUpdatedCommand = new RelayCommand(CancelUpdate);
            StartSerialUpdateCommand = new RelayCommand(StartSerialUpdate);
        }

        /*
        protected override void OnDFUProgress(DFUProgress progress)
        {
            DispatcherServices.Invoke(() =>
            {
                UploadProgress = (progress.Progress / 100.0f);
                StatusMessage = $"Progress {progress.Progress}% block {progress.BlockIndex} of {progress.TotalBlockCount} - checksum: {progress.CheckSum}";
            });
        }

        protected override void OnBTSerail_LineReceived(string line)
        {
            var variables = line.Split(',');
            foreach (var variable in variables)
            {
                Debug.WriteLine(variable);
                var parts = variable.Split('=');
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    if (key == "readonly-firmwareVersion")
                    {
                        DispatcherServices.Invoke(() =>
                        {
                            DeviceFirmwareVersion = value;
                        });
                    }

                    Debug.WriteLine($"{key}-{value}");
                }
            }
        }*/

        private List<string> GetFlashingArgs()
        {
            var fwPath = @"X:\ESP32\";

            var args = new List<string>
            {
                "--chip esp32",
                @"--port COM8",
                "--baud 460800",
                "--before default_reset",
                "--after hard_reset",
                "write_flash",
                "-z",
                "--flash_mode dio",
                "--flash_freq 40m",
                "--flash_size detect",
                $"0x1000 {fwPath}bootloader_dio_40m.bin",
                $"0x8000 {fwPath}partitions.bin",
                $"0xe000 {fwPath}boot_app0.bin",
                $"0x10000 {fwPath}firmware.bin"
            };

            return args;
        }

        public void StartSerialUpdate()
        {
            var path = @"c:\users\kevin\appdata\local\programs\python\python37\";
            var esptoolPath = Path.Combine(_appServices.AppInstallDirectory, "Resources");

            var args = GetFlashingArgs();
            var argString = new StringBuilder();
            foreach(var arg in args)
            {
                argString.Append($"{arg} ");
            }
           
            var scriptFile = Path.Combine(esptoolPath, $"esptool.py {argString}");
            RunProcess($"{path}python.exe", @"C:\", scriptFile, "Run Script");
        }

        public override async Task InitAsync()
        {
            await base.InitAsync();

            if (DeviceConnected)
            {
                try
                {
                    await SendAsync("HELLO\n");
                    await Task.Delay(250);
                    await SendAsync("PAUSE\n");
                    await Task.Delay(250);
                    await SendAsync("PROPERTIES\n");

                    var result = await PerformNetworkOperation(async () =>
                    {
                        var getDeviceUri = $"/api/device/{DeviceRepoId}/{DeviceId}";

                        var restClient = new FormRestClient<Device>(base.RestClient);

                        var device = await restClient.GetAsync(getDeviceUri);
                        var deviceType = await this.RestClient.GetAsync<DetailResponse<DeviceType>>($"/api/devicetype/{device.Result.Model.DeviceType.Id}");
                        if (EntityHeader.IsNullOrEmpty(deviceType.Result.Model.Firmware))
                        {
                            throw new Exception($"No firmware found for device type {deviceType.Result.Model.Name}");
                        }

                        var firmware = await this.RestClient.GetAsync<DetailResponse<Firmware>>($"/api/firmware/{deviceType.Result.Model.Firmware.Id}");
                        _firmwareUrl = $"/api/firmware/download/{firmware.Result.Model.Id}/{firmware.Result.Model.DefaultRevision.Id}";

                        HasServerFirmware = true;
                        FirmwareName = firmware.Result.Model.Name;
                        FirmwareSKU = firmware.Result.Model.FirmwareSku;
                        ServerFirmwareVersion = firmware.Result.Model.DefaultRevision.Text;
                    });

                    if (!result)
                    {
                        await Popups.ShowAsync("Could not connect to server.");
                        this.CloseScreen();
                    }
                }
                catch (Exception)
                {
                    await Popups.ShowAsync("Could not connect to device.");
                    this.CloseScreen();
                }
            }

            HasServerFirmware = true;
        }

        private void CancelUpdate()
        {

        }


        private void RunProcess(string cmd, string path, string args, string actionType, bool clearConsole = true, bool checkRemote = true)
        {
            Task.Run(() => 
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = cmd,
                        Arguments = args,
                        UseShellExecute = false,
                        WorkingDirectory = path,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                proc.OutputDataReceived += Proc_OutputDataReceived;
                proc.ErrorDataReceived += Proc_ErrorDataReceived;

                _processsOutputWriter.AddMessage(LogType.Message, $"cd {path}");
                _processsOutputWriter.AddMessage(LogType.Message, $"{proc.StartInfo.FileName} {proc.StartInfo.Arguments}");

                proc.Start();

                while (!proc.StandardOutput.EndOfStream)
                {
                    var line = proc.StandardOutput.ReadLine().Trim();
                    _processsOutputWriter.AddMessage(LogType.Message, line);
                    _processsOutputWriter.Flush();
                    Debug.WriteLine(line);
                }

                while (!proc.StandardError.EndOfStream)
                {
                    var line = proc.StandardError.ReadLine().Trim();
                    _processsOutputWriter.AddMessage(LogType.Error, line);
                    _processsOutputWriter.Flush();
                }

                if (proc.ExitCode == 0)
                {
                    _processsOutputWriter.AddMessage(LogType.Success, $"Success {actionType}");
                }
                else
                {
                    _processsOutputWriter.AddMessage(LogType.Error, $"Error {actionType}!");
                }

                _processsOutputWriter.AddMessage(LogType.Message, "------------------------------");
                _processsOutputWriter.AddMessage(LogType.Message, "");
                _processsOutputWriter.Flush();
            });
        }

        private void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _processsOutputWriter.AddMessage(LogType.Message, e.Data);
        }

        public async void UpdateDeviceFirmware()
        {
            StatusMessage = "Downloading firmare.";
            var responseMessage = await this.HttpClient.GetAsync(_firmwareUrl);
            var buffer = await responseMessage.Content.ReadAsByteArrayAsync();
            StatusMessage = "Firmware Downloaded, starting device firmware update.";
            await SendAsync("READFIRMWARE\n");
        //    SendDFU(buffer);
        }

        public RelayCommand UpdateDeviceFirmwareCommand { get; private set; }
        public RelayCommand CancelUpdatedCommand { get; private set; }

        public RelayCommand StartSerialUpdateCommand { get; private set; }

        private string _deviceFirmwareVersion;
        public string DeviceFirmwareVersion
        {
            get { return _deviceFirmwareVersion; }
            set { Set(ref _deviceFirmwareVersion, value); }
        }

        private string _serverFirmwareVersion;
        public string ServerFirmwareVersion
        {
            get { return _serverFirmwareVersion; }
            set { Set(ref _serverFirmwareVersion, value); }
        }

        private string _firmareName;
        public string FirmwareName
        {
            get { return _firmareName; }
            set { Set(ref _firmareName, value); }
        }

        private string _firmwareSKU;
        public string FirmwareSKU
        {
            get { return _firmwareSKU; }
            set { Set(ref _firmwareSKU, value); }
        }

        private float _uploadProgress;
        public float UploadProgress
        {
            get { return _uploadProgress; }
            set { Set(ref _uploadProgress, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { Set(ref _message, value); }
        }

        private bool _hasServerFirmware = false;
        public bool HasServerFirmware
        {
            get { return _hasServerFirmware; }
            set { Set(ref _hasServerFirmware, value); }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set { Set(ref _statusMessage, value); }
        }

        public ObservableCollection<ConsoleOutput> Output
        {
            get { return _processsOutputWriter.Output; }
        }
    }
}
