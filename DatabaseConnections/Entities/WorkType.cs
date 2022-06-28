using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseConnection.Entities {
    public class WorkType {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Work> Works { get; set; } = new List<Work>();
    }
}
