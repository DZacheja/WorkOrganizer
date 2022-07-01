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
    public sealed class TaskViewModel : ObservableObject {
        private static TaskViewModel _instance;

        public static TaskViewModel GetInstance() {
            if (_instance == null) {
                _instance = new TaskViewModel();
            }
            return _instance;
        }

        public ObservableCollection<ToDoTask> Tasks { get; set; }

        private WorkOrganizerContext dbContext;
        private TaskViewModel() {
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
            dbContext = new WorkOrganizerContext();
            using (dbContext) {
                Tasks.Clear();
                var x = dbContext.Tasks
                    .Where(x => x.Component.Works.WorkId == w.WorkId)
                    .Where(y => y.Component.WorkTypeId == wt.Id)
                    .ToList();
                foreach (var task in x) {
                    Tasks.Add(task);
                }
            }

        }
    }
}
