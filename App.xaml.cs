using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WorkOrganizer.UI.Core;
using WorkOrganizer.UI.MVVM.ViewModel;

namespace WorkOrganizer {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            Mediator mediator = new Mediator();
            LoginPageModel loginModel = new LoginPageModel();
            MainModel mainModel = new MainModel();
            MainWindow = new MainWindow() {
                DataContext = mainModel
            };

            MainWindow.Show();
            base.OnStartup(e);
        }
    }
}
