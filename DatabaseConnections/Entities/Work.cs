using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseConnection.Entities {
    public class Work {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? WorkId { get; set; }
        public string Name { get; set; }
        public string ColorHTML { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Principal Principals { get; set; }
        public int PrincipalsId { get; set; }

        public List<WorkType> Components { get; set; } = new List<WorkType>();
        
    }
}
