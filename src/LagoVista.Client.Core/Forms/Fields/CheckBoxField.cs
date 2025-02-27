using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;

namespace LagoVista.Client.Core.Forms.Fields
{
    public class CheckBoxField : FieldBase
    {
        private bool _checked;
        public bool Checked
        {
            get => _checked;
            set => Set(ref _checked, value);
        }

        public CheckBoxField(FormField formField) : base(formField)
        {
        }

        protected override InvokeResult<object> GetValue()
        {
            return InvokeResult<object>.Create(Checked);
        }

        protected override InvokeResult SetValue(object obj)
        {
            if(obj != null)
            {
                Checked = Convert.ToBoolean(obj);
            }
            else
            {
                Checked = false;
            }

            return InvokeResult.Success;
        }
    }
}
