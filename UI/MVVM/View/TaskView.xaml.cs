using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WorkOrganizer.UI.MVVM.ViewModel;

namespace WorkOrganizer.UI.MVVM.View {
    /// <summary>
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : UserControl {
        public TaskView() {
            TaskViewModel taskViewModel = TaskViewModel.GetInstance();
            this.DataContext = taskViewModel;
            InitializeComponent();
            ((INotifyCollectionChanged)tasksList.Items).CollectionChanged += ListView_CollectionChanged;
            System.Diagnostics.Debug.WriteLine("Cosntructor in tak view   ---> " + tasksList.Items.Count);
            var index = tasksList.Items.Count - 1;
            var item = tasksList.Items.GetItemAt(index);
            tasksList.ScrollIntoView(item);
        }
        private void ListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                // scroll the new item into view   
                tasksList.ScrollIntoView(e.NewItems[0]);
            }
        }
    }
}
