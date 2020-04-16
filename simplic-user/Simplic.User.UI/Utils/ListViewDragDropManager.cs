using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;
using Simplic.UI.MVC;

namespace Simplic.User.UI
{
    public class ListViewDragDropManager<ItemType> where ItemType : class, IDraggableUser
    {
        #region fields
        private bool _canInitiateDrag;
        private DragAdorner _dragAdorner;
        private double _dragAdornerOpacity;
        private int _indexToSelect;
        private bool _isDragInProgress;
        private ItemType _itemUnderDragCursor;
        private ListView _listView;
        private Point _ptMouseDown;
        private bool _showDragAdorner;
        #endregion

        #region ctrs
        public ListViewDragDropManager()
        {
            _canInitiateDrag = false;
            _dragAdornerOpacity = 0.55;
            _indexToSelect = -1;
            _showDragAdorner = true;
        }

        public ListViewDragDropManager(ListView listView) : this()
        {
            ListView = listView;
        }

        public ListViewDragDropManager(ListView listView, double dragAdornerOpacity) : this(listView)
        {
            DragAdornerOpacity = dragAdornerOpacity;
        }

        public ListViewDragDropManager(ListView listView, bool showDragAdorner) : this(listView)
        {
            ShowDragAdorner = showDragAdorner;
        }
        #endregion

        #region properties
        ItemType ItemUnderDragCursor
        {
            get { return _itemUnderDragCursor; }
            set
            {
                if (_itemUnderDragCursor == value)
                    return;
                for (int i = 0; i < 2; ++i)
                {
                    if (i == 1)
                        _itemUnderDragCursor = value;

                    if (_itemUnderDragCursor != null)
                    {
                        var listViewItem = GetListViewItem(_itemUnderDragCursor);
                        if (listViewItem != null)
                            ListViewItemDragState.SetIsUnderDragCursor(listViewItem, i == 1);
                    }
                }
            }
        }

        bool CanStartDragOperation
        {
            get
            {
                if (Mouse.LeftButton != MouseButtonState.Pressed)
                    return false;

                if (!_canInitiateDrag)
                    return false;

                if (_indexToSelect == -1)
                    return false;

                if (!HasCursorLeftDragThreshold)
                    return false;

                return true;
            }
        }

        public double DragAdornerOpacity
        {
            get { return _dragAdornerOpacity; }
            set
            {
                if (IsDragInProgress)
                    throw new InvalidOperationException("Cannot set the DragAdornerOpacity property during a drag operation.");

                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException("DragAdornerOpacity", value, "Must be between 0 and 1.");

                _dragAdornerOpacity = value;
            }
        }

        public bool IsDragInProgress
        {
            get { return _isDragInProgress; }
            private set { _isDragInProgress = value; }
        }

        public ListView ListView
        {
            get { return _listView; }
            set
            {
                if (IsDragInProgress)
                    throw new InvalidOperationException("Cannot set the ListView property during a drag operation.");

                if (_listView != null)
                {
                    _listView.PreviewMouseLeftButtonDown -= OnListViewPreviewMouseLeftButtonDown;
                    _listView.PreviewMouseMove -= OnListViewPreviewMouseMove;
                    _listView.DragOver -= OnListViewDragOver;
                    _listView.DragLeave -= OnListViewDragLeave;
                    _listView.DragEnter -= OnListViewDragEnter;
                    _listView.Drop -= OnListViewDrop;
                }

                _listView = value;
                if (_listView != null)
                {
                    if (!_listView.AllowDrop)
                        _listView.AllowDrop = true;
                    _listView.PreviewMouseLeftButtonDown += OnListViewPreviewMouseLeftButtonDown;
                    _listView.PreviewMouseMove += OnListViewPreviewMouseMove;
                    _listView.DragOver += OnListViewDragOver;
                    _listView.DragLeave += OnListViewDragLeave;
                    _listView.DragEnter += OnListViewDragEnter;
                    _listView.Drop += OnListViewDrop;
                }
            }
        }

