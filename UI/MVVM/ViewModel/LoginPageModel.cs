using DatabaseConnection;
using DatabaseConnection.Entities;
using ModernDesign.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WorkOrganizer.UI.Core;

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
            set {
                _password = value;
                OnPropertyChange();
            }
        }


        public async Task CreateNewAccount() {
            if (_login != null && _password != null && _mail != null && _userName != null) {
                if (_login != "" && _password != "" && _mail != "" && _userName != "") {
                    var dbContext = new WorkOrganizerContext();
                    var decodePassword = AesOperation.EncryptString(ProgramSettings.key, _password);
                    var chk = dbContext.Users.FirstOrDefault(x => x.UserLogin == _login);
                    if (chk == null) {
                        User newUser = new User() {
                            UserLogin = _login,
                            Name = _userName,
                            Mail = _mail,
                            Password = decodePassword
                        };
                        dbContext.Add(newUser);
                        var res = await dbContext.SaveChangesAsync();
                        if (res == 0) {
                            throw new Exception("Błąd podczas dodawania nowego użytkownika!");
                        }
                    } else {
                        throw new Exception("Podana nazwa użytkownika już istnieje!");
                    }
                } else {
                    throw new Exception("Uzupełnij wszystkie dane");
                }
            } else {
                throw new Exception("Uzupełnij wszystkie dane");
            }
        }

        public async Task LogIntoDatabase() {
            if (_login != null && _password != null) {
                var dbContext = new WorkOrganizerContext();
                var encodePassword = AesOperation.EncryptString(ProgramSettings.key, _password);
                using (dbContext) {
                    var usr = dbContext.Users.FirstOrDefault(x => x.UserLogin == _login && x.Password == encodePassword);
                    if (usr != null) {
                        ProgramSettings.currentUser = usr;
                        var serial = JsonConvert.SerializeObject(usr);
                        serial = AesOperation.EncryptString(ProgramSettings.key, serial);
                        File.WriteAllText(ProgramSettings.LoggedUserClassFile, serial);
                        System.Diagnostics.Debug.WriteLine(Directory.GetParent(ProgramSettings.LoggedUserClassFile).FullName);
                        MainModel mwDbContext = (MainModel)Application.Current.MainWindow.DataContext;
                        mwDbContext.HideLoginButton();
                    } else {
                        throw new Exception("Błędny login lub hasło!");
                    }
                }
            } else {
                throw new Exception("Uzupełnij wszystkie dane");
            }

        }
    }
}
