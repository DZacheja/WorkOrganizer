﻿using DatabaseConnection;
using DatabaseConnection.Entities;
using Microsoft.EntityFrameworkCore;
using ModernDesign.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WorkOrganizer.DatabaseConnections.Entities;
using WorkOrganizer.UI.Core;
using WorkOrganizer.UI.MVVM.View;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public class MainModel : ObservableObject {
        private static readonly Lazy<MainModel> instance = new Lazy<MainModel>(() => new MainModel());
        public static MainModel Instance => instance.Value;

        public WorkOrganizerContext dbContext;

        private object? _currentView;
        public object? CurrentView {
            get { return _currentView; }
            set {
                _currentView = value;
                OnPropertyChange();
            }
        }

        public RelayCommand loginPageCommand { get; set; }
        public RelayCommand taskCommand { get; set; }
        public RelayCommand filterTaskView { get; set; }
        public RelayCommand newWorkCommand { get; set; }
        public RelayCommand settingsCommand { get; set; }
        public RelayCommand saveFavoriteFilter { get; set; }

        public LoginPageModel loginPageMV { get; set; }
        public TaskViewModel taskMV { get; set; }
        public NewWorkPageModel newWorkPageMV { get; set; }
        public SettingsModel settingsMV { get; set; }


        private string? _currentUserLogged;

        public string? CurrentUserLogged {
            get { return _currentUserLogged; }
            set {
                _currentUserLogged = value;
                OnPropertyChange();
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        private MainModel() {
            dbContext = new WorkOrganizerContext();
            // ...  Login Page ...  //
            loginPageMV = LoginPageModel.GetInstance();
            loginPageCommand = new RelayCommand(o => {
                CurrentView = loginPageMV;
            });

            // ...  Task View ...  //
            taskMV = TaskViewModel.GetInstance();
            taskCommand = new RelayCommand(o => {
                CurrentView = taskMV;
            });

            // ... New Work View ... //
            newWorkPageMV = NewWorkPageModel.Instance();
            newWorkCommand = new RelayCommand(o => {
                if (CurrentView == newWorkPageMV) {
                    CurrentView = null;
                } else {
                    newWorkPageMV.RandColor();
                    CurrentView = newWorkPageMV;
                }
            });


            // ... New Settings View ... ///
            settingsMV = SettingsModel.Instance;
            settingsCommand = new RelayCommand(o => {
                if (CurrentView == settingsMV)
                    CurrentView = null;
                else
                    CurrentView = settingsMV;
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

            // ... Favorite Filters ... //
            FavoriteFiltersList = new ObservableCollection<FavoriteFilter>();
            try {
                var elem = File.ReadAllText(ProgramSettings.FavoriteFiltersFile);
                if (elem != null) {
                    var ds = AesOperation.DecryptString(ProgramSettings.key, elem);
                    List<FavoriteFilter> filter = JsonConvert.DeserializeObject<List<FavoriteFilter>>(ds);
                    if (filter != null) {
                        foreach (FavoriteFilter favoriteFilter in filter) {
                            FavoriteFiltersList.Add(favoriteFilter);
                        }
                    }
                }
            } catch (Exception) { }

            saveFavoriteFilter = new RelayCommand(prop => SaveFilter(), prop => true);

        }


        public void RefreshPrincipals() {
            dbContext = new WorkOrganizerContext();
            var dbList = dbContext.Principals.ToList();
            Principals.Clear();
            Principals.Add(new Principal { Name = "-" });
            foreach (var task in dbList) {
                Principals.Add(task);
            }

            if (Principals.Count > 0)
                SelectedPrincipal = Principals[0];
            ProgramSettings.currentWorkComponent = null;
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
                    ProgramSettings.currentWorkComponent = null;

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
                } else
                    ProgramSettings.currentWorkComponent = null;
            } else
                ProgramSettings.currentWorkComponent = null;
        }


        /// <summary>
        /// Login Visibility
        /// </summary>

        private Visibility _loginVisibility;

        public Visibility LoginBtnVisibility {
            get { return _loginVisibility; }
            set {
                _loginVisibility = value;

                OnPropertyChange();
            }
        }

        private Visibility _otherButtonsVilibility;
        public Visibility OtherButtonsVilibility {
            get { return _otherButtonsVilibility; }
            set {
                _otherButtonsVilibility = value;

                OnPropertyChange();
            }
        }
        public void HideLoginButton() {
            LoginBtnVisibility = Visibility.Collapsed;
            OtherButtonsVilibility = Visibility.Visible;
            CurrentView = null;
            CurrentUserLogged = ProgramSettings.currentUser.Name;
        }

        public void ShowLoginButton() {
            LoginBtnVisibility = Visibility.Visible;
            OtherButtonsVilibility = Visibility.Collapsed;
            CurrentView = null;
        }


        private async void CreateFilterForTaskView() {
            if (CurrentView == taskMV) {
                taskMV.FilterByWorkAndWorkType(SelectedWorkType, SelectedPrincipalWork, SelectedPrincipal);
            }
        }

        ///<summary>
        ///Filters
        /// </summary>
        /// 
        public ObservableCollection<FavoriteFilter> FavoriteFiltersList { get; set; }
        private FavoriteFilter _selectedFavoriteFilter;

        public FavoriteFilter SelectedFavoriteFilter {
            get { return _selectedFavoriteFilter; }
            set {
                _selectedFavoriteFilter = value;
                if (value != null) {
                    taskMV.FilterText = SelectedFavoriteFilter.FavStringMatch;
                    taskMV.FilterByWorkAndWorkType(
                    wt: SelectedFavoriteFilter.FavWorkType,
                    w: SelectedFavoriteFilter.FavWork,
                    p: SelectedFavoriteFilter.FavPrincipal);
                }
                OnPropertyChange();
            }
        }

        private void SaveFilter() {
            InputBox ib = new InputBox("Wpisz nazwę filtra,Nowy filtr");
            ib.ShowDialog();
            if (ib.Results != null) {
                FavoriteFilter fv = new FavoriteFilter() {
                    FavStringMatch = taskMV.FilterText,
                    FilterName = ib.Results
                };

                if(SelectedPrincipal != null) 
                    fv.FavPrincipal = new Principal() { PrincipalID = SelectedPrincipal.PrincipalID, Name = SelectedPrincipal.Name };

                if (SelectedPrincipalWork != null)
                    fv.FavWork = new Work() { WorkId = SelectedPrincipalWork.WorkId, Name = SelectedPrincipalWork.Name };

                if (SelectedWorkType != null)
                    fv.FavWorkType = new WorkType() { Id = SelectedWorkType.Id, Name = SelectedWorkType.Name };
                
                FavoriteFiltersList.Add(fv);
                var s = JsonConvert.SerializeObject(FavoriteFiltersList);
                var es = AesOperation.EncryptString(ProgramSettings.key, s);
                File.WriteAllText(ProgramSettings.FavoriteFiltersFile, es);
            }

        }

        public void RemoveFilter() {
            if (SelectedFavoriteFilter != null) {
                FavoriteFiltersList.Remove(SelectedFavoriteFilter);
                var s = JsonConvert.SerializeObject(FavoriteFiltersList);
                var es = AesOperation.EncryptString(ProgramSettings.key, s);
                File.WriteAllText(ProgramSettings.FavoriteFiltersFile, es);
            }

        }
        /// <summary>
        /// LBL info
        /// </summary>
        private Visibility? _lblInfoVisibility = Visibility.Collapsed;
        public Visibility? lblInfoVisibility {
            get { return _lblInfoVisibility; }
            set {
                if (_lblInfoVisibility != value)
                    _lblInfoVisibility = value;
                OnPropertyChange();
            }
        }


        private string? _lblInfoText;

        public string? LblInfoText {
            get { return _lblInfoText; }
            set {
                _lblInfoText = value;
                LblInfoText = "";
            }
        }

        private Brush _lblInfoColor;

        public Brush LblInfoColor {
            get { return _lblInfoColor; }
            set {
                _lblInfoColor = value;
                OnPropertyChange();
            }
        }


        public async Task showLblInfo(string txt, string color) {
            _ = Dispatcher.CurrentDispatcher.Invoke(async () => {
                _lblInfoText = txt;
                var bc = new BrushConverter();
                LblInfoColor = (Brush)bc.ConvertFrom(color);
                lblInfoVisibility = System.Windows.Visibility.Visible;
                await Task.Delay(3000);
                lblInfoVisibility = System.Windows.Visibility.Collapsed;
            });
        }



    }
}
