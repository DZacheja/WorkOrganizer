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
    public sealed class LoginPageModel : ObservableObject {
        private static LoginPageModel _instance;

        public static LoginPageModel GetInstance() {
            if (_instance == null) {
                _instance = new LoginPageModel();
            }
            return _instance;
        }

        private string _login;
        public string Login {
            get { return _login; }
            set {
                _login = value;
                OnPropertyChange();
            }
        }


        private string _mail;
        public string Mail {
            get { return _mail; }
            set {
                _mail = value;
                OnPropertyChange();
            }
        }

        private string _userName;
        public string UserName {
            get { return _userName; }
            set {
                _userName = value;
                OnPropertyChange();
            }
        }

        private string _password;

        public string Password {
            get { return _password; }
            set { _password = value;
                OnPropertyChange();
            }
        }


        private async void LogInto() {
            if (_login != null && _password != null && _mail != null && _userName != null) {

            } else {
                throw new Exception("Empty login or password");
            }
        }

        public async void LogIntoDatabase() {
            if(_login != null && _password != null) {

            } else {
                throw new Exception("Empty login or password");
            }
        }

    }
}
