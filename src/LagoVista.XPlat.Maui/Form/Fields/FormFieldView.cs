using LagoVista.Client.Core.Forms.Fields;
using LagoVista.Core.Attributes;

namespace LagoVista.XPlat.Maui.Form.Fields
{
    public class FormFieldView : ContentView
    {
        public FormFieldView()
        {
            this.BindingContextChanged += FormFieldView_BindingContextChanged;
        }

        public static readonly BindableProperty FieldProperty =
            BindableProperty.CreateAttached(
                nameof(Field),
                typeof(FieldBase),
                typeof(FormFieldView),null);

        public FieldBase Field
        {
            get { return (FieldBase)GetValue(FieldProperty); }
            set { SetValue(FieldProperty, value); }
        }

        private void FormFieldView_BindingContextChanged(object? sender, EventArgs e)
        {
            var field = BindingContext as FieldBase;
            var fieldType = Enum.Parse<FieldTypes>(field.FormField.FieldType);

            var view = new TextFieldView();
            view.BindingContext = field;

            switch (fieldType)
            {
                case FieldTypes.RowId:
                    break;
                case FieldTypes.Picker:
                    break;
                case FieldTypes.Hidden:
                    break;
                case FieldTypes.Text:
                    break;
                case FieldTypes.Key:
                    break;
                case FieldTypes.MultiLineText:
                    break;
                case FieldTypes.Integer:
                    break;
                case FieldTypes.Decimal:
                    break;
                case FieldTypes.Date:
                    break;
                case FieldTypes.Time:
                    break;
                case FieldTypes.LinkButton:
                    break;
                case FieldTypes.DateTime:
                    break;
                case FieldTypes.Phone:
                    break;
                case FieldTypes.Password:
                    break;
                case FieldTypes.Email:
                    break;
                case FieldTypes.Bool:
                    break;
                case FieldTypes.ChildList:
                    break;
                case FieldTypes.ChildItem:
                    break;
                case FieldTypes.ChildView:
                    break;
                case FieldTypes.NodeScript:
                    break;
                case FieldTypes.JsonDateTime:
                    break;
                case FieldTypes.NameSpace:
                    break;
                case FieldTypes.CheckBox:
                    break;
                case FieldTypes.OptionsList:
                    break;
                case FieldTypes.GeoLocation:
                    break;
                case FieldTypes.EntityHeaderPicker:
                    break;
                case FieldTypes.Byte:
                    break;
                case FieldTypes.Secret:
                    break;
                case FieldTypes.Icon:
                    break;
                case FieldTypes.Color:
                    break;
                case FieldTypes.ChildListInline:
                    break;
                case FieldTypes.FileUpload:
                    break;
                case FieldTypes.ChildListInlinePicker:
                    break;
                case FieldTypes.UserPicker:
                    break;
                case FieldTypes.WebLink:
                    break;
                case FieldTypes.Action:
                    break;
                case FieldTypes.ReadonlyLabel:
                    break;
                case FieldTypes.Duration:
                    break;
                case FieldTypes.MediaResources:
                    break;
                case FieldTypes.ProductPicker:
                    break;
                case FieldTypes.PaymentMethod:
                    break;
                case FieldTypes.HtmlEditor:
                    break;
                case FieldTypes.Discussion:
                    break;
                case FieldTypes.Category:
                    break;
                case FieldTypes.Year:
                    break;
                case FieldTypes.Month:
                    break;
                case FieldTypes.Money:
                    break;
                case FieldTypes.SiteContentPicker:
                    break;
                case FieldTypes.ChildListSiteContentPicker:
                    break;
                case FieldTypes.Surveys:
                    break;
                case FieldTypes.DevicePicker:
                    break;
                case FieldTypes.OrgLocationPicker:
                    break;
                case FieldTypes.Schedule:
                    break;
                case FieldTypes.ProductPickerList:
                    break;
                case FieldTypes.Custom:
                    break;
                case FieldTypes.Percent:
                    break;
                case FieldTypes.SecureCertificate:
                    break;
                case FieldTypes.CustomerPicker:
                    break;
                case FieldTypes.Point2D:
                    break;
                case FieldTypes.Point3D:
                    break;
                case FieldTypes.Point2DSize:
                    break;
            }
        }
    }
}
