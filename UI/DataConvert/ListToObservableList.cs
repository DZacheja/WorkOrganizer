using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DatabaseConnection.Entities;


namespace WorkOrganizer.UI.DataConvert {
    [ValueConversion(typeof(List<Subtask>), typeof(ObservableCollection<Subtask>))]
    public class ListToObservableList : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            ObservableCollection<Subtask> newCollection = new ObservableCollection<Subtask>();
            List<Subtask> list = (List<Subtask>)value;
            foreach (Subtask item in list) {
                newCollection.Add(item);
            }
            return newCollection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            ObservableCollection<Subtask> newCollection = (ObservableCollection<Subtask>)value;
            return newCollection.ToList();
        }

    }
}
