using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseConnection.Entities {
    public class User {
        [Key]
        public int UserID { get; set; }

        public string Name{ get; set; }
        public string Mail { get; set; }

        public List<ToDoTask> ToDoTasks { get; set; }= new List<ToDoTask>();
        public List<Message> Messages { get; set; } = new List<Message>();

    }
}
