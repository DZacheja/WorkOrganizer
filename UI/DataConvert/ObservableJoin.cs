using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
namespace WorkOrganizer.UI.DataConvert {
    public class ObservableJoin: IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                ObservableCollection<string> RemoveTextValues = (ObservableCollection<string>)value;
                return string.Join(",", RemoveTextValues.Select(s => s));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                string[] items = value.ToString().Trim().Split(new[] { ',', ' ' });
                ObservableCollection<string> observableItems = new ObservableCollection<string>(items);
                return observableItems;
            }
            return null;
        }
    }
}
