using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;

namespace LagoVista.Client.Core.Forms.Fields
{
    public class TextField : FieldBase
    {
        public String _value;
        public String Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        public TextField(FormField formField) : base(formField)
        {
        }

        protected override InvokeResult<object> GetValue()
        {
            return InvokeResult<object>.Create(Value);
        }

        protected override InvokeResult SetValue(object obj)
        {
            if(obj != null)
                Value = Convert.ToString(obj);
            else
                Value = null;

            return InvokeResult.Success;
        }
    }
}
