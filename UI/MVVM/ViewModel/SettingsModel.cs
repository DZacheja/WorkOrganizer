using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrganizer.UI.Core;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public class SettingsModel : ObservableObject {
        private static SettingsModel _instance;
        public static SettingsModel Instance { get { return _instance ?? (_instance = new SettingsModel()); } }
        
        public RelayCommand logOutCommand { get; set; }
        private SettingsModel() {
            logOutCommand = new RelayCommand(x => {
                Logout();
            });
        }

        private void Logout() {
            File.WriteAllText(ProgramSettings.LoggedUserClassFile, null);
            MainModel mw = MainModel.Instance;
            mw.ShowLoginButton();
            
        }
    }
}
