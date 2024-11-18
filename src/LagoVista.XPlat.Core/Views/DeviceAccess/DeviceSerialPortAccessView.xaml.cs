using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LagoVista.XPlat.Core.Views.DeviceAccess
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceSerialPortAccessView : LagoVistaContentPage
    {
        public DeviceSerialPortAccessView()
        {
            InitializeComponent();
            SerialPortOutput.PropertyChanged += SerialPortOutput_PropertyChanged;
        }

        private void SerialPortOutput_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Debug.WriteLine("PROP CHANGED" + e.PropertyName);
            if(e.PropertyName == nameof(ListView.ItemsSource))
            {
                var dataSource = (ObservableCollection<string>)SerialPortOutput.ItemsSource;

                if (dataSource != null)
                {
                    dataSource.CollectionChanged += DataSource_CollectionChanged;
                }
            }
        }

        private void DataSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var dataSource = SerialPortOutput.ItemsSource as ObservableCollection<String>;
            var lastItem = dataSource.LastOrDefault();
            if(lastItem != null)
            {
                SerialPortOutput.ScrollTo(lastItem, ScrollToPosition.MakeVisible, true);
            }
        }
    }
}