using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace LagoVista.XPlat.Core
{
    // TODO: May want to look at Layout<TabContent>
    public class TabContentHolder : Grid
    {
        ObservableCollection<TabContent> _tabContentCollection;
        public TabContentHolder()
        {
            _tabContentCollection = new ObservableCollection<TabContent>();
            ChildAdded += TabContentHolder_ChildAdded;
        }

        private void TabContentHolder_ChildAdded(object sender, ElementEventArgs e)
        {
            var content = Children.Last() as TabContent;
            if (content == null)
            {
                throw new ArgumentException("Should only add tabs to tab bar.");
            }

            _tabContentCollection.Add(content);

            content.IsVisible = _tabContentCollection.Count == 1;
        }

        public void ShowTab(int visibleTab)
        {
            for (int idx = 0; idx < _tabContentCollection.Count; ++idx)
            {
                _tabContentCollection[idx].IsVisible = idx == visibleTab;
            }
        }      
    }

    public class TabHeaderHolder : Grid
    {

    }

}
