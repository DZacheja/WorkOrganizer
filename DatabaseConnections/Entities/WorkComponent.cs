using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseConnection.Entities {
    public class WorkComponent {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? ComponentId { get; set; }

        public List<ToDoTask> Tasks { get; set; }
        public List<Message> Messages { get; set; }
        public WorkType WorkTypes { get; set; } 
        public int WorkTypeId { get; set; }    

        public Work Works { get; set; }
        public int WorkId { get; set; }

    }
}
