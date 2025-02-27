using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;

namespace LagoVista.Client.Core.Forms.Fields
{
    public class IntegerField : FieldBase
    {
        public Int32? _value;
        public Int32? Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        public IntegerField(FormField formField) : base(formField)
        {

        }

        protected override InvokeResult<object> GetValue()
        {
            if (_value.HasValue)
                return InvokeResult<object>.Create(Value.Value);

            return InvokeResult<object>.Create(null);
        }

        protected override InvokeResult SetValue(object obj)
        {
            if (obj != null)
                Value = Convert.ToInt32(obj);
            else
                Value = null;

            return InvokeResult.Success;
        }
    }
}
