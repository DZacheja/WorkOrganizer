using DatabaseConnection.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WorkOrganizer.UI.DataConvert {
    [ValueConversion(typeof(List<Subtask>), typeof(Visibility))]

    public class AnySubitemsUnchecked : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            List<Subtask> list = (List<Subtask>)value;
            var aby = list.FirstOrDefault(x => x.Status == false);

            if (aby != null) {
                return Visibility.Collapsed;
            } else {
                return Visibility.Visible;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }

    }
}
