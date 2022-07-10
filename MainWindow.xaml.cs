using DatabaseConnection.Entities;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WorkOrganizer.UI.Core;

namespace WorkOrganizer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        static RadioButton r;
        public MainWindow() {
            InitializeComponent();
            this.btnTasksViewShow.Visibility = Visibility.Collapsed;
            try {
                string cryptoUser = File.ReadAllText("ProgramSettings.txt");
                var decryptUser = AesOperation.DecryptString(ProgramSettings.key, cryptoUser);
                User  us = JsonConvert.DeserializeObject<User>(decryptUser);
                if(us != null) {
                    ProgramSettings.currentUser = us;
                    this.btnLoginViewShow.Visibility = Visibility.Collapsed;
                    this.btnTasksViewShow.Visibility = Visibility.Visible;
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

    }
}
