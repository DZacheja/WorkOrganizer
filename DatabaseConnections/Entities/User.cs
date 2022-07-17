using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseConnection.Entities {
    public class User {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? UserID { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string Name{ get; set; }
        public string Mail { get; set; }

        public List<ToDoTask>? ToDoTasks { get; set; }= new List<ToDoTask>();
        public List<ToDoTask>? ConfirmedTasks { get; set; }
        public List<Subtask>? ConfirmedSubTasks { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();

    }
}
