﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseConnection.Entities {
    public class WorkComponent {
        [Key]
        public Guid ComponentId { get; set; }

        public List<ToDoTask> Tasks { get; set; }
        public List<Message> Messages { get; set; }
        public WorkType WorkTypes { get; set; } 
        public int WorkTypeId { get; set; }    

        public Work Works { get; set; }
        public Guid WorkId { get; set; }

    }
}
