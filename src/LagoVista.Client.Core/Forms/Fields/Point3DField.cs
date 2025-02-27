using LagoVista.Core.Models.Drawing;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Forms.Fields
{
    public class Point3DField : FieldBase
    {
        Point3D<double> _value;
        public Point3D<double> Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        public Point3DField(FormField formField) : base(formField)
        {
        }

        protected override InvokeResult<object> GetValue()
        {
            return InvokeResult<object>.Create(Value);
        }

        protected override InvokeResult SetValue(object obj)
        {
            Value = obj as Point3D<double>;

            return InvokeResult.Success;
        }
    }
}
