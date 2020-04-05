using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace Simplic.User.UI
{
    public partial class StringComparisonFilterControl : IFilteringControl
    {
        #region fields
        private GridViewBoundColumnBase _column;
        private CompositeFilterDescriptor _compositeFilter;
        private FilterDescriptor _filter;
        #endregion

        #region ctr
        public StringComparisonFilterControl()
        {
            InitializeComponent();
        }
        #endregion

        #region methods
        public void Prepare(GridViewColumn columnToPrepare)
        {
            _column = columnToPrepare as GridViewBoundColumnBase;
            if (_column == null)
                return;
            if (_compositeFilter == null)
                CreateFilters();
            _filter.Value = FilteredText;
        }

        private void CreateFilters()
        {
            string dataMember = _column.DataMemberBinding.Path.Path;

            _compositeFilter = new CompositeFilterDescriptor();

            _filter = new FilterDescriptor(dataMember, FilterOperator.Contains, null);
            _compositeFilter.FilterDescriptors.Add(_filter);
        }

        private void OnFilter(object sender, RoutedEventArgs e)
        {
            _filter.Value = FilteredText;

            if (!_column.DataControl.FilterDescriptors.Contains(_compositeFilter))
                _column.DataControl.FilterDescriptors.Add(_compositeFilter);
            IsActive = true;
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            if (_column.DataControl.FilterDescriptors.Contains(_compositeFilter))
                _column.DataControl.FilterDescriptors.Remove(_compositeFilter);
            FilteredText = string.Empty;
            IsActive = false;
        }
        #endregion

        #region properties
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(StringComparisonFilterControl), new PropertyMetadata(false));

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(StringComparisonFilterControl));

        public string FilteredText
        {
            get { return (string)GetValue(FilteredTextProperty); }
            set { SetValue(FilteredTextProperty, value); }
        }
        public static readonly DependencyProperty FilteredTextProperty = DependencyProperty.Register("FilteredText", typeof(string), typeof(StringComparisonFilterControl));
        #endregion
    }
}
