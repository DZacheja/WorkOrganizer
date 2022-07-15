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
    public class AnySubitemsForTask : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            List<Subtask> e = (List<Subtask>)value;
            if (e.Count == 0) {
                return Visibility.Visible;
            } else {
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }

    }
}
