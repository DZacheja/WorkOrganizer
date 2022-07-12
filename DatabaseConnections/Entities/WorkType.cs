using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseConnection.Entities {
    public class WorkType {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "serial")]
        public int? Id { get; set; }
        public string Name { get; set; }

        public List<Work> Works { get; set; } = new List<Work>();
    }
}
