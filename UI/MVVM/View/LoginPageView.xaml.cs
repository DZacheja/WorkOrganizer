using System;
using System.Windows.Controls;

namespace WorkOrganizer.UI.MVVM.View {
    /// <summary>
    /// Interaction logic for LoginPageView.xaml
    /// </summary>
    public partial class LoginPageView : UserControl {
        public LoginPageView() {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("LoginViewCreated!");
        }
    }
}
