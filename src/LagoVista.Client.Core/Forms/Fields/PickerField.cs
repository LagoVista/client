
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Forms.Fields
{
    public class PickerField : FieldBase
    {
        private string _selectedId;
        public string SelectedId
        {
            get => _selectedId;
            set => Set(ref _selectedId, value);
        }

        public List<EnumDescription> Options { get; }

        public PickerField(FormField formField) : base(formField)
        {
            var selectMessage = String.IsNullOrEmpty(formField.Watermark) ? "-select-" : formField.Watermark;

            formField.Options.Insert(0, new EnumDescription() { Id = "-1", Key = "-1", Label = selectMessage });
            Options = formField.Options;
        }

        protected override InvokeResult<object> GetValue()
        {
            throw new NotImplementedException();
        }

        protected override InvokeResult SetValue(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
