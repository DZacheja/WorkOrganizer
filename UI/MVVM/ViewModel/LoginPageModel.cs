using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public class LoginPageModel : ObservableObject {
        public Action _hideButtonMethod;
        public LoginPageModel(Action hideButton) {
            _hideButtonMethod = hideButton;
            System.Diagnostics.Debug.WriteLine("Not empty constructor");
        }
        public LoginPageModel() {
            System.Diagnostics.Debug.WriteLine("Emptty constructor");
        }

        private string _login;
        public string Login {
            get { return _login; }
            set {
                _login = value;
                OnPropertyChange();
                System.Diagnostics.Debug.WriteLine("Zmieniono login na: " + _login);
            }
        }
        private ICommand _logIntoDatabase;

        public ICommand LogIntoDatabase {
            get {
                if (_logIntoDatabase == null) {
                    _logIntoDatabase = new RelayCommand(
                        param => this.LogInto(),
                        param => this.CanLogInto()
                        );
                }
                return _logIntoDatabase;
            }
        }

        private void LogInto() {
            System.Diagnostics.Debug.WriteLine("Można logować");
            MainWindow.HideLogin();
        }

        private bool CanLogInto() {
            if (_login != null) {
                return true;
            } else {
                System.Diagnostics.Debug.WriteLine("Pusty login");
                return false;
            }
        }
    }
}
