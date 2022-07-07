using DatabaseConnection;
using DatabaseConnection.Entities;
using Microsoft.EntityFrameworkCore;
using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public sealed class TaskViewModel : ObservableObject {
        private static TaskViewModel _instance;
        public static TaskViewModel GetInstance() {
            if (_instance == null) {
                _instance = new TaskViewModel();
            }
            return _instance;
        }

        private Visibility addNewTaskVisible;
        public RelayCommand changeStatusOfTast;
        public Visibility AddNewTaskVisible {
            get { return addNewTaskVisible; }
            set {
                addNewTaskVisible = value;
                OnPropertyChange();
            }
        }

        private bool addNewTaskVisibleClicked;

        public bool AddNewTaskVisibleButtonClicked {
            get { return addNewTaskVisibleClicked; }
            set {
                addNewTaskVisibleClicked = value;
                if (value)
                    AddNewTaskVisible = Visibility.Visible;
                else
                    AddNewTaskVisible = Visibility.Collapsed;
                OnPropertyChange();
            }
        }


        public ObservableCollection<ToDoTask> Tasks { get; set; }
        private ToDoTask? selectedTask;

        public ToDoTask? SelectedTask {
            get { return selectedTask; }
            set { selectedTask = value;
                OnPropertyChange();
            }
        }

        private WorkOrganizerContext dbContext;
        private TaskViewModel() {
            addNewTaskVisible = Visibility.Collapsed;
            dbContext = new WorkOrganizerContext();
            var tsk = dbContext.Tasks.Include(a => a.Authors)
                .Include(x => x.Component).ThenInclude(x => x.Works)
                .Include(x => x.Component).ThenInclude(x => x.WorkTypes)
                .OrderByDescending(o => o.Deadline).ToList();

            Tasks = new ObservableCollection<ToDoTask>();
            foreach (var task in tsk) {
                Tasks.Add(task);
            }
            changeStatusOfTast = new RelayCommand(
                o => ChangeStatusOfTask(), o => true);
        }


        public async void FilterByWorkAndWorkType(WorkType wt, Work w, Principal p) {
            dbContext = new WorkOrganizerContext();
            using (dbContext) {
                Tasks.Clear();

                var f = dbContext.Tasks
                    .Include(a => a.Authors)
                    .Include(x => x.Component).ThenInclude(x => x.Works)
                    .Include(x => x.Component).ThenInclude(x => x.WorkTypes);

                
                if (p.Name != "-") { //Filter by Principal
                    var x = f.Where(x => x.Component.Works.PrincipalsId == p.PrincipalID);
                    if (w != null) {
                        if (w.Name != "-") { //Filter By Work
                            x = x.Where(x => x.Component.Works.WorkId == w.WorkId);
                            if (wt != null) {
                                if (wt.Name != "-") { //Filter By WorkType
                                 x = x.Where(y => y.Component.WorkTypeId == wt.Id);

                                }
                            }
                        }
                    }
                    var res = x.OrderByDescending(o => o.Deadline).ToList();
                    foreach (var task in res) {
                        Tasks.Add(task);
                    }
                } else {
                    var res = f.OrderByDescending(o => o.Deadline).ToList();
                    foreach (var task in res) {
                        Tasks.Add(task);
                    }
                }
      
            }

        }
        public void ChangeStatusOfTask() {
            System.Diagnostics.Debug.WriteLine("ASDASDASD");
        }

        public void UpdateChange(Object sender, EventArgs e) {
            System.Diagnostics.Debug.WriteLine("ASDASDASD");

        }
    }
}
