using LagoVista.Core.Commanding;
using LagoVista.XPlat.Core.Services;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core
{
    /// <summary>
    /// Entry has a customer renderer that will leave out the underbar, this is useful with borders around text boxes
    /// </summary>
    public class Entry : Xamarin.Forms.Entry
    {
        public Entry()
        {
            if(ResourceSupport.UseCustomfonts)
            {
                FontFamily = ResourceSupport.GetString("EntryFont");
                FontSize = ResourceSupport.GetNumber("EntryFontSize");
            }

            if (ResourceSupport.UseCustomColors)
            {
                PlaceholderColor = ResourceSupport.GetColor("EditControlPlaceholder");
                BackgroundColor = ResourceSupport.GetColor("EditControlBackground");
                TextColor = ResourceSupport.GetColor("EditControlText");
            }

            TextChanged += Entry_TextChanged;
        }

        public static readonly BindableProperty TextChangedCommandProperty = BindableProperty.Create(nameof(TextChangedCommand), typeof(RelayCommand),
            typeof(Entry), null, BindingMode.OneWay, null, (view, oldValue, newValue) => (view as Entry).TextChangedCommand = (RelayCommand)newValue);

        public static readonly BindableProperty TextClearedCommandProperty = BindableProperty.Create(nameof(TextClearedCommand), typeof(RelayCommand),
            typeof(Entry), null, BindingMode.OneWay, null, (view, oldValue, newValue) => (view as Entry).TextClearedCommand = (RelayCommand)newValue);

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                TextClearedCommand?.Execute(null);
            }
            else
            {
                TextChangedCommand?.Execute(this.Text);
            }
        }
        public RelayCommand TextClearedCommand
        {
            get { return (RelayCommand)base.GetValue(TextClearedCommandProperty); }
            set { base.SetValue(TextClearedCommandProperty, value); }
        }

        public RelayCommand TextChangedCommand
        {
            get { return (RelayCommand)base.GetValue(TextChangedCommandProperty); }
            set { base.SetValue(TextChangedCommandProperty, value); }
        }
    }
}
