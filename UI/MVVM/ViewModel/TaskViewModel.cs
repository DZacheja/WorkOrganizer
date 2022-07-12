using DatabaseConnection;
using DatabaseConnection.Entities;
using Microsoft.EntityFrameworkCore;
using ModernDesign.Core;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WorkOrganizer.UI.Core;

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public sealed class TaskViewModel : ObservableObject, INotifyPropertyChanged {
        /// <summary>
        /// Class instance
        /// </summary>
        private static TaskViewModel _instance;

        /// <summary>
        /// Db context for database connections
        /// </summary>
        private WorkOrganizerContext dbContext;

        /// <summary>
        /// Public constructor
        /// </summary>
        /// <returns>Object instance</returns>
        public static TaskViewModel GetInstance() {
            if (_instance == null) {
                _instance = new TaskViewModel();
            }
            return _instance;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private TaskViewModel() {
            addNewTaskVisible = Visibility.Collapsed;
            

            if (ProgramSettings.TasksTable == null) {
                dbContext = new WorkOrganizerContext();
                var tsk = dbContext.Tasks.Include(a => a.Authors)
                .Include(x => x.Component).ThenInclude(x => x.Works)
                .Include(x => x.Component).ThenInclude(x => x.WorkTypes)
                .OrderByDescending(o => o.Deadline).ToList();
                Tasks = new ObservableCollection<ToDoTask>();
                foreach (var task in tsk) {
                    Tasks.Add(task);
                }
                ProgramSettings.TasksTable = Tasks;
            } else {
                Tasks = ProgramSettings.TasksTable;
            }


        }

        #region add new Task

        public async Task InsertingNewTask(DateTime? deadline) {
            if (_newTaskText != null &&
                deadline != null &&
                ProgramSettings.currentWorkComponent != null
                && ProgramSettings.currentUser != null) {

                DateTime utc = (DateTime)deadline;
                utc = utc.ToUniversalTime();
                ToDoTask newTask = new ToDoTask() {
                    AuthorsID = (int)ProgramSettings.currentUser.UserID,
                    ComponentsId = (int)ProgramSettings.currentWorkComponent.ComponentId,
                    Content = _newTaskText,
                    Status = false,
                    Deadline = utc
                };
                dbContext = new WorkOrganizerContext();
                try {
                    using (dbContext) {

                        dbContext.Tasks.Add(newTask);

                        var ok = await dbContext.SaveChangesAsync();

                        if (ok > 0) {

                            var tsk = await dbContext.Tasks
                                .Include(a => a.Authors)
                                .Include(x => x.Component).ThenInclude(x => x.Works)
                                .Include(x => x.Component).ThenInclude(x => x.WorkTypes)
                                .FirstOrDefaultAsync(x => x.ToDoTaskID == newTask.ToDoTaskID);
                            Tasks.Add(newTask);
                            NewTaskText = "";
                        }

                    }
                } catch (Exception) {
                    throw new Exception("Błąd podczas dodawania zadania!");
                }
            } else {
                throw new Exception("Wybierz rodzaj roboty, dla której dodać zadanie");
            }
        }

        /// <summary>
        /// New Task item text
        /// </summary>
        private string _newTaskText;

        public string NewTaskText {
            get { return _newTaskText; }
            set {
                _newTaskText = value;
                OnPropertyChange();
            }
        }

        /// <summary>
        /// Visibility of add new task GridRow
        /// </summary>
        private Visibility addNewTaskVisible;
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

        #endregion

        #region Select and edit tasks

        /// <summary>
        /// Collections of tasks
        /// </summary>
        private ObservableCollection<ToDoTask> _tasks;

        public ObservableCollection<ToDoTask> Tasks {
            get {
                if (_tasks == null) {
                    _tasks = new ObservableCollection<ToDoTask>();
                }
                return _tasks;
            }
            set {
                _tasks = value;
                OnPropertyChange();
            }
        }

        /// <summary>
        /// Selected task
        /// </summary>
        private ToDoTask? selectedTask;

        public ToDoTask? SelectedTask {
            get { return selectedTask; }
            set {
                selectedTask = value;
                OnPropertyChange();
            }
        }


        /// <summary>
        /// Insert new filter to listview and get data from database
        /// </summary>
        /// <param name="wt">Work Type object</param>
        /// <param name="w">Work Name object</param>
        /// <param name="p">Principal object</param>
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

        /// <summary>
        /// Chaange status of selected task on doubleclick
        /// </summary>
        public async void UpdateChange() {
            if (selectedTask != null) {
                dbContext = new WorkOrganizerContext();
                if (selectedTask.Status == false) {
                    using (dbContext) {
                        var td = await dbContext.Tasks.FirstOrDefaultAsync(t => t.ToDoTaskID == selectedTask.ToDoTaskID);
                        td.Status = true;
                        //var x = Tasks.FirstOrDefault(t => t.ToDoTaskID == selectedTask.ToDoTaskID);
                        //x.Status = true;
                        selectedTask.Status = true;
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }
        #endregion
    }
}
