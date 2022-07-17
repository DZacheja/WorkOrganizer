using Microsoft.EntityFrameworkCore;
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
using WorkOrganizer.UI.Core;
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


        private bool _status;
        
        public bool Status {
            get { return _status; }
            set {
                // Call OnPropertyChanged whenever the property is updated
                    _status = value;
                if (Update) {
                    OnPropertyChanged();
                    new Task(async () => {
                        await UpdateStatus();
                    }).Start();
                }
                    

            }
        }
        [NotMapped]
        public bool Update = true;
        private User _confirmedPersonSubtask;

        public User ConfirmedPersonSubtask {
            get { return _confirmedPersonSubtask; }
            set { _confirmedPersonSubtask = value;
                OnPropertyChanged();
            }
        }
        public int? ConfirmedPersonSubtaskID { get; set; }

        public ToDoTask MainTask { get; set; }
        public int MainTaskID { get; set; }

        private async Task UpdateStatus() {
            if (Id != null) {
                try {
                    WorkOrganizerContext context = new WorkOrganizerContext();
                    using (context) {
                        var thisObject = await context.Subtasks.FirstOrDefaultAsync(x => x.Id == this.Id);
                        if (thisObject != null) {
                            thisObject.Update = false;
                            thisObject.Status = this.Status;
                            if (this.Status == true) {
                                thisObject.ConfirmedPersonSubtaskID = ProgramSettings.currentUser.UserID;
                                this.ConfirmedPersonSubtask = ProgramSettings.currentUser;
                                this.ConfirmedPersonSubtaskID = ProgramSettings.currentUser.UserID;
                            } else {
                                thisObject.ConfirmedPersonSubtaskID = null;
                                this.ConfirmedPersonSubtask = null;
                                this.ConfirmedPersonSubtaskID = null;
                            }
                            await context.SaveChangesAsync();
                            var el = await context.Subtasks.FirstOrDefaultAsync(x => x.Status == false);
                            if (el == null) {
                                TaskViewModel tMV = TaskViewModel.GetInstance();
                                var mainTask = tMV.Tasks.FirstOrDefault(x => x.ToDoTaskID == this.MainTaskID);
                                if (mainTask != null) {
                                    mainTask.Status = true;
                                    var tsk = await context.Tasks.FirstOrDefaultAsync(x => x.ToDoTaskID == mainTask.ToDoTaskID);
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
