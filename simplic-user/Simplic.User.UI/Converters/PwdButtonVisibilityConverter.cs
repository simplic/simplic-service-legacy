using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Simplic.User.UI
{
    class PwdButtonVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)values[0])
                return Visibility.Visible;
            return (bool)values[1] ? Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
