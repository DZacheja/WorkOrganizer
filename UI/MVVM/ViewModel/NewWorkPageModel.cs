using DatabaseConnection;
using DatabaseConnection.Entities;
using Microsoft.EntityFrameworkCore;
using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public class NewWorkPageModel : ObservableObject {
        private static NewWorkPageModel _instance;
        private WorkOrganizerContext dbContext;

        public static NewWorkPageModel Instance() {
            if (_instance == null) {
                _instance = new NewWorkPageModel();
            }
            return _instance;
        }

        private NewWorkPageModel() {
            dbContext = new WorkOrganizerContext();
            SelectedWorkTypes = new List<WorkType>();
            PrincipalList = new ObservableCollection<Principal>();
            using (dbContext) {
                var principals = dbContext.Principals.ToList();
                foreach (var principal in principals) {
                    PrincipalList.Add(principal);
                }

                WorkTypesList = new ObservableCollection<WorkType>();
                var workTypes = dbContext.WorkTypes.ToList();
                foreach (var workType in workTypes) {
                    WorkTypesList.Add(workType);
                }
            }
        }



        /// <summary>
        /// Principal selection
        /// </summary>

        private ObservableCollection<Principal> _principalLists;

        public ObservableCollection<Principal> PrincipalList {
            get { return _principalLists; }
            set {
                _principalLists = value;
                OnPropertyChange();
            }
        }

        private Principal _selectedPrincipal;

        public Principal SelectedPrincipal {
            get { return _selectedPrincipal; }
            set {
                _selectedPrincipal = value;
                OnPropertyChange();
            }
        }

        private bool _newPrincipalChcek;

        public bool NewPrincipalCheck {
            get { return _newPrincipalChcek; }
            set {
                _newPrincipalChcek = value;
                OnPropertyChange();
            }
        }

        /// <summary>
        /// New Work Type
        /// </summary>

        private ObservableCollection<WorkType> _workTypesList;

        public ObservableCollection<WorkType> WorkTypesList {
            get { return _workTypesList; }
            set {
                _workTypesList = value;
                OnPropertyChange();
            }
        }


        public List<WorkType> SelectedWorkTypes { get; set; }

        /// <summary>
        /// Work Name
        /// </summary>
        private string _newWorkName;

        public string NewWorkName {
            get { return _newWorkName; }
            set {
                _newWorkName = value;
                OnPropertyChange();
            }
        }

        ///new Work type
        private string _newWorkTypeName;

        public string NewWorkTypeName {
            get { return _newWorkTypeName; }
            set {
                _newWorkTypeName = value;
                OnPropertyChange();
            }
        }

        ///new Principal Name
        private string _newPrincipalName;

        public string NewPrincipalName {
            get { return _newPrincipalName; }
            set {
                _newPrincipalName = value;
                OnPropertyChange();
            }
        }

        private Color _selectedColor;

        public Color SelectedColor {
            get { return _selectedColor; }
            set {
                _selectedColor = value;
                OnPropertyChange();
            }
        }


        public async Task InsertNewWorkType() {
            using (dbContext = new WorkOrganizerContext()) {
                var chk = await dbContext.WorkTypes.FirstOrDefaultAsync(x => x.Name == NewWorkTypeName);
                if (chk == null) {
                    WorkType wk = new WorkType() {
                        Name = NewWorkTypeName
                    };
                    dbContext.WorkTypes.Add(wk);
                    try {
                        if (await dbContext.SaveChangesAsync() > 0) {
                            WorkTypesList.Add(wk);
                        }
                    } catch (Exception) {
                        throw new Exception("Błąd podaczas dodawania nowego rodzaju roboty");
                    }
                } else {
                    throw new Exception("Podany typ roboty już istnieje!");
                }
            }
        }


        public async Task InsertNewWork(DateTime time) {
            try {
                using (dbContext = new WorkOrganizerContext()) {
                    //Check principal first
                    if (NewPrincipalCheck) {
                        if (NewPrincipalName == null || NewPrincipalName == "")
                            throw new Exception("Pusta nazwa nowego zleceniodawcy!");

                        var chk = await dbContext.Principals.FirstOrDefaultAsync(x => x.Name == NewPrincipalName);
                        if (chk == null) {
                            chk = new Principal();
                            chk.Name = NewPrincipalName;
                            dbContext.Add(chk);

                            if (await dbContext.SaveChangesAsync() > 0) {
                                PrincipalList.Add(chk);
                                SelectedPrincipal = chk;
                            } else {
                                throw new Exception("Błąd podczas dodwania nowego pracodawcy");
                            }

                        } else {
                            throw new Exception("Podny zleceniodawca już istnieje!");
                        }
                    }
                    //Insert new Work
                    time = time.ToUniversalTime();
                    if (SelectedPrincipal.PrincipalID != null) {
                        var maxWork = dbContext.Works.Max(x => x.WorkId);
                        Work w = new Work() {
                            Name = NewWorkName,
                            EndDate = time,
                            PrincipalsId = (int)SelectedPrincipal.PrincipalID,
                            ColorHTML = SelectedColor.ToString()
                        };
                        dbContext.Add(w);
                        if (await dbContext.SaveChangesAsync() > 0) {
                            var maxWorkType = dbContext.WorkComponents.Max(x => x.ComponentId);
                            maxWorkType++;
                            List<WorkComponent> wc = new List<WorkComponent>();
                            foreach (var wt in SelectedWorkTypes) {
                                WorkComponent nwk = new WorkComponent() {
                                    WorkId = (int)w.WorkId,
                                    WorkTypeId = (int)wt.Id,
                                    ComponentId = (int)maxWorkType
                                };
                                wc.Add(nwk);
                                maxWorkType++;
                            }
                            await dbContext.AddRangeAsync(wc);
                            if(await dbContext.SaveChangesAsync() == 0) {
                                throw new Exception("Błąd podczas dodwania elementów zlecenia");
                            }
                        } else {
                            throw new Exception("Błąd podczas dodwania nowego zlecenia");
                        }
                    } else {
                        throw new Exception("Nie odnaleziono zaznaczonego zleceniodawcy!");
                    }
                }
            } catch (Exception) {
                throw new Exception("Błąd podczas dodwania nowego pracodawcy");

            }
        }

    }
}
