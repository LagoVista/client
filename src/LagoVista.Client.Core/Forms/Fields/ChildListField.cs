using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.Client.Core.Forms.Fields
{
    class ChildListField : FieldBase
    {
        ObservableCollection<Object> _items;

        public ChildListField(FormField formField) : base(formField)
        {

        }

        protected override InvokeResult<object> GetValue()
        {
            return InvokeResult<Object>.Create(_items);
        }

        protected override InvokeResult SetValue(object obj)
        {
            _items = new ObservableCollection<Object>((IEnumerable<Object>)obj);
            return InvokeResult.Success;
        }
    }
}
