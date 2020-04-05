using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Simplic.User.UI
{
    class BoolToVisibilityConverter : IValueConverter
    {
        public Visibility VisibilityForTrue { get; set; }
        public Visibility VisibilityForFalse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? VisibilityForTrue : VisibilityForFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
