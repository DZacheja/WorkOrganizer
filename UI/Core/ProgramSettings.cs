using DatabaseConnection.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrganizer.UI.Core {
    public static class ProgramSettings {

        public static User? currentUser { get; set; }
        public static WorkComponent? currentWorkComponent { get; set; }
        public static ObservableCollection<ToDoTask>? TasksTable { get; set; }  

    }
}
