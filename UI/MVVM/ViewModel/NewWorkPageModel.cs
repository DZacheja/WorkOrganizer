using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public class NewWorkPageModel : ObservableObject {
        private static NewWorkPageModel _instance;
        public static NewWorkPageModel Instance() {
            if (_instance == null) {
                _instance = new NewWorkPageModel();
            }
            return _instance;
        }

        private NewWorkPageModel() {
            NewPrincipalTextBoxVisibility = Visibility.Collapsed;

            showNewPrincipalCommand = new RelayCommand(o => {
                if (NewPrincipalTextBoxVisibility == Visibility.Visible) {
                    NewPrincipalTextBoxVisibility = Visibility.Collapsed;
                    CboPrincipalVisibility = Visibility.Visible;
                } else {
                    NewPrincipalTextBoxVisibility = Visibility.Visible;
                    CboPrincipalVisibility = Visibility.Collapsed;
                }
            });
        }

        /// <summary>
        /// Principal textBox Visibility
        /// </summary>
        private Visibility _newPrincipalTextBoxVisibility;

        public Visibility NewPrincipalTextBoxVisibility {
            get { return _newPrincipalTextBoxVisibility; }
            set {
                _newPrincipalTextBoxVisibility = value;
                OnPropertyChange();
            }
        }
        private Visibility _cboPrincipalVisibility;

        public Visibility CboPrincipalVisibility {
            get { return _cboPrincipalVisibility; }
            set { _cboPrincipalVisibility = value; }
        }

        public RelayCommand showNewPrincipalCommand;

    }
}
