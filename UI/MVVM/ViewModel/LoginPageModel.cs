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

        private async void LogInto() {
            System.Diagnostics.Debug.WriteLine("Można logować");
            var DatabaseContext = new WorkOrganizerContext();
            var newUser = new User() {
                Name = "Dammian",
                Mail = "d.zacheja@bg-p.pl"
            };
            using (DatabaseContext) {
                await DatabaseContext.Users.AddAsync(newUser);
                if (await DatabaseContext.SaveChangesAsync() == 1) {
                    ((MainWindow)Application.Current.MainWindow).TestRadioButton.Content = "Zmienono!";
                } else {
                    ((MainWindow)Application.Current.MainWindow).TestRadioButton.Content = "Nie Zmienono!";
                }
            }
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
