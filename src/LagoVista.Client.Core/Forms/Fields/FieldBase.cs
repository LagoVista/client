using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace LagoVista.Client.Core.Forms.Fields
{
    public abstract class FieldBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T storage, T value, string columnName = null, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                var expr = property.Body as MemberExpression;
                if (expr != null)
                    eventHandler(this, new PropertyChangedEventArgs(expr.Member.Name));
            }
        }


        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public readonly FormField FormField;
        public FieldBase(FormField formField)
        {
            FormField = formField;
        }

        public InvokeResult ModelToView(EntityBase model)
        {
            var modelType = model.GetType();

            var properties = modelType.GetProperties();
            var property = properties.ToList().FirstOrDefault(prp => prp.Name.ToLower() == FormField.Name.ToLower());
            var value = property.GetValue(model);
            
            return SetValue(value);
        }

        public InvokeResult ViewToModel(EntityBase model)
        {
            var modelType = model.GetType();
            var properties = modelType.GetProperties();

            var property = properties.ToList().FirstOrDefault( prp => prp.Name.ToLower() == FormField.Name.ToLower());
            var value = GetValue();
            property.SetValue(model, value);

            return InvokeResult.Success;
        }

        protected abstract InvokeResult SetValue(Object obj);

        protected abstract InvokeResult<Object> GetValue(); 
    }
}
