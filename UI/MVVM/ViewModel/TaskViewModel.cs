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
                .Include(x => x.Component).ThenInclude(x => x.Works)
                .Include(x => x.Component).ThenInclude(x => x.WorkTypes)
                .OrderByDescending(o => o.Deadline).ToList();

            Tasks = new ObservableCollection<ToDoTask>();
            foreach (var task in tsk) {
                Tasks.Add(task);
            }
        }


        public void FilterByWorkAndWorkType(WorkType wt, Work w, Principal p) {
            if (Tasks != null) {
                var x = Tasks.Where(x => x.Component.Works.WorkId == w.WorkId).Where(y => y.Component.WorkTypeId == wt.Id).ToList();
                Tasks.Clear();
                foreach (var task in x) {
                    Tasks.Add(task);
                }

            }
        }
    }
}
