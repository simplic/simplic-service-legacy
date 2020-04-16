using System;
using System.Globalization;
using System.Windows.Data;

namespace Simplic.User.UI
{
    class UserToTitleConverter : IValueConverter
    {
        public string NullString { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is UserViewModel user) ? NullString : user.UserName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
