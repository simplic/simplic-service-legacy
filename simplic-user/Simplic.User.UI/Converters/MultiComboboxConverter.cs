using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Simplic.User.UI
{
    class MultiComboboxConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var users = values[0] as ObservableCollection<UserViewModel>;
            var user = values[1] as UserViewModel;
            return users.Contains(user);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
