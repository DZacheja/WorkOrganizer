using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseConnection.Entities {
    public class Work {
        [Key]
        public Guid WorkId { get; set; }
        public string Name { get; set; }
        public string ColorHTML { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Principal Principals { get; set; }
        public Guid PrincipalsId { get; set; }

        public List<WorkType> Components { get; set; } = new List<WorkType>();
        
    }
}