        public bool ShowDragAdorner
        {
            get { return _showDragAdorner; }
            set
            {
                if (IsDragInProgress)
                    throw new InvalidOperationException("Cannot set the ShowDragAdorner property during a drag operation.");

                _showDragAdorner = value;
            }
        }

        bool HasCursorLeftDragThreshold
        {
            get
            {
                if (_indexToSelect < 0)
                    return false;

                var item = GetListViewItem(_indexToSelect);
                var bounds = VisualTreeHelper.GetDescendantBounds(item);
                var ptInItem = _listView.TranslatePoint(_ptMouseDown, item);
                var topOffset = Math.Abs(ptInItem.Y);
                var btmOffset = Math.Abs(bounds.Height - ptInItem.Y);
                var vertOffset = Math.Min(topOffset, btmOffset);

                var width = SystemParameters.MinimumHorizontalDragDistance * 2;
                var height = Math.Min(SystemParameters.MinimumVerticalDragDistance, vertOffset) * 2;
                var szThreshold = new Size(width, height);

                var rect = new Rect(_ptMouseDown, szThreshold);
                rect.Offset(szThreshold.Width / -2, szThreshold.Height / -2);
                var ptInListView = MouseUtilities.GetMousePosition(_listView);
                return !rect.Contains(ptInListView);
            }
        }

        int IndexUnderDragCursor
        {
            get
            {
                int index = -1;
                for (int i = 0; i < _listView.Items.Count; ++i)
                {
                    var item = GetListViewItem(i);
                    if (item == null)
                        continue;
                    if (IsMouseOver(item))
                    {
                        index = i;
                        break;
                    }
                }
                return index;
            }
        }

        bool IsMouseOverScrollbar
        {
            get
            {
                var ptMouse = MouseUtilities.GetMousePosition(_listView);
                var res = VisualTreeHelper.HitTest(_listView, ptMouse);
                if (res == null)
                    return false;
                var depObj = res.VisualHit;
                while (depObj != null)
                {
                    if (depObj is ScrollBar)
                        return true;
                    if (depObj is Visual || depObj is System.Windows.Media.Media3D.Visual3D)
                        depObj = VisualTreeHelper.GetParent(depObj);
                    else
                        depObj = LogicalTreeHelper.GetParent(depObj);
                }
                return false;
            }
        }

        bool ShowDragAdornerResolved
        {
            get { return ShowDragAdorner && DragAdornerOpacity > 0.0; }
        }
        #endregion 

        #region methods
        void OnListViewPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseOverScrollbar)
            {
                _canInitiateDrag = false;
                return;
            }

            int index = IndexUnderDragCursor;
            _canInitiateDrag = index > -1;

