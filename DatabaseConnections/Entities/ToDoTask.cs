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
using WorkOrganizer.UI.MVVM.ViewModel;

namespace DatabaseConnection.Entities {
    public class ToDoTask : INotifyPropertyChanged {
        private bool? status;
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ToDoTaskID { get; set; }

        public string Content { get; set; }

        public DateTime Deadline { get; set; }
        public bool? Status {
            get { return status; }
            set {
                status = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged();
                new Task(async () => {
                    await UpdateStatus();
                }).Start();

            }
        }
        public WorkComponent Component { get; set; }
        public int ComponentsId { get; set; }

        public User Authors { get; set; }
        public int AuthorsID { get; set; }

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
                            thisObject.Status = this.status;
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
