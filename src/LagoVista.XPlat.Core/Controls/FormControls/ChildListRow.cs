using LagoVista.Core.Interfaces;
using LagoVista.Core.IOC;
using LagoVista.Core.Models.UIMetaData;
using LagoVista.XPlat.Core.Resources;
using LagoVista.XPlat.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core.Controls.FormControls
{
    public class ChildListRow : FormControl
    {
        Label _label;
        IconButton _addImage;
        StackLayout _childItemList;

        public event EventHandler<string> Add;
        public event EventHandler<DeleteItemEventArgs> Deleted;

        public event EventHandler<ItemSelectedEventArgs> ItemSelected;

        public ChildListRow(FormViewer formViewer, FormField field) : base(formViewer, field)
        {
            HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true);

            var titleBar = new Grid()
            {
                HeightRequest = 48,
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true)
            };
            titleBar.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
            titleBar.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            _label = new Label()
            {
                Margin = new Thickness(24, 0, 0, 0),
                VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                Text = field.Label,
                FontAttributes = FontAttributes.Bold,
                FontFamily = ResourceSupport.GetString("ListItemFont"),
                FontSize = ResourceSupport.GetNumber("ListItemFontSize")
            };

            _addImage = new IconButton()
            {
                Margin = new Thickness(0, 0, 20, 0),
                VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                WidthRequest = 48,
                HeightRequest = 48,
                FontSize = Device.RuntimePlatform == Device.Android ? 20 : 28
            };

            switch (Device.RuntimePlatform)
            {
                case Device.iOS: _addImage.IconKey = "md-add"; break;
                default: _addImage.IconKey = "ep-plus"; break;
            }

            if (Device.RuntimePlatform != Device.UWP)
            {
                titleBar.BackgroundColor = ResourceSupport.GetColor("TitleBarBackground");
                _label.TextColor = ResourceSupport.GetColor("TitleBarText");
                _addImage.TextColor = ResourceSupport.GetColor("HighlightColor");
            }


            _addImage.Clicked += _addImage_Clicked;
            _addImage.SetValue(Grid.ColumnProperty, 1);

            _childItemList = new StackLayout();

            titleBar.Children.Add(_label);
            titleBar.Children.Add(_addImage);

            Children.Add(titleBar);
            Children.Add(_childItemList);
        }

        private void _addImage_Clicked(object sender, EventArgs e)
        {
            Add?.Invoke(this, Field.Name);
        }

        private void Item_Tapped(object sender, EventArgs e)
        {
            var childItem = (sender as Grid).BindingContext as IEntityHeaderEntity;

            ItemSelected?.Invoke(this, new ItemSelectedEventArgs()
            {
                Id = childItem.ToEntityHeader().Id,
                Type = Field.Name
            });
        }

        public override void Refresh()
        {
            _childItemList.Children.Clear();

            if (_childItems != null)
            {
                foreach (var child in _childItems)
                {
                    var label = new Label();
                    label.Margin = new Thickness(15, 10, 10, 10);
                    label.Text = child.ToEntityHeader().Text;
                    label.FontFamily = ResourceSupport.GetString("ListItemFont");
                    label.FontSize = ResourceSupport.GetNumber("ListItemFontSize");

                    var grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                    var boxView = new BoxView();
                    boxView.HeightRequest = 1;
                    boxView.SetValue(Grid.ColumnSpanProperty, 3);
                    boxView.SetValue(Grid.RowProperty, 1);
                  
                    var tapGenerator = new TapGestureRecognizer();
                    grid.BindingContext = child;
                    tapGenerator.Tapped += Item_Tapped;

                    var deleteButton = new IconButton()
                    {
                        IconKey = "typcn-delete-outline",
                        FontSize = 28,
                        VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                        Tag = child.ToEntityHeader().Id
                    };

                    deleteButton.SetValue(Grid.ColumnProperty, 1);
                    deleteButton.Clicked += DeleteButton_Clicked;

                    var img = new Icon()
                    {
                        IconKey = "md-chevron-right",
                        Margin = new Thickness(2, 0, 10, 0),
                        FontSize = 36,
                        VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                    };
                    img.SetValue(Grid.ColumnProperty, 2);

                    if (Device.RuntimePlatform != Device.UWP)
                    {
                        label.TextColor = ResourceSupport.GetColor("PageText");
                        boxView.Color = ResourceSupport.GetColor("TitleBarBackground");
                        deleteButton.TextColor = ResourceSupport.GetColor("DangerColor");
                        img.TextColor = ResourceSupport.GetColor("NavIconColor");
                    }

                    grid.GestureRecognizers.Add(tapGenerator);

                    grid.Children.Add(label);
                    grid.Children.Add(boxView);
                    grid.Children.Add(deleteButton);
                    grid.Children.Add(img);

                    _childItemList.Children.Add(grid);
                }
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var btn = sender as IconButton;
            var deleteItemArgs = new DeleteItemEventArgs()
            {
                Id = btn.Tag as String,
                Type = Field.Name.ToPropertyName()
            };

            var obj = ChildItems as Object;

            if (obj is System.Collections.IList)
            {
                if (await SLWIOC.Get<LagoVista.Core.PlatformSupport.IPopupServices>().ConfirmAsync(XPlatResources.Msg_ConfirmDeleteItemTitle, XPlatResources.Msg_ConfirmDeleteItem))
                {
                    var childList = ChildItems as System.Collections.IList;
                    var itemToBeDeleted = ChildItems.Where(itm => itm.ToEntityHeader().Id == btn.Tag as string).FirstOrDefault();
                    childList.Remove(itemToBeDeleted);
                    Deleted?.Invoke(sender, deleteItemArgs);
                    Refresh();
                }
            }
        }

        IEnumerable<IEntityHeaderEntity> _childItems;
        public IEnumerable<IEntityHeaderEntity> ChildItems
        {
            get { return _childItems; }
            set
            {
                _childItems = value;
                if (value is INotifyCollectionChanged)
                {
                    (value as INotifyCollectionChanged).CollectionChanged += ChildListRow_CollectionChanged;
                }
                Refresh();
            }
        }

        private void ChildListRow_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }

        public override bool Validate()
        {
            return true;
        }
    }
}
