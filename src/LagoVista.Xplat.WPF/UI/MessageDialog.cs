using System;
using System.Windows;
using System.Windows.Controls;

namespace LagoVista.Core.WPF.UI
{
    internal class MessageDialog : Window
    {
        TextBox _textInput;
        TextBlock _help;

        public bool isRequired { get; set; }

        public MessageDialog()
        {
            Width = 400;
            Height = 400;
            var container = new Grid();
            container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            Content = container;

            Button okButton = new Button();
            okButton.Margin = new Thickness(4);
            okButton.Content = "Done";
            okButton.SetValue(Grid.ColumnProperty, 1);
            okButton.SetValue(Grid.RowProperty, 2);
            container.Children.Add(okButton);
            okButton.Click += OkButton_Click;

            Button cancelButton = new Button();
            cancelButton.Margin = new Thickness(4);
            cancelButton.Content = "Cancel";
            cancelButton.SetValue(Grid.ColumnProperty, 2);
            cancelButton.SetValue(Grid.RowProperty, 2);
            container.Children.Add(cancelButton);
            cancelButton.Click += (sndr, args) => { DialogResult = false; Close(); };

            _help = new TextBlock();
            _help.Margin = new Thickness(16);
            _help.SetValue(Grid.ColumnSpanProperty, 3);
            _help.Visibility = Visibility.Collapsed;
            _help.FontSize = 18;
            _help.TextWrapping = TextWrapping.Wrap;
            container.Children.Add(_help);


            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            this.WindowStyle = WindowStyle.ToolWindow;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true; Close();
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
                _help.Visibility = Visibility.Visible;
            }
        }

    }


}
