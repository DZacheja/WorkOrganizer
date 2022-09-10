using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkOrganizer.UI.Core;
using WorkOrganizer.UI.MVVM.ViewModel;

namespace DatabaseConnection.Entities {
    public class ToDoTask : INotifyPropertyChanged {
        private bool _status;
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ToDoTaskID { get; set; }

        public string Content { get; set; }

        public DateTime Deadline { get; set; }
        public bool Status {
            get { return _status; }
            set {
                _status = value;
                // Call OnPropertyChanged whenever the property is updated
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

        public WorkComponent Component { get; set; }
        public int ComponentsId { get; set; }

        public User Authors { get; set; }
        public int? AuthorsID { get; set; }

        public User? ConfirmedPersonTask { get; set; }
        public int? ConfirmedPersonTaskID { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private List<Subtask> _subtasks = new List<Subtask>();

        public List<Subtask> Subtaskas {
            get { return _subtasks; }
            set {
                _subtasks = value;
                OnPropertyChanged();
            }
        }


        private async Task UpdateStatus() {
            try {
                if (ToDoTaskID != null) {
                    WorkOrganizerContext context = new WorkOrganizerContext();
                    using (context) {
                        var thisObject = context.Tasks.FirstOrDefault(x => x.ToDoTaskID == this.ToDoTaskID);
                        if (thisObject != null) {
                            thisObject.Update = false;
                            thisObject.Status = this.Status;
                            if(this.Status == true && ProgramSettings.currentUser != null && ProgramSettings.currentUser.UserID != null) {
                                this.ConfirmedPersonTask = ProgramSettings.currentUser;

                                thisObject.ConfirmedPersonTaskID = ProgramSettings.currentUser.UserID;
                                this.ConfirmedPersonTaskID = ProgramSettings.currentUser.UserID;
                            } else {
                                this.ConfirmedPersonTask = null;

                                thisObject.ConfirmedPersonTaskID = null;
                                this.ConfirmedPersonTaskID = null;
                            }
                            await context.SaveChangesAsync();
                        }
                    }
                }
            } catch (Exception ex) {
                MainModel m = MainModel.Instance;
                await m.showLblInfo("Błąd podczas aktualizacji!", "#00cc00");
            }



        }

    }
}
