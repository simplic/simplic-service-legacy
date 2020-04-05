using System.Windows;
using System.Windows.Controls;

namespace Simplic.User.UI
{
    class ListViewWithDragAndDrop : ListView
    {
        private ListViewDragDropManager<UserViewModel> _dragMgr;
        public ListViewWithDragAndDrop() : base()
        {
            Loaded += OnListViewWithDragAndDropLoaded;
        }

        private void OnListViewWithDragAndDropLoaded(object sender, RoutedEventArgs e)
        {
            if (_dragMgr == null)
                _dragMgr = new ListViewDragDropManager<UserViewModel>(this);
        }
    }
}
