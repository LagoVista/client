using LagoVista.Core;
using System.Linq;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using LagoVista.XPlat.Core.Controls.FormControls;
using LagoVista.Core.Models.UIMetaData;
using System.Diagnostics;

namespace LagoVista.XPlat.Core
{
    public class FormViewer : ScrollView
    {
        StackLayout _container;

        public FormViewer()
        {
            _formControls = new List<FormControl>();

            _container = new StackLayout();
            _container.WidthRequest = 400;
            Content = _container;
        }

        public bool Validate()
        {
            var valid = true;
            foreach (var field in _formControls)
            {
                valid &= field.Validate();
            }

            return valid;
        }

        public bool GetIsDirty()
        {
            var isDirty = false;
            foreach (var field in _formControls)
            {
                if (field.IsDirty)
                {
                    isDirty = true;
                }
            }

            return isDirty;
        }

        public void Refresh()
        {
            foreach (var field in _formControls)
            {
                field.Refresh();
            }
        }

        public void SetViewVisibility(string name, bool isVisible)
        {
            var field = _formControls.Where(fld => fld.Field.Name == name.ToFieldKey()).First();
            field.IsVisible = isVisible;
        }

        public EditFormAdapter Form
        {
            get { return (EditFormAdapter)base.GetValue(FormProperty); }
            set
            {
                base.SetValue(FormProperty, value);
                value.SetValidationMethod(Validate);
                value.SetGetIsDirtyMethod(GetIsDirty);
                value.SetRefreshMethod(Refresh);
                value.SetVisibilityMethod(SetViewVisibility);
                Populate();
            }
        }

        public static BindableProperty FormProperty = BindableProperty.Create(
                                                            propertyName: nameof(Form),
                                                            returnType: typeof(EditFormAdapter),
                                                            declaringType: typeof(FormViewer),
                                                            defaultValue: null,
                                                            defaultBindingMode: BindingMode.Default,
                                                            propertyChanged: HandleFormFieldsAssigned);

        private static void HandleFormFieldsAssigned(BindableObject bindable, object oldValue, object newValue)
        {
            var viewer = (FormViewer)bindable;
            viewer.Form = newValue as EditFormAdapter;
        }

        private void AddChild(FormControl field)
        {
            _formControls.Add(field);
            _container.Children.Add(field);
        }

        private List<FormControl> _formControls;

        protected void Populate()
        {
            _formControls.Clear();
            _container.Children.Clear();

            if (Form != null)
            {
                foreach (var field in Form.FormItems)
                {
                    switch (field.FieldType)
                    {
                        case FormField.FieldType_MultilineText: AddChild(new TextAreaRow(this, field)); break;
                        case "Bool": AddChild(new CheckBoxRow(this, field)); break;
                        case FormField.FieldType_CheckBox:
                            var cbRow = new CheckBoxRow(this, field);
                            cbRow.OptionSelected += Picker_OptionSelected;
                            AddChild(cbRow);
                            break;
                        case FormField.FeildType_EntityHeaderPicker:
                            var ehPicker = new EntityHeaderPicker(this, field);
                            ehPicker.PickerTapped += EhPicker_PickerTapped;
                            AddChild(ehPicker);
                            break;
                        case FormField.FieldType_Picker:
                            var picker = new SelectRow(this, field);
                            picker.OptionSelected += Picker_OptionSelected;
                            AddChild(picker);
                            break;
                        case FormField.FieldType_ChildList:
                            var childListControl = new ChildListRow(this, field);
                            if (Form.ChildLists.ContainsKey(field.Name))
                            {
                                childListControl.ChildItems = Form.ChildLists[field.Name];
                            }
                            childListControl.Add += ChildListControl_Add;
                            childListControl.Deleted += ChildListControl_Deleted;
                            childListControl.ItemSelected += ChildListControl_ItemSelected;
                            AddChild(childListControl);
                            break;
                        case FormField.FieldType_LinkButton: AddChild(new LinkButton(this, field)); break;
                        default: AddChild(new TextEditRow(this, field)); break;
                    }
                }
            }
        }

        private void EhPicker_PickerTapped(object sender, string e)
        {
           Form.InvokeEHPickerTapped(e);
        }

        private void ChildListControl_Deleted(object sender, DeleteItemEventArgs e)
        {
            Form.InvokeItemDeleted(e);
        }

        private void Picker_OptionSelected(object sender, OptionSelectedEventArgs e)
        {
            Form.InvokeOptionSelected(e);
        }

        private void ChildListControl_ItemSelected(object sender, ItemSelectedEventArgs e)
        {
            Form.InvokeItemSelected(e);
        }

        private void ChildListControl_Add(object sender, string childType)
        {
            Form.InvokeAdd(childType);
        }
    }
}
