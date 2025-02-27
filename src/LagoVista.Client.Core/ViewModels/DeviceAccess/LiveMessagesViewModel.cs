using LagoVista.Client.Core.Models;
using LagoVista.IoT.DeviceManagement.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess
{
    public class LiveMessagesViewModel : MonitoringViewModelBase
    {        
        Device _device;

        public override async Task InitAsync()
        {
            await base.InitAsync();

            IsBusy = true;
        }

        public override string GetChannelURI()
        {
            //return $"/api/wsuri/device/{C}/normal";
            return "";
        }

        public override void HandleMessage(Notification notification)
        {
            if (!String.IsNullOrEmpty(notification.PayloadType))
            {
                switch (notification.PayloadType)
                {
                    case "DeviceArchive":
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
                    case "Device":
                        DispatcherServices.Invoke(() =>
                        {
                            Device = JsonConvert.DeserializeObject<Device>(notification.Payload);
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

        private ObservableCollection<DeviceArchive> _deviceMessasges;
        public ObservableCollection<DeviceArchive> DeviceMessages
        {
            get { return _deviceMessasges; }
            set { Set(ref _deviceMessasges, value); }
        }

        public Device Device
        {
            get { return _device; }
            set
            {
                Set(ref _device, value);
            }
        }
    }
}