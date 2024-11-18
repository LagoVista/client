using LagoVista.Client.Core.Interfaces;
using LagoVista.Client.Core.Models;
using LagoVista.Client.Core.Resources;
using LagoVista.Core.Attributes;
using LagoVista.Core.Commanding;
using LagoVista.Core.IOC;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.ViewModels;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess
{
    public class DeviceViewModel : MonitoringViewModelBase
    {
        Device _device;
        
        private string _btDeviceId;
        private BTDevice _currentDevice;
        private readonly IBluetoothSerial _btSerial;

        protected string DeviceId { get; set; }
        protected string DeviceRepoId { get; set; }

        List<InputCommandEndPoint> _inputCommandEndPoints;

        public DeviceViewModel()
        {
            _btSerial = SLWIOC.Get<IBluetoothSerial>();
            _btSerial.DeviceConnected += BtSerial_DeviceConnected;
            _btSerial.DeviceDiscovered += BtSerial_DeviceDiscovered;
            _btSerial.DeviceDisconnected += BtSerial_DeviceDisconnected;
            _btSerial.ReceivedLine += BtSerial_ReceivedLine;

            EditDeviceCommand = new RelayCommand(EditDevice);
            DeviceMessages = new ObservableCollection<DeviceArchive>();
            SendCommand = new RelayCommand(Send);
            CancelSendCommand = new RelayCommand(CancelSend);

            MenuOptions = new List<MenuItem>()
            {
                new MenuItem() { Command = new RelayCommand(() => ShowView<PairBTDeviceViewModel>()), FontIconKey = "fa-bluetooth", Name = ClientResources.DeviceMore_PairDevice, Help="Associate this application using  BlueTooth" },
                new MenuItem() { Command = new RelayCommand(() => ShowView<DeviceSerialPortAccessViewModel>()), FontIconKey = "fa-wrench", Name = ClientResources.DeviceSerialPort_View, Help="Hard-wired serial port" },
                new MenuItem() { Command = new RelayCommand(() => ShowView<ConsoleViewModel>()), FontIconKey = "fa-terminal", Name = ClientResources.DeviceMore_Console, Help="Communicate using terminal" },
                new MenuItem() { Command = new RelayCommand(() => ShowView<IOConfigViewModel>()), FontIconKey = "fa-bolt", Name = ClientResources.DeviceMore_IOConfig, Help="Configuration the IO Ports" },
                new MenuItem() { Command = new RelayCommand(() => ShowView<ProvisionDeviceViewModel>()), FontIconKey = "fa-wrench", Name = ClientResources.DeviceMore_Provision, Help="Configure primary device settings" },
                new MenuItem() { Command = new RelayCommand(() => ShowView<DFUViewModel>()),FontIconKey = "fa-microchip", Name = ClientResources.DeviceMore_FirmwareUpdate,Help= "Update firmware" },
                new MenuItem() { Command = new RelayCommand(() => ShowView<LiveDataViewModel>()),FontIconKey = "fa-table", Name = ClientResources.DeviceMore_LiveData, Help="View live data" },
            };
        }

        private async void BtSerial_DeviceDiscovered(object sender, BTDevice e)
        {
            try
            {
                if (e.DeviceId == _btDeviceId && !_btSerial.IsConnected)
                {
                    await _btSerial.ConnectAsync(e);
                    _currentDevice = e;
                    ConnectionStatus = "Connected";
                    ConnectedViaBluetooth = true;
                }
            }
            catch (Exception ex)
            {
                await Popups.ShowAsync("Could not connect to device via Bluetooth: " + ex.Message);
            }
        }

        private void BtSerial_DeviceDisconnected(object sender, BTDevice e)
        {
            ConnectedViaBluetooth = false;
        }

        private void BtSerial_DeviceConnected(object sender, BTDevice e)
        {
        }

        private void BtSerial_ReceivedLine(object sender, string e)
        {

        }

        public async override Task InitAsync()
        {
            await base.InitAsync();

            /*await PerformNetworkOperation(async () =>
            {
                var path = $"/api/device/{DeviceRepoId}/{DeviceId}/metadata";


                var response = await RestClient.GetAsync<DetailResponse<Device>>(path);
                if (response.Successful)
                {
                    Device = response.Result.Model;

                    var form = new EditFormAdapter(response.Result.Model, response.Result.View, ViewModelNavigation);
                    ShowProperties(form, response.Result.Model);
                    FormAdapter = form;
                    ViewReady = true;
                    TryConnectBluetooth();
                }

                return response.ToInvokeResult();
            });*/
        }

        public override string GetChannelURI()
        {
            return $"/api/wsuri/device/{DeviceId}/normal";
        }

        public override void HandleMessage(Notification notification)
        {
            if (!String.IsNullOrEmpty(notification.PayloadType))
            {
                switch (notification.PayloadType)
                {
                    case nameof(DeviceArchive):
                        var archive = JsonConvert.DeserializeObject<DeviceArchive>(notification.Payload);
                        DispatcherServices.Invoke(() =>
                        {
                            DeviceMessages.Insert(0, archive);
                            if (DeviceMessages.Count == 50)
                            {
                                DeviceMessages.RemoveAt(2);
                            }
                        });
                        break;
                    case nameof(Device):
                        DispatcherServices.Invoke(() =>
                        {
                            var device = JsonConvert.DeserializeObject<Device>(notification.Payload);
                            if (Device != null)
                            {
                                device.DeviceIdLabel = Device.DeviceIdLabel;
                                device.DeviceNameLabel = Device.DeviceNameLabel;
                                device.DeviceTypeLabel = Device.DeviceTypeLabel;
                            }
                            Device = device;
                        });

                        break;
                }
                Debug.WriteLine("----");
                Debug.WriteLine(notification.PayloadType);
                Debug.WriteLine(notification.Payload);
                Debug.WriteLine("BYTES: " + notification.Payload.Length);
                Debug.WriteLine("----");
            }
            else
            {
                Debug.WriteLine(notification.Text);
            }
        }

        public async void Send()
        {
            await PerformNetworkOperation(async () =>
            {
                var queryString = String.Empty;
                foreach (var param in InputCommandParameters)
                {
                    if (!String.IsNullOrEmpty(queryString))
                    {
                        queryString += "&";
                    }

                    queryString += param.ToQueryStringPair();
                }

                var client = new HttpClient();
                var endPoint = _inputCommandEndPoints.Where(inp => inp.InputCommand.Key == SelectedInputCommand.Key).FirstOrDefault();
                if (endPoint != null)
                {
                    var uri = String.IsNullOrEmpty(queryString) ? endPoint.EndPoint : $"{endPoint.EndPoint}?{queryString}";
                    var result = await client.GetAsync(uri);
                    if (!result.IsSuccessStatusCode)
                    {
                        await Popups.ShowAsync(result.ReasonPhrase + $" ({result.StatusCode})");
                    }
                }
            });

            SelectedInputCommand = InputCommands.FirstOrDefault();
            DeviceStatusVisible = true;
            InputCommandVisible = false;
        }

        public bool _viewReady = false;
        public bool ViewReady
        {
            get { return _viewReady; }
            set { Set(ref _viewReady, value); }
        }

        private ObservableCollection<DeviceArchive> _deviceMessasges;
        public ObservableCollection<DeviceArchive> DeviceMessages
        {
            get { return _deviceMessasges; }
            set { Set(ref _deviceMessasges, value); }
        }

        ObservableCollection<KeyValuePair<string, object>> _stateMachines;
        public ObservableCollection<KeyValuePair<string, object>> StateMachines
        {
            get { return _stateMachines; }
            set { Set(ref _stateMachines, value); }
        }

        ObservableCollection<KeyValuePair<string, object>> _deviceAttributes;
        public ObservableCollection<KeyValuePair<string, object>> DeviceAttributes
        {
            get { return _deviceAttributes; }
            set { Set(ref _deviceAttributes, value); }
        }

        public void CancelSend()
        {
            SelectedInputCommand = InputCommands.FirstOrDefault();
            DeviceStatusVisible = true;
            InputCommandVisible = false;
        }


        private void ShowInputCommand(InputCommand inputCommand)
        {
            InputCommandVisible = true;
            DeviceStatusVisible = false;

            InputCommandParameters = new ObservableCollection<Models.InputCommandParameter>();
            foreach (var param in inputCommand.Parameters)
            {
                InputCommandParameters.Add(new Models.InputCommandParameter(param));
            }
        }

        public void EditDevice()
        {
            var launchArgs = new ViewModelLaunchArgs()
            {
                ViewModelType = typeof(EditDeviceViewModel),
                LaunchType = LaunchTypes.Edit
            };

            launchArgs.Parameters.Add(EditDeviceViewModel.DeviceRepoId, DeviceRepoId);
            launchArgs.Parameters.Add(EditDeviceViewModel.DeviceId, DeviceId);

            ViewModelNavigation.NavigateAsync(launchArgs);
        }


        private void ShowProperties(EditFormAdapter formAdapter, Device _device)
        {
            foreach (var field in _device.PropertiesMetaData)
            {
                var formField = FormField.Create(field.Key, new FormFieldAttribute(), null);

                formField.Label = field.Label;

                switch (field.FieldType.Value)
                {
                    case ParameterTypes.State:
                        formField.Options = new List<EnumDescription>();
                        foreach (var state in field.StateSet.Value.States)
                        {
                            formField.Options.Add(new EnumDescription() { Key = state.Key, Label = state.Name, Name = state.Name });
                        }

                        formField.FieldType = FormField.FieldType_Picker;

                        var initialState = field.StateSet.Value.States.Where(st => st.IsInitialState).FirstOrDefault();
                        if (initialState != null)
                        {
                            formField.Value = initialState.Key;
                        }

                        break;
                    case ParameterTypes.String:
                        formField.FieldType = FormField.FieldType_Text;
                        formField.Value = field.DefaultValue;
                        break;
                    case ParameterTypes.DateTime:
                        formField.FieldType = FormField.FieldType_DateTime;
                        formField.Value = field.DefaultValue;
                        break;
                    case ParameterTypes.Integer:
                        formField.FieldType = FormField.FieldType_Integer;
                        formField.Value = field.DefaultValue;
                        break;
                    case ParameterTypes.Decimal:
                        formField.FieldType = FormField.FieldType_Decimal;
                        formField.Value = field.DefaultValue;
                        break;
                    case ParameterTypes.GeoLocation:
                        formField.FieldType = FormField.FieldType_Text;
                        formField.Value = field.DefaultValue;
                        break;
                    case ParameterTypes.TrueFalse:
                        formField.FieldType = FormField.FieldType_CheckBox;
                        formField.Value = field.DefaultValue;
                        break;
                    case ParameterTypes.ValueWithUnit:
                        formField.FieldType = FormField.FieldType_Decimal;
                        formField.Value = field.DefaultValue;
                        break;
                }

                formField.IsRequired = field.IsRequired;
                formField.IsUserEditable = !field.IsReadOnly;

                var fieldValue = _device.Properties.Where(prop => prop.Key == field.Key).FirstOrDefault();
                if (fieldValue != null)
                {
                    formField.Value = fieldValue.Value;
                }
                else
                {
                    formField.Value = formField.DefaultValue;
                }

                formAdapter.FormItems.Add(formField);
            }
        }

        ObservableCollection<Models.InputCommandParameter> _inputCommandParameters;
        public ObservableCollection<Models.InputCommandParameter> InputCommandParameters
        {
            get { return _inputCommandParameters; }
            set { Set(ref _inputCommandParameters, value); }
        }

        public Device Device
        {
            get { return _device; }
            set
            {
                var attrs = new ObservableCollection<KeyValuePair<string, object>>();
                var stateMachines = new ObservableCollection<KeyValuePair<string, object>>();
                var inputCommands = new ObservableCollection<InputCommand>();

                if (value != null)
                {
                    foreach (var attr in value.Attributes)
                    {
                        attrs.Add(new KeyValuePair<string, object>(attr.Name, attr.Value));
                    }

                    foreach (var stm in value.States)
                    {
                        var state = stm.StateSet.Value.States.Where(stat => stat.Key == stm.Value).FirstOrDefault();
                        if (state != null)
                        {
                            stateMachines.Add(new KeyValuePair<string, object>(stm.Name, state.Name));
                        }
                        else
                        {
                            stateMachines.Add(new KeyValuePair<string, object>(stm.Name, stm.Value));
                        }
                    }

                    /* We only get these when the device is first loaded, not when the device
                     * is updated via web socket */
                    if (value.InputCommandEndPoints != null)
                    {
                        _inputCommandEndPoints = value.InputCommandEndPoints;
                        foreach (var cmd in _inputCommandEndPoints)
                        {
                            inputCommands.Add(cmd.InputCommand);
                        }

                        HasInputCommands = inputCommands.Count > 0;
                        if (HasInputCommands)
                        {
                            inputCommands.Insert(0, new InputCommand()
                            {
                                Key = "N/A",
                                Name = ClientResources.MonitorDevice_SelectInputCommand
                            });
                            InputCommands = inputCommands;
                            SelectedInputCommand = inputCommands[0];
                        }
                        else
                        {
                            SelectedInputCommand = null;
                        }
                    }
                }

                StateMachines = stateMachines;
                HasStateMachines = StateMachines.Any();
                HasAttributes = attrs.Any();
                DeviceAttributes = attrs;
                Set(ref _device, value);
            }
        }

        public List<MenuItem> MenuOptions { get; }

        public RelayCommand SendCommand { get; }
        public RelayCommand CancelSendCommand { get; }
        public RelayCommand EditDeviceCommand { get; }


        public ObservableCollection<InputCommand> _inputCommands;
        public ObservableCollection<InputCommand> InputCommands
        {
            get { return _inputCommands; }
            set { Set(ref _inputCommands, value); }
        }

        InputCommand _selectedInputCommand;
        public InputCommand SelectedInputCommand
        {
            get { return _selectedInputCommand; }
            set
            {
                if (value != null)
                {
                    if (value != InputCommands.FirstOrDefault())
                    {
                        ShowInputCommand(value);
                    }
                }

                Set(ref _selectedInputCommand, value);
            }
        }

        EditFormAdapter _editFormAdapter;
        public EditFormAdapter FormAdapter
        {
            get => _editFormAdapter;
            set => Set(ref _editFormAdapter, value);
        }

        private bool _hasInputCommands = false;
        public bool HasInputCommands
        {
            get { return _hasInputCommands; }
            set { Set(ref _hasInputCommands, value); }
        }

        private bool _hasStateMachines = false;
        public bool HasStateMachines
        {
            get { return _hasStateMachines; }
            set { Set(ref _hasStateMachines, value); }
        }

        private bool _hasAttributes = false;
        public bool HasAttributes
        {
            get { return _hasAttributes; }
            set { Set(ref _hasAttributes, value); }
        }

        private bool _inputCommandVisible = false;
        public bool InputCommandVisible
        {
            get { return _inputCommandVisible; }
            set { Set(ref _inputCommandVisible, value); }
        }

        private bool _deviceStatusVisible = true;
        public bool DeviceStatusVisible
        {
            get { return _deviceStatusVisible; }
            set { Set(ref _deviceStatusVisible, value); }
        }

        private bool _connectedViaBluetooth = false;
        public bool ConnectedViaBluetooth
        {
            get { return _connectedViaBluetooth; }
            set { Set(ref _connectedViaBluetooth, value); }
        }

        private string _connectionStatus;
        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set { Set(ref _connectionStatus, value); }
        }

        public async override Task IsClosingAsync()
        {
            if (_btSerial.IsConnected)
            {
                //DisconnectBTDevice();
            }

            await base.IsClosingAsync();

        }
    }
}
