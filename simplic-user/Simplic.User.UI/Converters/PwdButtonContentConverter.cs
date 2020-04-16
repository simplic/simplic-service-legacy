using System;
using System.Globalization;
using System.Windows.Data;

namespace Simplic.User.UI
{
    class PwdButtonContentConverter : IValueConverter
    {
        public string FalseString { get; set; }
        public string TrueString { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? TrueString : FalseString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
