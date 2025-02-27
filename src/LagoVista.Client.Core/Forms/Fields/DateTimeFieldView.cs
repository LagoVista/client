using LagoVista.Core;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Forms.Fields
{
    public class DateTimeField : FieldBase
    {
        DateTime? _dateTime;

        public DateTimeField(FormField formField) : base(formField)
        {
        }

        protected override InvokeResult<object> GetValue()
        {
            return InvokeResult<object>.Create(_dateTime.HasValue ? _dateTime.ToJSONString() : null);
        }

        protected override InvokeResult SetValue(object obj)
        {
            _dateTime = obj == null ? (DateTime?)null : DateTime.Parse(obj.ToString());
            return InvokeResult.Success;
        }
    }
}
