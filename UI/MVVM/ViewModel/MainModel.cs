using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public class MainModel:ObservableObject {
        public RelayCommand loginPageCommand { get; set; }

        public LoginPageModel loginPageMV { get; set; }

        private object _currentView;
        public object CurrentView {
            get { return _currentView; }
            set {
                _currentView = value;
                OnPropertyChange();
            }
        }

        public MainModel() {
            loginPageMV = new LoginPageModel();
            loginPageCommand = new RelayCommand(o => {
                CurrentView = loginPageMV;
            });
        }
    }
}
