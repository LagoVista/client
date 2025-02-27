using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;

namespace LagoVista.Client.Core.Forms.Fields
{
    public class EntityHeaderPickerField : FieldBase
    {
        EntityHeader _value;
        public EntityHeader Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        public EntityHeaderPickerField(FormField formField) : base(formField)
        {

        }

        protected override InvokeResult<object> GetValue()
        {
            return InvokeResult<object>.Create(Value);
        }

        protected override InvokeResult SetValue(object obj)
        {
            Value = obj as EntityHeader;
            return InvokeResult.Success;
        }
    }
}
