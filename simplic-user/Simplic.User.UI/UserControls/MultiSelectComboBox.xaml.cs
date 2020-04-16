using Simplic.UI.MVC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Simplic.User.UI
{
    public partial class MultiSelectComboBox 
    {
        private readonly List<string> _selectedItems = new List<string>();

        public MultiSelectComboBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty EditCommandProperty = DependencyProperty.Register("EditCommand", typeof(ICommand), typeof(MultiSelectComboBox));
        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty = DependencyProperty.Register("SelectionChangedCommand", typeof(ICommand), typeof(MultiSelectComboBox));
        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        private static readonly DependencyProperty CurrentUserProperty = DependencyProperty.Register("CurrentUser", typeof(UserViewModel), typeof(MultiSelectComboBox));
        public UserViewModel CurrentUser
        {
            get { return (UserViewModel)GetValue(CurrentUserProperty); }
            set { SetValue(CurrentUserProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<ViewModelBase>), typeof(MultiSelectComboBox));
        public IEnumerable<ViewModelBase> ItemsSource
        {
            get { return (IEnumerable<ViewModelBase>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty CurrentUserSubItemsProperty = DependencyProperty.Register("CurrentUserSubItems", typeof(IEnumerable<INamedEntity>), typeof(MultiSelectComboBox));
        public IEnumerable<INamedEntity> CurrentUserSubItems
        {
            get { return (IEnumerable<INamedEntity>)GetValue(CurrentUserSubItemsProperty); }
            set { SetValue(CurrentUserSubItemsProperty, value); }
        }

        private static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox));
        private string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty NoneCheckedTextProperty = DependencyProperty.Register("NoneCheckedText", typeof(string), typeof(MultiSelectComboBox));
        public string NoneCheckedText
        {
            get { return (string)GetValue(NoneCheckedTextProperty); }
            set { SetValue(NoneCheckedTextProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _selectedItems.AddRange(CurrentUserSubItems.Where(g => ItemsSource.OfType<INamedEntity>().Contains(g)).Select(g => g.Name));
            if (!_selectedItems.Any())
                Text = NoneCheckedText;
            else
            {
                var sb = new SeperatedStringBuilder(", ");
                foreach (var str in _selectedItems)
                    sb.Append(str);
                Text = sb.ToString();
            }
        }

        private void OnCheckBoxClick(object sender, RoutedEventArgs e)
        {
            var clickedBox = (CheckBox)sender;
            var content = clickedBox.Content.ToString();
            SelectionChangedCommand?.Execute(clickedBox.DataContext);
            if (clickedBox.IsChecked.HasValue && clickedBox.IsChecked.Value)
                _selectedItems.Add(content);
            else if (clickedBox.IsChecked.HasValue && !clickedBox.IsChecked.Value)
            {
                _selectedItems.Remove(content);
                if (!_selectedItems.Any())
                {
                    Text = NoneCheckedText;
                    return;
                }
            }
            var sb = new SeperatedStringBuilder(", ");
            foreach (var str in _selectedItems)
                sb.Append(str);
            Text = sb.ToString();
        }

        private void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (EditCommand != null)
                EditCommand.Execute((sender as Button).Tag);
        }
    }
}
