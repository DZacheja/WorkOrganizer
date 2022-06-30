using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseConnection.Entities {
    public class Principal {
        [Key]
        public int PrincipalID { get; set; }
        public string Name { get; set; }
        public List<Work> Works { get; set; } = new List<Work>();
    }
}