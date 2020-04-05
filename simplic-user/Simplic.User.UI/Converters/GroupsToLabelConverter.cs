using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Simplic.User.UI
{
    class GroupsToLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var groups = value as ObservableCollection<GroupViewModel>;
            if (!groups.Any())
                return "None";
            var sb = new SeperatedStringBuilder(", ");
            foreach (var g in groups)
                sb.Append(g.Name);
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
