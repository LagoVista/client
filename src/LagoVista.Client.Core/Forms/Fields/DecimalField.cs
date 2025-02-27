using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Forms.Fields
{
    public class DecimalField : FieldBase
    {
        public Decimal? _value;
        public Decimal? Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        public DecimalField(FormField formField) : base(formField)
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
                Value = Convert.ToDecimal(obj);
            else
                Value = null;

            return InvokeResult.Success;
        }
    }
}
