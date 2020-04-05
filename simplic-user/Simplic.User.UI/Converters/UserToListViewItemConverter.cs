using System;
using System.Globalization;
using System.Windows.Data;

namespace Simplic.User.UI
{
    class UserToListViewItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var firstName = (string)values[0];
            var lastName = (string)values[1];
            var userName = (string)values[2];
            var email = (string)values[3];

            var fullName = string.IsNullOrEmpty(firstName) ? string.Empty : firstName;
            fullName += string.IsNullOrEmpty(lastName) ? string.Empty : " " + lastName;
            var sb = new SeperatedStringBuilder("; ");
            if (!string.IsNullOrEmpty(fullName))
                sb.Append(fullName);
            if (!string.IsNullOrEmpty(userName))
                sb.Append(userName);
            if (!string.IsNullOrEmpty(email))
                sb.Append(email);
            return sb.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
