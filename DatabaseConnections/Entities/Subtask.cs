using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkOrganizer;
using WorkOrganizer.UI.MVVM.ViewModel;

namespace DatabaseConnection.Entities {
    public class Subtask : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }


        private bool? _status;
        public bool? Status {
            get { return _status; }
            set {
                _status = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged();
                new Task(async () => {
                    await UpdateStatus();
                }).Start();
                    

            }
        }

        public ToDoTask MainTask { get; set; }
        public int MainTaskID { get; set; }

        private async Task UpdateStatus() {
            if (Id != null) {
                try {
                    WorkOrganizerContext context = new WorkOrganizerContext();
                    using (context) {
                        var thisObject = context.Subtasks.FirstOrDefault(x => x.Id == this.Id);
                        if (thisObject != null) {
                            thisObject.Status = this._status;
                            await context.SaveChangesAsync();
                            var el = context.Subtasks.FirstOrDefault(x => x.Status == false);
                            if (el == null) {
                                TaskViewModel tMV = TaskViewModel.GetInstance();
                                var mainTask =tMV.Tasks.FirstOrDefault(x => x.ToDoTaskID == this.MainTaskID);
                                if (mainTask != null) {
                                    mainTask.Status = true;
                                    var tsk = context.Tasks.FirstOrDefault(x => x.ToDoTaskID == mainTask.ToDoTaskID);
                                    tsk.Status = true;
                                    await context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                } catch (Exception ex) {
                }
            }

        }
    }
}
