using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Simplic.User.UI
{
    public partial class UserManagmentEditorView
    {
        public static readonly DependencyProperty IsGroupsSelectedProperty = DependencyProperty.Register("IsGroupsSelected", typeof(bool), typeof(UserManagmentEditorView));
        public bool IsGroupsSelected
        {
            get { return (bool)GetValue(IsGroupsSelectedProperty); }
            set { SetValue(IsGroupsSelectedProperty, value); }
        }

        public UserManagmentEditorView()
        {
            InitializeComponent();
            roleGridView.LoadConfiguration("GRID_Role");
        }

        private void ExpandOrCollapse(DependencyObject control, bool isExpanded)
        {
            var children = FindInVisualTreeDown(control, typeof(Expander))?.ToList();
            if (children != null || children.Any())
                foreach (var exp in children.OfType<Expander>())
                    exp.IsExpanded = isExpanded;
        }

        private void OnAllOrganizationsExpand(object sender, RoutedEventArgs e)
        {
            ExpandOrCollapse(_lstOrganizations, true);
        }

        private static IEnumerable<DependencyObject> FindInVisualTreeDown(DependencyObject obj, Type type)
        {
            if (obj != null)
            {
                if (obj.GetType() == type)
                    yield return obj;
                
                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    foreach (var child in FindInVisualTreeDown(VisualTreeHelper.GetChild(obj, i), type))
                    {
                        if (child != null)
                            yield return child;
                    }
                }
            }
            yield break;
        }

        private void OnAllOrganizationsCollapse(object sender, RoutedEventArgs e)
        {
            ExpandOrCollapse(_lstOrganizations, false);
        }

        private void OnAllGroupsCollapse(object sender, RoutedEventArgs e)
        {
            ExpandOrCollapse(_lstGroups, false);
        }

        private void OnAllGroupsExpand(object sender, RoutedEventArgs e)
        {
            ExpandOrCollapse(_lstGroups, true);
        }
    }
}
