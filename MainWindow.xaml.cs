using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WorkOrganizer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        static RadioButton r;
        public MainWindow() {
            InitializeComponent();
            this.btnTasksViewShow.Visibility = Visibility.Collapsed;
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            var appsettings = ConfigurationManager.AppSettings;
            string res = appsettings["Login"];
            System.Diagnostics.Debug.WriteLine("Config file: " + res);
            settings["Login"].Value = "Zmienione!";
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
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
