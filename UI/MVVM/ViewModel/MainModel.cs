using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WorkOrganizer.UI.MVVM.View;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public class MainModel:ObservableObject {
        public RelayCommand loginPageCommand { get; set; }
        public RelayCommand taskCommand { get; set; }

        public LoginPageModel loginPageMV { get; set; }
        public TaskViewModel taskMV { get; set; }

        private object _currentView;
        public object CurrentView {
            get { return _currentView; }
            set {
                _currentView = value;
                OnPropertyChange();
            }
        }

        public MainModel() {
            loginPageMV = new LoginPageModel(HideLoginButton);
            taskMV = new TaskViewModel();
            loginPageCommand = new RelayCommand(o => {
                CurrentView = loginPageMV;
            });
            taskCommand = new RelayCommand(o => {
                CurrentView = taskMV;
            });
        }

        public void HideLoginButton() {
            CurrentView = null;
        }
    }
}
