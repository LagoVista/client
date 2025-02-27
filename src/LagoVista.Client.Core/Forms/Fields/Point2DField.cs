using LagoVista.Core.Models.Drawing;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.Client.Core.Forms.Fields
{
    public class Point2DField : FieldBase
    {
        Point2D<double> _value;
        public Point2D<double> Value
        {
            get => _value;
            set => Set(ref _value, value);
        }
        public Point2DField(FormField formField) : base(formField)
        {

        }

        protected override InvokeResult<object> GetValue()
        {
            return InvokeResult<object>.Create(Value);
        }

        protected override InvokeResult SetValue(object obj)
        {
            Value = obj as Point2D<double>;

            return InvokeResult.Success;
        }
    }
}
