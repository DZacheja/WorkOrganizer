using DatabaseConnection.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WorkOrganizer.DatabaseConnections.Entities;
using WorkOrganizer.UI.Core;
using WorkOrganizer.UI.MVVM.ViewModel;

namespace WorkOrganizer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            try {
                string cryptoUser = File.ReadAllText(ProgramSettings.LoggedUserClassFile);

                var decryptUser = AesOperation.DecryptString(ProgramSettings.key, cryptoUser);
                User  us = JsonConvert.DeserializeObject<User>(decryptUser);
                if(us != null) {
                    ProgramSettings.currentUser = us;
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message); 
            }
        }
       
        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                DragMove();
            }
        }

        private void Minimalize_Button_Click(object sender, RoutedEventArgs e) {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Maximalize_Button_Click(object sender, RoutedEventArgs e) {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else
                Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void CloseApplication_Button_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void UnselectListViewItem(object sender, MouseButtonEventArgs e) {
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

        private void RemoveFavoriteItem(object sender, KeyEventArgs e) {
            if (e.Key == Key.Delete) {
                if (lstFilter.SelectedItems.Count > 0) {
                    MainModel mm = (MainModel)this.DataContext;
                    mm.RemoveFilter();
                }
            }
        }

    }
}
