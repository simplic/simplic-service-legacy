using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Simplic.User.UI
{
    class OrganizationsToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var organizations = value as ObservableCollection<OrganizationViewModel>;
            if (!organizations.Any())
                return "None";
            var sb = new SeperatedStringBuilder(", ");
            foreach (var o in organizations)
                sb.Append(o.Name);
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