            if (_canInitiateDrag)
            {
                _ptMouseDown = MouseUtilities.GetMousePosition(_listView);
                _indexToSelect = index;
            }
            else
            {
                _ptMouseDown = new Point(-10000, -10000);
                _indexToSelect = -1;
            }
        }

        void OnListViewPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!CanStartDragOperation)
                return;

            if (_listView.SelectedIndex != _indexToSelect)
                _listView.SelectedIndex = _indexToSelect;


            if (_listView.SelectedItem == null)
                return;

            var itemToDrag = GetListViewItem(_listView.SelectedIndex);
            if (itemToDrag == null)
                return;

            var adornerLayer = ShowDragAdornerResolved ? InitializeAdornerLayer(itemToDrag) : null;

            InitializeDragOperation(itemToDrag);
            PerformDragOperation();
            FinishDragOperation(itemToDrag, adornerLayer);
        }

        void OnListViewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;

            if (ShowDragAdornerResolved)
                UpdateDragAdornerLocation();

            int index = IndexUnderDragCursor;
            ItemUnderDragCursor = index < 0 ? null : ListView.Items[index] as ItemType;
        }

        void OnListViewDragLeave(object sender, DragEventArgs e)
        {
            if (!IsMouseOver(_listView))
            {
                if (ItemUnderDragCursor != null)
                    ItemUnderDragCursor = null;

                if (_dragAdorner != null)
                    _dragAdorner.Visibility = Visibility.Collapsed;
            }
        }

        void OnListViewDragEnter(object sender, DragEventArgs e)
        {
            if (_dragAdorner != null && _dragAdorner.Visibility != Visibility.Visible)
            {
                UpdateDragAdornerLocation();
                _dragAdorner.Visibility = Visibility.Visible;
            }
        }

        void OnListViewDrop(object sender, DragEventArgs e)
        {
            if (ItemUnderDragCursor != null)
                ItemUnderDragCursor = null;

            e.Effects = DragDropEffects.None;

            if (!e.Data.GetDataPresent(typeof(ItemType)))
                return;

            var data = e.Data.GetData(typeof(ItemType)) as ItemType;
            if (data == null)
                return;

            var itemsSource = _listView.ItemsSource as ObservableCollection<ItemType>;
            if (itemsSource == null)
                throw new Exception("Wrong ItemsSource type.");

            data.Join(_listView.DataContext as ViewModelBase);
        }

        void FinishDragOperation(ListViewItem draggedItem, AdornerLayer adornerLayer)
        {
            ListViewItemDragState.SetIsBeingDragged(draggedItem, false);

            IsDragInProgress = false;

            if (ItemUnderDragCursor != null)
                ItemUnderDragCursor = null;

            if (adornerLayer != null)
            {
                adornerLayer.Remove(_dragAdorner);
                _dragAdorner = null;
            }
        }

        ListViewItem GetListViewItem(int index)
        {
            if (_listView.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return _listView.ItemContainerGenerator.ContainerFromIndex(index) as ListViewItem;
        }

        ListViewItem GetListViewItem(ItemType dataItem)
        {
            if (_listView.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;

            return _listView.ItemContainerGenerator.ContainerFromItem(dataItem) as ListViewItem;
        }

        AdornerLayer InitializeAdornerLayer(ListViewItem itemToDrag)
        {
            var brush = new VisualBrush(itemToDrag);
            _dragAdorner = new DragAdorner(_listView, itemToDrag.RenderSize, brush)
            {
                Opacity = DragAdornerOpacity
            };

            var layer = AdornerLayer.GetAdornerLayer(_listView);
            layer.Add(_dragAdorner);
            _ptMouseDown = MouseUtilities.GetMousePosition(_listView);
            return layer;
        }

        void InitializeDragOperation(ListViewItem itemToDrag)
        {
            IsDragInProgress = true;
            _canInitiateDrag = false;
            ListViewItemDragState.SetIsBeingDragged(itemToDrag, true);
        }

        bool IsMouseOver(FrameworkElement target)
        {
            var bounds = VisualTreeHelper.GetDescendantBounds(target);
            var mousePos = MouseUtilities.GetMousePosition(target);
            return bounds.Contains(mousePos);
        }

        void PerformDragOperation()
        {
            var selectedItem = _listView.SelectedItem as ItemType;
            var allowedEffects = DragDropEffects.Move | DragDropEffects.Move | DragDropEffects.Link;
            if (DragDrop.DoDragDrop(_listView, selectedItem, allowedEffects) != DragDropEffects.None)
                _listView.SelectedItem = selectedItem;
        }

        void UpdateDragAdornerLocation()
        {
            if (_dragAdorner != null)
            {
                var ptCursor = MouseUtilities.GetMousePosition(ListView);
                var left = ptCursor.X - _ptMouseDown.X;
                var itemBeingDragged = GetListViewItem(_indexToSelect);
                var itemLoc = itemBeingDragged.TranslatePoint(new Point(0, 0), ListView);
                var top = itemLoc.Y + ptCursor.Y - _ptMouseDown.Y;
                _dragAdorner.SetOffsets(left, top);
            }
        }
        #endregion
    }
}
