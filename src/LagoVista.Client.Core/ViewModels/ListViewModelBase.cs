using LagoVista.Client.Core.Net;
using LagoVista.Core.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LagoVista.Client.Core.ViewModels
{
    public abstract class ListViewModelBase<TSummaryModel> : AppViewModelBase, IListViewModel where TSummaryModel : class
    {
        private readonly ListRestClient<TSummaryModel> _formRestClient;
        private bool _shouldRefresh = false;

        public ListViewModelBase()
        {
            _formRestClient = new ListRestClient<TSummaryModel>(RestClient);
        }

        public ListRestClient<TSummaryModel> FormRestClient { get { return _formRestClient; } }

        IEnumerable<TSummaryModel> _listItems;
        public virtual IEnumerable<TSummaryModel> ListItems
        {
            get { return _listItems; }
            protected set
            {
                Set(ref _listItems, value);
                IsListEmpty = value != null && !value.Any();
            }
        }

        private bool _isListEmpty = false;
        public bool IsListEmpty
        {
            get { return _isListEmpty; }
            set
            {
                Set(ref _isListEmpty, value);
                RaisePropertyChanged(nameof(ListHasData));
            }
        }

        public bool ListHasData
        {
            get { return !_isListEmpty; }
        }

        protected async Task<InvokeResult> LoadItems()
        {
            ListItems = null;
            var listResponse = await FormRestClient.GetForOrgAsync(GetListURI(), null);
            if (listResponse.Successful)
            {
                SetListItems(listResponse.Model);
            }

            return listResponse.ToInvokeResult();
        }

        protected virtual void SetListItems(IEnumerable<TSummaryModel> items)
        {
            ListItems = items;
        }

        protected abstract string GetListURI();


        public override Task InitAsync()
        {
            return PerformNetworkOperation(LoadItems);
        }

        public override async Task ReloadedAsync()
        {
            if (_shouldRefresh)
            {
                await PerformNetworkOperation(LoadItems);
                _shouldRefresh = false;
            }
        }

        public void MarkAsShouldRefresh()
        {
            _shouldRefresh = true;
        }

        protected virtual void ItemSelected(TSummaryModel model) { }

        /* so far will always be null just used to detect clicking on object */
        TSummaryModel _selectedItem;
        public TSummaryModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value != null && _selectedItem != value)
                {
                    _selectedItem = value;
                    ItemSelected(value);
                }
                else if (value == null)
                {
                    _selectedItem = null;
                }

                RaisePropertyChanged();
            }
        }
    }

    public interface IListViewModel
    {
        void MarkAsShouldRefresh();
    }
}