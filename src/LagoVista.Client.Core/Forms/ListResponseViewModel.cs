using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Models.UIMetaData;
using NLog.Filters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.Forms
{
    public class ListResponseViewModel<TEntitySummary> : AppViewModelBase where TEntitySummary : class, ISummaryData
    {
        private ListResponse<TEntitySummary> _items;
        public ListResponse<TEntitySummary> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        public override async Task InitAsync()
        {
            var type = typeof(TEntitySummary);
            var formAttr = type.GetCustomAttribute<EntityDescriptionAttribute>();

            var response = await RestClient.GetListResponseAsync<TEntitySummary>(formAttr.GetListUrl);
            if (response.Successful)
            {
                Items = response;
            }
        }

        TEntitySummary _selectedItem;
        public TEntitySummary SelectedItem
        {
            get => _selectedItem;
            set
            {
                Set(ref _selectedItem, value);
                if (value != null)
                {
                    var formUrl = Items.GetUrl.Replace("{id}", value.Id);
                    ShowView<FormViewModel<EntityBase>>(new KeyValuePair<string, object>("formurl", formUrl));
                }
            }
        }
    }
}
