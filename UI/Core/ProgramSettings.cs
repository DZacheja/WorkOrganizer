using DatabaseConnection.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrganizer.UI.Core {
    public static class ProgramSettings {
        public static readonly string key = "$B&E)H@McQfTjWnZ";
        public static readonly string LoggedUserClassFile = "LoggedUser.txt";
        public static User? currentUser;


        public static WorkComponent? currentWorkComponent { get; set; }
        public static ObservableCollection<ToDoTask>? TasksTable { get; set; }  

    }
}
