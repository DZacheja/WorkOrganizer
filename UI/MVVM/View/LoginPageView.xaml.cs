using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using WorkOrganizer.UI.MVVM.ViewModel;

namespace WorkOrganizer.UI.MVVM.View {
    /// <summary>
    /// Interaction logic for LoginPageView.xaml
    /// </summary>
    public partial class LoginPageView : UserControl {
        private LoginPageModel loginPageM;
        public LoginPageView() {
            loginPageM = LoginPageModel.GetInstance();
            this.DataContext = loginPageM;
            InitializeComponent();
            this.borderNewAccount.Visibility = System.Windows.Visibility.Collapsed;

        }

        private void txtPassword_PasswordChanged(object sender, System.Windows.RoutedEventArgs e) {
            if(this.DataContext != null) {
                PasswordBox pb = (PasswordBox)sender;
                loginPageM.Password = pb.Password;
            }
        }

        private void LogInButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            try {
                loginPageM.LogIntoDatabase();
            } catch (Exception ex) {
                showLblInfo(ex.Message, "#DC143C");
            }
        }

        private void NewAccount_Click(object sender, System.Windows.RoutedEventArgs e) {
            if(this.borderNewAccount.Visibility == System.Windows.Visibility.Collapsed) {
                this.borderNewAccount.Visibility = System.Windows.Visibility.Visible;
                this.BorderLogin.Visibility = System.Windows.Visibility.Collapsed;
                this.btnNewAccount.Tag = "Anuluj";
            } else {
                this.BorderLogin.Visibility = System.Windows.Visibility.Visible;
                this.borderNewAccount.Visibility = System.Windows.Visibility.Collapsed;
                this.btnNewAccount.Tag = "Utwórz nowe konto";

            }
        }

        private void CreateNewAccount_Click(object sender, System.Windows.RoutedEventArgs e) {
            this.BorderLogin.Visibility = System.Windows.Visibility.Collapsed;
        }
        private async Task showLblInfo(string txt, string color) {
            Task.Run(() => {
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
    }
}
