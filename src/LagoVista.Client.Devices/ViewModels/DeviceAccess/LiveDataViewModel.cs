using LagoVista.Client.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels.DeviceAccess
{
    public class LiveDataViewModel : DeviceViewModelBase
    {
        
        IOConfig _config;

        protected async override void OnBLEDevice_Disconnected(BLEDevice device)
        {
            await Popups.ShowAsync("Device Disconnected.");
        }
        /*
        protected override void OnBTSerail_MsgReceived(string e)
        {
            DispatcherServices.Invoke(() =>
            {
                var lines = e.Split('\r');
                foreach (var line in lines)
                {
                    var parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();

                        var existingItem = DataItems.FirstOrDefault(lst => lst.Key == key);
                        if (existingItem != null)
                        {
                            existingItem.Value = value;
                        }
                        else
                        {
                            var label = key;
                            if (_config != null)
                            {
                                label = _config.ResolveLabel(key);
                            }

                            DataItems.Add(new LiveDataItem(key, label, value));
                        }
                    }
                }
            });
        }*/

        public override async Task InitAsync()
        {
            _config = await Storage.GetAsync<IOConfig>($"{DeviceId}.ioconfig.json");

            await base.InitAsync();
        }

        public ObservableCollection<LiveDataItem> DataItems { get; set; } = new ObservableCollection<LiveDataItem>();
    }
}
