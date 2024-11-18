using LagoVista.Core.Attributes;
using LagoVista.Core.Commanding;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.XPlat.Core.Services;
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.FormControls
{
    public class TextEditRow : FormControl
    {
        FormFieldHeader _header;
        FormFieldValidationMessage _validationMessage;
        Entry _editor;

        Grid _editorContainer;

        RelayCommand _command;

        public const string VIEW_SECRET = "ViewSecret";
        public const string COPY_SECRET = "CopySecret";
        public const string REFRESH_SECRET = "RefreshSecret";

        public TextEditRow(FormViewer formViewer, FormField field) : base(formViewer, field)
        {
            _editorContainer = new Grid();

            _editorContainer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            _command = field.Command;

            _header = new FormFieldHeader(field.Label);

            _editor = new Entry()
            {
                Text = field.Value,
                IsEnabled = field.IsUserEditable,
                Placeholder = field.Watermark
            };

            _editor.IsPassword = field.FieldType == FieldTypes.Password.ToString();
            _editor.TextChanged += _editor_TextChanged;

            _editorContainer.Children.Add(_editor);

            _editorContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            if (Enum.TryParse<FieldTypes>(field.FieldType, out FieldTypes fieldType))
            {
                switch (FieldType)
                {
                    case FieldTypes.Secret:
                        _editorContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                        _editorContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                        _editorContainer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                        _editorContainer.Children.Add(new IconButton() { Command = _command, WidthRequest = 36, CommandParameter = VIEW_SECRET, IconKey = "fa-eye", TextColor = ResourceSupport.AccentColor }, 1, 0);
                        _editorContainer.Children.Add(new IconButton() { Command = _command, WidthRequest = 36, CommandParameter = COPY_SECRET, IconKey = "fa-clipboard", TextColor = ResourceSupport.AccentColor }, 2, 0);
                        _editorContainer.Children.Add(new IconButton() { Command = _command, WidthRequest = 36, CommandParameter = REFRESH_SECRET, IconKey = "fa-refresh", TextColor = ResourceSupport.AccentColor }, 3, 0);

                        break;
                    case FieldTypes.Key:
                        _editor.Keyboard = Keyboard.Plain;
                        break;
                    case FieldTypes.Decimal:
                    case FieldTypes.Integer:
                        _editor.Keyboard = Keyboard.Numeric;
                        _editor.HorizontalTextAlignment = TextAlignment.End;
                        break;
                }

                _validationMessage = new FormFieldValidationMessage(field.RequiredMessage);

                Children.Add(_header);
                Children.Add(_editorContainer);
                Children.Add(_validationMessage);
                Margin = RowMargin;
            }

            _editor.Focus();

            Refresh();
        }

        public override void Refresh()
        {
            base.Refresh();
            _editor.Text = Field.Value;
        }


        public override bool Validate()
        {
            if (Field.IsRequired && String.IsNullOrEmpty(Field.Value))
            {
                _validationMessage.IsVisible = true;
                _validationMessage.Text = Field.RequiredMessage;
                return false;
            }

            if (!String.IsNullOrEmpty(Field.RegEx) && !String.IsNullOrEmpty(Field.Value))
            {
                var regEx = new Regex(Field.RegEx);
                if (!regEx.Match(Field.Value).Success)
                {
                    _validationMessage.Text = Field.RegExMessage;
                    _validationMessage.IsVisible = true;
                }
            }

            return true;
        }

        private void _editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.NewTextValue))
            {
                switch (FieldType)
                {
                    case FieldTypes.Integer:
                        {
                            if (!int.TryParse(e.NewTextValue, out int value))
                            {
                                _editor.Text = e.OldTextValue;
                            }
                        }
                        break;
                    case FieldTypes.Decimal:
                        {
                            if (!double.TryParse(e.NewTextValue, out double value))
                            {
                                _editor.Text = e.OldTextValue;
                            }
                        }
                        break;
                }
            }

            Field.Value = e.NewTextValue;
            IsDirty = OriginalValue != Field.Value;

            if (Field.IsRequired && String.IsNullOrEmpty(Field.Value))
            {
                _validationMessage.IsVisible = true;
                _validationMessage.Text = Field.RequiredMessage;
            }
            else if (!String.IsNullOrEmpty(Field.RegEx) && !String.IsNullOrEmpty(Field.Value))
            {
                var regEx = new Regex(Field.RegEx);
                if (!regEx.Match(Field.Value).Success)
                {
                    _validationMessage.Text = Field.RegExMessage;
                    _validationMessage.IsVisible = true;
                }
                else
                {
                    _validationMessage.IsVisible = false;
                }
            }
            else
            {
                _validationMessage.IsVisible = false;
            }
        }
    }
}