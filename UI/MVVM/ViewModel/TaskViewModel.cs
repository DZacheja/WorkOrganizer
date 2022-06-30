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

namespace WorkOrganizer.UI.MVVM.ViewModel {
    public class TaskViewModel: ObservableObject {
        public ObservableCollection<ToDoTask> Tasks { get; set; }

        private WorkOrganizerContext dbContext;
        public TaskViewModel() {
            dbContext = new WorkOrganizerContext();
            var tsk = dbContext.Tasks.Include(a => a.Authors)
                .Include(x => x.Component).ThenInclude(x => x.Works).ToList();
            Tasks = new ObservableCollection<ToDoTask>();
            foreach (var task in tsk) {
                Tasks.Add(task);
            }
        }

        private async void FillTaskObject() {

        }
    }
}
