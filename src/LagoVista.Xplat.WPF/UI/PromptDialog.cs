using System;
using System.Windows;
using System.Windows.Controls;

namespace LagoVista.Core.WPF.UI
{
    internal class PromptDialog<T> : Window
    {
        TextBox _textInput;
        TextBlock _help;

        Button _okButton;
        Button _cancelButton;

        public bool isRequired { get; set; }

        public PromptDialog()
        {
            Width = 200;
            Height = 160;
            var container = new Grid();
            container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            Content = container;

            _okButton = new Button();
            _okButton.Margin = new Thickness(4);
            _okButton.Content = "Ok";
            _okButton.SetValue(Grid.ColumnProperty, 1);
            _okButton.SetValue(Grid.RowProperty, 2);
            container.Children.Add(_okButton);
            _okButton.Click += OkButton_Click;

            _cancelButton = new Button();
            _cancelButton.Margin = new Thickness(4);
            _cancelButton.Content = "Cancel";
            _cancelButton.SetValue(Grid.ColumnProperty, 2);
            _cancelButton.SetValue(Grid.RowProperty, 2);
            container.Children.Add(_cancelButton);
            _cancelButton.Click += (sndr, args) => { DialogResult = false; Close(); };

            _help = new TextBlock();
            _help.Margin = new Thickness(8);
            _help.SetValue(Grid.ColumnSpanProperty, 3);
            _help.Visibility = Visibility.Collapsed;
            _help.FontSize = 18;
            container.Children.Add(_help);

            _textInput = new TextBox();
            _textInput.SetValue(Grid.RowProperty, 1);
            _textInput.SetValue(Grid.ColumnSpanProperty, 3);
            _textInput.Margin = new Thickness(8);
            _help.TextWrapping = TextWrapping.Wrap;

            container.Children.Add(_textInput);

            this.WindowStyle = WindowStyle.ToolWindow;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            if (typeof(T) == typeof(int))
            {
                _textInput.TextAlignment = TextAlignment.Right;
                Masking.SetMask(_textInput, @"^\d+$");
            }

            if (typeof(T) == typeof(decimal))
            {
                _textInput.TextAlignment = TextAlignment.Right;
                Masking.SetMask(_textInput, @"^[0-9]+\.?([0-9][0-9]?)?$");
            }

            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        public bool TextInputVisible
        {
            get { return _textInput.Visibility == Visibility.Visible; }
            set { _textInput.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }
        
        public bool OkButtonVisible
        {
            get { return _okButton.Visibility == Visibility.Visible; }
            set { _okButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public bool CancelButtonVisible
        {
            get { return _cancelButton.Visibility == Visibility.Visible; }
            set { _cancelButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public string OkButtonCaption
        {
            get { return _okButton.Content.ToString(); }
            set { _okButton.Content = value; }
        }

        public string CancelButtonCaption
        {
            get { return _cancelButton.Content.ToString(); }
            set { _cancelButton.Content = value; }
        }


        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid)
            {
                DialogResult = true; Close();
            }
            else
            {
                if (typeof(T) == typeof(int))
                {
                    MessageBox.Show("Please enter a valid number.");
                }
                else if (typeof(T) == typeof(decimal))
                {
                    MessageBox.Show("Please enter a valid decimal.");
                }
                else if (typeof(T) == typeof(string))
                {
                    MessageBox.Show("Please enter a valid string.");
                }
            }
        }

        public String Help
        {
            get
            {
                return _help.Text;
            }
            set
            {
                _help.Text = value;
                if (String.IsNullOrEmpty(value))
                {
                    Height = 160;
                    _help.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Height = 190;
                    _help.Visibility = Visibility.Visible;
                }
            }
        }

        public bool HasValue
        {
            get { return !String.IsNullOrEmpty(_textInput.Text); }
        }

        public bool IsValid
        {
            get
            {
                if (!isRequired)
                {
                    return true;
                }

                if (String.IsNullOrEmpty(_textInput.Text))
                {
                    return false;
                }

                if (typeof(T) == typeof(int))
                {
                    decimal val;
                    return decimal.TryParse(_textInput.Text, out val);
                }
                else if (typeof(T) == typeof(decimal))
                {
                    decimal val;
                    return decimal.TryParse(_textInput.Text, out val);
                }
                else if (typeof(T) == typeof(string))
                {
                    return true;
                }
                else
                {
                    throw new Exception("Invalid Type");
                }
            }
        }

        public String StringValue
        {
            get { return _textInput.Text; }
            set { _textInput.Text = value; }
        }

        public Double? DoubleValue
        {
            get
            {
                Double val;
                if (Double.TryParse(_textInput.Text, out val))
                    return val;
                else
                    return null;
            }
            set
            {
                _textInput.Text = value.HasValue ? value.ToString() : String.Empty;
            }
        }

        public int? IntValue
        {
            get
            {
                int val;
                if (int.TryParse(_textInput.Text, out val))
                    return val;
                else
                    return null;
            }
            set
            {
                _textInput.Text = value.HasValue ? value.ToString() : String.Empty;
            }
        }
    }


}
