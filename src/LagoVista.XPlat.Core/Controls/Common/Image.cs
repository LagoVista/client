using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core
{
    public class Image : Xamarin.Forms.Image
    {

        public static readonly BindableProperty UWPPathProperty = BindableProperty.Create("UWPPath", typeof(string), typeof(Image), string.Empty, BindingMode.Default, null, 
            (view, oldValue, newValue) => (view as Image).UWPPath = (string)newValue);
        public static readonly BindableProperty FileNameProperty = BindableProperty.Create("FileName", typeof(string), typeof(Image), string.Empty, BindingMode.Default, null, 
            (view, oldValue, newValue) => (view as Image).FileName = (string)newValue);

        public string FileName
        {
            get { return GetValue(Image.FileNameProperty) as String; }
            set
            {
                SetValue(Image.FileNameProperty, value);
                if (!String.IsNullOrEmpty(FileName))
                {
                    if (Device.RuntimePlatform == Device.UWP && !String.IsNullOrEmpty(UWPPath))
                    {
                        base.Source = ImageSource.FromFile($"{UWPPath}/{FileName}");
                    }                   
                    else if(Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                    {
                        base.Source = ImageSource.FromFile(FileName);
                    }
                }
            }
        }


        public string UWPPath
        {
            get { return GetValue(Image.UWPPathProperty) as String; }
            set
            {
                SetValue(Image.UWPPathProperty, value);

                if (!String.IsNullOrEmpty(FileName) && Device.RuntimePlatform == Device.UWP)
                {
                    base.Source = ImageSource.FromFile($"{UWPPath}/{FileName}");
                }
            }
        }
    }
}
