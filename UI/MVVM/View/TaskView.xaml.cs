using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WorkOrganizer.UI.MVVM.ViewModel;

namespace WorkOrganizer.UI.MVVM.View {
    /// <summary>
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : UserControl {
        private TaskViewModel viewModel;
        public TaskView() {
            viewModel = TaskViewModel.GetInstance();
            this.DataContext = viewModel;
            InitializeComponent();
            ((INotifyCollectionChanged)tasksList.Items).CollectionChanged += ListView_CollectionChanged;
            if (tasksList.Items.Count > 0) {
                var index = tasksList.Items.Count - 1;
                var item = tasksList.Items.GetItemAt(index);
                tasksList.ScrollIntoView(item);
            }
        }
        private void ListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                // scroll the new item into view   
                tasksList.ScrollIntoView(e.NewItems[0]);
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            viewModel.AddNewTaskVisibleButtonClicked = !viewModel.AddNewTaskVisibleButtonClicked;
            if (tasksList.Items.Count > 2) {
                tasksList.ScrollIntoView(tasksList.Items[tasksList.Items.Count - 2]);
            }
        }


        private async void AddNewTask_Click(object sender, RoutedEventArgs e) {
            try {
                await viewModel.InsertingNewTask(dateNewTaskDeadline.SelectedDate);
                await showLblInfo("Pomyślnie dodano zadanie!", "#00cc00");
            } catch (Exception ex) {
                await showLblInfo(ex.Message, "#DC143C");
            }
        }

        private void AddNewSubtask(object sender, RoutedEventArgs e) {
            viewModel.AddSubtaskToList();
        }

        private async Task showLblInfo(string txt, string color) {
            _ = Task.Run(() => {
                this.lblInfo.Dispatcher.Invoke(new Action(async () => {
                    this.lblInfo.Text = txt;
                    var bc = new BrushConverter();
                    this.lblInfo.Foreground = (Brush)bc.ConvertFrom(color);
                    this.lblInfo.Visibility = System.Windows.Visibility.Visible;
                    await Task.Delay(3000);
                    this.lblInfo.Visibility = System.Windows.Visibility.Collapsed;
                }));
            });

        }

        private void tasksList_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListBoxItem)) {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;

            ListBoxItem item = (ListBoxItem)dep;

            if (item.IsSelected) {
                item.IsSelected = !item.IsSelected;
                e.Handled = true;
            }
        }

        private void ListSubitems_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Delete) {
                if (ListSubitems.SelectedItems.Count > 0) {
                    var s = ListSubitems.SelectedItem;
                    ((ObservableCollection<string>)ListSubitems.ItemsSource).Remove((string)s);
                }
            }
        }
    }
}
