using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection.Entities {
    public class ToDoTask {
        [Key]
        public int ToDoTaskID { get; set; }

        public string Content { get; set; }

        public DateTime Deadline { get; set; }
        public bool Status { get; set; }

        public WorkComponent Component { get; set; }
        public int ComponentsId { get; set; }

        public User Authors { get; set; }
        public int AuthorsID { get; set; }


    }
}
