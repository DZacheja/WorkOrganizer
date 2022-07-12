using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseConnection.Entities {
    public class Principal {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "serial")]
        public int? PrincipalID { get; set; }
        public string Name { get; set; }
        public List<Work> Works { get; set; } = new List<Work>();
    }
}