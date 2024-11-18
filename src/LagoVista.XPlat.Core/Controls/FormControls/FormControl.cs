using LagoVista.Core.Attributes;
using LagoVista.Core.Models.UIMetaData;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.FormControls
{
    public abstract class FormControl : StackLayout, INotifyPropertyChanged
    {
        FormField _field;
        FormViewer _viewer;

        public FormControl(FormViewer viewer, FormField field)
        {
            _field = field;
            _viewer = viewer;
            IsVisible = field.IsVisible;
            OriginalValue = _field.Value;            
        }

        public FormField Field { get { return _field; } }
        protected FormViewer Viewer { get { return _viewer; } }

        public abstract bool Validate();

        public virtual void Refresh()
        {
            IsVisible = _field.IsVisible;
            IsEnabled = _field.IsUserEditable;

            Debug.WriteLine($"{Field.Name} - {Field.IsVisible} - {IsVisible}");
        }

        public FieldTypes FieldType
        {
            get
            {
                FieldTypes fieldType;
                if (Enum.TryParse<FieldTypes>(Field.FieldType, out fieldType))
                {
                    return fieldType;
                }
                else
                {
                    throw new Exception("Could not create field type from field type: " + Field.FieldType);
                }
            }
        }

        private bool _isDirty;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                OnPropertyChanged();
            }
        }

        public String OriginalValue { get; set; }

        protected Thickness RowMargin
        {
            get
            {
                switch (Device.RuntimePlatform)
                {

                    case Device.Android: return new Thickness(20, 0, 10, 20);
                }

                return new Thickness(20, 5, 10, 8);
            }
        }
    }
}