using DatabaseConnection;
using DatabaseConnection.Entities;
using Microsoft.EntityFrameworkCore;
using ModernDesign.Core;
using System;
using System.Collections.Generic;
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
            FilterOnlyAvtiveTasks = true;

            if (ProgramSettings.TasksTable == null) {
                dbContext = new WorkOrganizerContext();
                var tsk = dbContext.Tasks.Include(a => a.Authors)
                .Include(x=>x.ConfirmedPersonTask)
                .Include(x => x.Component).ThenInclude(x => x.Works)
                .Include(x => x.Component).ThenInclude(x => x.WorkTypes)
                .Include(x => x.Subtaskas).ThenInclude(x=>x.ConfirmedPersonSubtask)
                .Where(x => x.Status == false)
                .OrderByDescending(o => o.Deadline)
                .ToList();

                Tasks = new ObservableCollection<ToDoTask>();
                foreach (var task in tsk) {
                    task.Subtaskas = task.Subtaskas.OrderBy(x => x.Id).ToList();
                    Tasks.Add(task);
                }
                ProgramSettings.TasksTable = Tasks;
            } else {
                Tasks = ProgramSettings.TasksTable;
            }

            NewSubtasksList = new ObservableCollection<string>();

            editSelectedTask = new RelayCommand(o => {
                System.Diagnostics.Debug.WriteLine("działa -- - adsa asd- sad !");
            });
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
                            if (NewSubtasksList.Count > 0) {
                                List<Subtask> NewSubtasks = new List<Subtask>();
                                int max = await dbContext.Subtasks.MaxAsync(x => x.Id);
                                max++;
                                foreach (string s in NewSubtasksList) {
                                    Subtask subtask = new Subtask() {
                                        Id = max,
                                        MainTaskID = newTask.ToDoTaskID,
                                        Content = s
                                    };
                                    NewSubtasks.Add(subtask);
                                    max++;
                                }
                                await dbContext.AddRangeAsync(NewSubtasks);
                                if(await dbContext.SaveChangesAsync() == 0) {
                                    dbContext.Tasks.Remove(newTask);
                                    try {
                                        await dbContext.SaveChangesAsync();
                                        throw new Exception("Bład podczas dodwawania elementów zadania!");
                                    } catch (Exception) {
                                        throw new Exception("Bład podczas usuwania błednie dodanego zadania!");
                                    }
                                }
                            }
                                var tsk = await dbContext.Tasks.Include(a => a.Authors)
                                            .Include(x => x.ConfirmedPersonTask)
                                            .Include(x => x.Component).ThenInclude(x => x.Works)
                                            .Include(x => x.Component).ThenInclude(x => x.WorkTypes)
                                            .Include(x => x.Subtaskas).ThenInclude(x => x.ConfirmedPersonSubtask)
                                    .FirstOrDefaultAsync(x => x.ToDoTaskID == newTask.ToDoTaskID);
                                Tasks.Add(newTask);
                                NewTaskText = "";
                                NewSubtasksList.Clear();
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


        private string _newSubtaskText;

        public string NewSubtaskTekst {
            get { return _newSubtaskText; }
            set {
                _newSubtaskText = value;
                OnPropertyChange();
            }
        }

        private bool _addSubtaskClicked;

        public bool AddSubtaskClicked {
            get { return _addSubtaskClicked; }
            set {
                _addSubtaskClicked = value;
                OnPropertyChange();
                AddSubtaskClick();
            }
        }

        public ObservableCollection<string> NewSubtasksList { get; set; }

        public void AddSubtaskToList() {
            if (NewSubtaskTekst != "") {
                NewSubtasksList.Add(NewSubtaskTekst);
                NewSubtaskTekst = "";
            }
        }
        public async Task AddSubtaskClick() {
            if (SelectedTask != null && NewSubtaskTekst != "") {
                dbContext = new WorkOrganizerContext();
                using (dbContext) {
                    var mxST = dbContext.Subtasks.Max(x => x.Id);
                    mxST++;
                    Subtask s = new Subtask() {
                        Id = mxST,
                        MainTaskID = SelectedTask.ToDoTaskID,
                        Content = NewSubtaskTekst
                    };
                    dbContext.Subtasks.Add(s);
                    if (await dbContext.SaveChangesAsync() > 0) {
                        SelectedTask.Subtaskas.Add(s);
                        SelectedTask.Status = false;
                        SelectedTask.Subtaskas = SelectedTask.Subtaskas;
                        OnPropertyChange();
                        var todotask = await dbContext.Tasks.FirstOrDefaultAsync(x => x.ToDoTaskID == SelectedTask.ToDoTaskID);
                        if (todotask != null) {
                            todotask.Status = false;
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
                NewSubtaskTekst = "";
            }
        }

        #endregion

        #region Select and edit tasks

        public RelayCommand editSelectedTask { get; set; }


        private bool _filterOnlyAvtiveTasks;

        public bool FilterOnlyAvtiveTasks {
            get { return _filterOnlyAvtiveTasks; }
            set {
                _filterOnlyAvtiveTasks = value;
                OnPropertyChange();
            }
        }

        private string _filterText;

        public string FilterText {
            get { return _filterText; }
            set {
                _filterText = value;
                OnPropertyChange();
            }
        }



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
        /// Selected task
        /// </summary>
        private Subtask? _selectedSubTask;

        public Subtask? SelectedSubTask {
            get { return _selectedSubTask; }
            set {
                _selectedSubTask = value;
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

                var f = dbContext.Tasks.Include(a => a.Authors)
                                .Include(x => x.ConfirmedPersonTask)
                                .Include(x => x.Component).ThenInclude(x => x.Works)
                                .Include(x => x.Component).ThenInclude(x => x.WorkTypes)
                                .Include(x => x.Subtaskas).ThenInclude(x => x.ConfirmedPersonSubtask);


                if (p.Name != "-") { //Filter by Principal
                    var x = f.Where(o => o.Component.Works.PrincipalsId == p.PrincipalID);
                    if (w != null) {
                        if (w.Name != "-") { //Filter By Work
                            x = x.Where(o => o.Component.Works.WorkId == w.WorkId);
                            if (wt != null) {
                                if (wt.Name != "-") { //Filter By WorkType
                                    x = x.Where(o => o.Component.WorkTypeId == wt.Id);

                                }
                            }
                        }
                    }

                    if (FilterOnlyAvtiveTasks && (FilterText != null && FilterText != "")) {
                        x = x.Where(o => o.Status == false).Where(x => x.Content.Contains(FilterText));
                    } else if (FilterOnlyAvtiveTasks) {
                        x = x.Where(o => o.Status == false);
                    } else if (FilterText != null && FilterText != "") {
                        x = x.Where(o => o.Content.Contains(FilterText));
                    }

                    var res = x.OrderByDescending(o => o.Deadline).ToList();

                    foreach (var task in res) {
                        task.Subtaskas = task.Subtaskas.OrderBy(x => x.Id).ToList();
                        Tasks.Add(task);
                    }
                } else {
                    IQueryable<ToDoTask>? fs = null;
                    if (FilterOnlyAvtiveTasks && (FilterText != null && FilterText != "")) {
                        fs = f.Where(o => o.Status == false).Where(x => x.Content.Contains(FilterText));
                    } else if (FilterOnlyAvtiveTasks) {
                        fs = f.Where(o => o.Status == false);
                    } else if (FilterText != null && FilterText != "") {
                        fs = f.Where(o => o.Content.Contains(FilterText));
                    }
                    List<ToDoTask>? res = null;

                    if (fs != null)
                        res = fs.OrderByDescending(o => o.Deadline).ToList();
                    else
                        res = f.OrderByDescending(o => o.Deadline).ToList();

                    foreach (var task in res) {
                        task.Subtaskas = task.Subtaskas.OrderBy(x => x.Id).ToList();
                        Tasks.Add(task);
                    }
                }

            }

        }
        #endregion
    }
}
