using LagoVista.Client.Core.Forms.Fields;
using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Attributes;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.Core.Validation;
using NLog.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.Forms
{
    public class FormViewModel<TEntity> : AppViewModelBase where TEntity : EntityBase, new()
    {

        DetailResponse<TEntity> _form;
        public DetailResponse<TEntity> Form
        {
            get => _form;
            set => Set(ref _form, value);
        }

        ObservableCollection<FormField> _formFields;
        public ObservableCollection<FormField> FormFields
        {
            get => _formFields;
            set => Set(ref _formFields, value);
        }

        ObservableCollection<Fields.FieldBase> _fields;
        public ObservableCollection<Fields.FieldBase> Fields
        {
            get => _fields;
            set => Set(ref _fields, value);
        }

        private FieldBase CreateField(FormField formField)
        {
            var fieldType = Enum.Parse<FieldTypes>(formField.FieldType);

            switch (fieldType)
            {
                case FieldTypes.RowId:
                case FieldTypes.Hidden:
                case FieldTypes.Key:
                case FieldTypes.LinkButton:
                case FieldTypes.DateTime:
                case FieldTypes.Bool:
                case FieldTypes.ChildList:
                case FieldTypes.ChildItem:
                case FieldTypes.ChildView:
                case FieldTypes.NodeScript:
                case FieldTypes.JsonDateTime:
                case FieldTypes.OptionsList:
                case FieldTypes.GeoLocation:
                case FieldTypes.Byte:
                case FieldTypes.Icon:
                case FieldTypes.Color:
                case FieldTypes.ChildListInline:
                case FieldTypes.FileUpload:
                case FieldTypes.ChildListInlinePicker:
                case FieldTypes.UserPicker:
                case FieldTypes.Action:
                case FieldTypes.ReadonlyLabel:
                case FieldTypes.Duration:
                case FieldTypes.MediaResources:
                case FieldTypes.ProductPicker:
                case FieldTypes.PaymentMethod:
                case FieldTypes.HtmlEditor:
                case FieldTypes.Discussion:
                case FieldTypes.Category:
                case FieldTypes.Year:
                case FieldTypes.Month:
                case FieldTypes.Money:
                case FieldTypes.SiteContentPicker:
                case FieldTypes.ChildListSiteContentPicker:
                case FieldTypes.Surveys:
                case FieldTypes.DevicePicker:
                case FieldTypes.OrgLocationPicker:
                case FieldTypes.Schedule:
                case FieldTypes.ProductPickerList:
                case FieldTypes.Custom:
                case FieldTypes.SecureCertificate:
                case FieldTypes.CustomerPicker:
                    return new TextField(formField);

                case FieldTypes.Integer: return new IntegerField(formField);
                case FieldTypes.Percent:
                case FieldTypes.Decimal: return new DecimalField(formField);
                case FieldTypes.MultiLineText: return new PickerField(formField);
                case FieldTypes.Picker: return new PickerField(formField);
                case FieldTypes.Date: return new DateField(formField);
                case FieldTypes.Time: return new TimeField(formField);
                case FieldTypes.CheckBox: return new CheckBoxField(formField);
                case FieldTypes.EntityHeaderPicker: return new EntityHeaderPickerField(formField);


                case FieldTypes.Point3D:
                    return new Point3DField(formField);

                case FieldTypes.Point2DSize:
                case FieldTypes.Point2D:
                    return new Point2DField(formField);    

                case FieldTypes.Phone:
                case FieldTypes.Password:
                case FieldTypes.Email:
                case FieldTypes.NameSpace:
                case FieldTypes.Secret:
                case FieldTypes.WebLink:
                case FieldTypes.Text:
                    return new TextField(formField);
            }

            throw new NotImplementedException($"Do not know how to create form field for {fieldType}");
        }

        public override async Task InitAsync()
        {
            var url = LaunchArgs.Parameters["formurl"].ToString();

            var response = await RestClient.GetAsync<DetailResponse<TEntity>>(url);
            if (response.Successful)
            {
                Fields = new ObservableCollection<FieldBase>();
                Form = response.Result;
                var fields = new ObservableCollection<FormField>();
                foreach (var field in Form.FormFields)
                {
                    fields.Add(Form.View[field]);
                    Fields.Add(CreateField(Form.View[field]));
                }

                FormFields = fields;
                Model = Form.Model;
                ModelToView();
            }
        }

        public TEntity Model { get; set; }

        public InvokeResult ModelToView()
        {
            foreach (var field in Fields)
            {
                field.ModelToView(Model);
            }

            return InvokeResult.Success;
        }
    }
}
