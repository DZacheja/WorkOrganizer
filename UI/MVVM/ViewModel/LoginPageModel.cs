using DatabaseConnection;
using DatabaseConnection.Entities;
using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public class LoginPageModel : ObservableObject {
        public Action _hideButtonMethod;       

        private string _login;
        public string Login {
            get { return _login; }
            set {
                _login = value;
                OnPropertyChange();
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

        private async void LogInto() {
            
        }

        private bool CanLogInto() {
            if (_login != null) {
                return true;
            } else {
                return false;
            }
        }

    }
}
