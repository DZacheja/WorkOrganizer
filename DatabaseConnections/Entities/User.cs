using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseConnection.Entities {
    public class User {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "serial")]
        public int? UserID { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string Name{ get; set; }
        public string Mail { get; set; }

        public List<ToDoTask> ToDoTasks { get; set; }= new List<ToDoTask>();
        public List<Message> Messages { get; set; } = new List<Message>();

    }
}
