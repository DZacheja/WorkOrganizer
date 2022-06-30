using DatabaseConnection;
using DatabaseConnection.Entities;
using Microsoft.EntityFrameworkCore;
using ModernDesign.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public class MainModel:ObservableObject {

        public WorkOrganizerContext dbContext;
        public RelayCommand loginPageCommand { get; set; }
        public RelayCommand taskCommand { get; set; }

        public LoginPageModel loginPageMV { get; set; }
        public TaskViewModel taskMV { get; set; }


        // Principal Selection
        public ObservableCollection<Principal> Principals { get; set; }
        private Principal? _selectedPrincipal;

        public Principal SelectedPrincipal {
            get { return _selectedPrincipal; }
            set { _selectedPrincipal = value;
                OnPropertyChange();

                if (_selectedPrincipal != null)
                    FillPrincipalItems();
                else
                    PrincipalsWork.Clear();
            }
        }

        // work selection
        public ObservableCollection<Work> PrincipalsWork { get; set; }
        private Work? _selectedPrincipalWork;

        public Work SelectedPrincipalWork {
            get { return _selectedPrincipalWork; }
            set {
                _selectedPrincipalWork = value;
                OnPropertyChange();

                if (_selectedPrincipalWork != null)
                    FillWorkTypes();
                else
                    WorkTypes.Clear();
            }
        }

        private async void FillPrincipalItems() {
            dbContext = new WorkOrganizerContext();
            PrincipalsWork.Clear();
            using (dbContext) {
                var Works = await dbContext.Works.Where(x => x.PrincipalsId == _selectedPrincipal.PrincipalID).ToListAsync();
                foreach (var task in Works) {
                    PrincipalsWork.Add(task);
                }

                if (PrincipalsWork.Count > 0)
                    SelectedPrincipalWork = PrincipalsWork[0];
            }
        }

        // Work Type selection
        public ObservableCollection<WorkType> WorkTypes { get; set; }
        private WorkType? _selectedWorkType;

        public WorkType SelectedWorkType {
            get { return _selectedWorkType; }
            set {
                _selectedWorkType = value;
                OnPropertyChange();
                if(SelectedWorkType != null && SelectedPrincipalWork != null && SelectedPrincipal != null)
                    taskMV.FilterByWorkAndWorkType(SelectedWorkType, SelectedPrincipalWork, SelectedPrincipal);
            }
        }

        private async void FillWorkTypes() {
            dbContext = new WorkOrganizerContext();
            WorkTypes.Clear();
            using (dbContext) {
                var Worktypes = await dbContext.WorkComponents
                    .Where(x => x.WorkId == SelectedPrincipalWork.WorkId)
                    .Include(x => x.WorkTypes)
                    .Select(o => o.WorkTypes)
                    .ToListAsync();

                foreach (var task in Worktypes) {
                    WorkTypes.Add(task);
                }

                if (WorkTypes.Count > 0)
                    SelectedWorkType = WorkTypes[0];
            }
        }


        private object _currentView;
        public object CurrentView {
            get { return _currentView; }
            set {
                _currentView = value;
                OnPropertyChange();
            }
        }

        public MainModel() {
            dbContext = new WorkOrganizerContext();

            loginPageMV = new LoginPageModel();
            taskMV = new TaskViewModel();
            loginPageCommand = new RelayCommand(o => {
                CurrentView = loginPageMV;
            });
            taskCommand = new RelayCommand(o => {
                CurrentView = taskMV;
            });
            
            PrincipalsWork = new ObservableCollection<Work>();
            PrincipalsWork.Add(new Work { Name = "-" });
            
            WorkTypes = new ObservableCollection<WorkType>();

            using (dbContext) {
                var dbList = dbContext.Principals.ToList() ;
                Principals = new ObservableCollection<Principal>();
                Principals.Add(new Principal { Name = "-" });
                foreach (var task in dbList) {
                    Principals.Add(task);
                }

                if(Principals.Count > 0)
                    SelectedPrincipal = Principals[0];
            }

        }

        public void HideLoginButton() {
            CurrentView = null;
        }
    }
}
