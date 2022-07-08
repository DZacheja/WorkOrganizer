﻿using DatabaseConnection;
using DatabaseConnection.Entities;
using Microsoft.EntityFrameworkCore;
using ModernDesign.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WorkOrganizer.UI.Core;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public class MainModel : ObservableObject {

        public WorkOrganizerContext dbContext;
        public RelayCommand loginPageCommand { get; set; }
        public RelayCommand taskCommand { get; set; }
        public RelayCommand filterTaskView { get; set; }

        public LoginPageModel loginPageMV { get; set; }
        public TaskViewModel taskMV { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainModel() {
            dbContext = new WorkOrganizerContext();

            // ...  Login Page ...  //
            loginPageMV = new LoginPageModel();
            loginPageCommand = new RelayCommand(o => {
                CurrentView = loginPageMV;
            });

            // ...  Task View ...  //
            taskMV = TaskViewModel.GetInstance();
            taskCommand = new RelayCommand(o => {
                CurrentView = taskMV;
            });

            // ...  Fill Comboboxes ...  //

            filterTaskView = new RelayCommand(o => this.CreateFilterForTaskView(),
                o => true);

            PrincipalsWork = new ObservableCollection<Work>();
            PrincipalsWork.Add(new Work { Name = "-" });

            WorkTypes = new ObservableCollection<WorkType>();

            using (dbContext) {
                var dbList = dbContext.Principals.ToList();
                Principals = new ObservableCollection<Principal>();
                Principals.Add(new Principal { Name = "-" });
                foreach (var task in dbList) {
                    Principals.Add(task);
                }

                if (Principals.Count > 0)
                    SelectedPrincipal = Principals[0];
            }

        }

        /// <summary>
        /// Principal filters combobox fill
        /// </summary>
        public ObservableCollection<Principal> Principals { get; set; }
        private Principal? _selectedPrincipal;

        public Principal SelectedPrincipal {
            get { return _selectedPrincipal; }
            set {
                _selectedPrincipal = value;
                OnPropertyChange();

                if (_selectedPrincipal != null)
                    FillPrincipalsWork();
                else
                    PrincipalsWork.Clear();

            }
        }
        private async void FillPrincipalsWork() {
            dbContext = new WorkOrganizerContext();
            PrincipalsWork.Clear();
            {
                using (dbContext) {
                    var Works = await dbContext.Works.Where(x => x.PrincipalsId == _selectedPrincipal.PrincipalID).ToListAsync();
                    foreach (var task in Works) {
                        PrincipalsWork.Add(task);
                    }

                    if (PrincipalsWork.Count > 0) {
                        PrincipalsWork.Insert(0, new Work() { Name = "-" });
                        SelectedPrincipalWork = PrincipalsWork[0];
                    }
                }
            }

        }



        /// <summary>
        /// Work filters combobox fill
        /// </summary>
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

        private async void FillWorkTypes() {
            dbContext = new WorkOrganizerContext();
            WorkTypes.Clear();
            if (SelectedPrincipalWork.Name != "-") {
                using (dbContext) {
                    var Worktypes = await dbContext.WorkComponents
                        .Where(x => x.WorkId == SelectedPrincipalWork.WorkId)
                        .Include(x => x.WorkTypes)
                        .Select(o => o.WorkTypes)
                        .ToListAsync();

                    foreach (var task in Worktypes) {
                        WorkTypes.Add(task);
                    }
                    WorkTypes.Insert(0, new WorkType() { Name = "-" });
                    if (WorkTypes.Count > 0)
                        SelectedWorkType = WorkTypes[0];
                }
            }
        }



        /// <summary>
        /// Work Type Comboboxes Fill
        /// </summary>
        public ObservableCollection<WorkType> WorkTypes { get; set; }
        private WorkType? _selectedWorkType;

        public WorkType SelectedWorkType {
            get { return _selectedWorkType; }
            set {
                _selectedWorkType = value;
                UpdateWorkTypeSelectedInProgramSettings();
                OnPropertyChange();
            }
        }

        private async void UpdateWorkTypeSelectedInProgramSettings() {
            if (_selectedWorkType != null) {
                if (_selectedWorkType.Name != "-") {
                    dbContext = new WorkOrganizerContext();
                    using (dbContext) {
                        var x = await dbContext.WorkComponents.FirstOrDefaultAsync(
                        s => s.WorkTypeId == _selectedWorkType.Id && s.WorkId == _selectedPrincipalWork.WorkId);
                        if (x != null) {
                            ProgramSettings.currentWorkComponent = x;
                        }
                    }
                }
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


        public void HideLoginButton() {
            CurrentView = null;
        }

        private async void CreateFilterForTaskView() {
            taskMV.FilterByWorkAndWorkType(SelectedWorkType, SelectedPrincipalWork, SelectedPrincipal);
        }


    }
}
