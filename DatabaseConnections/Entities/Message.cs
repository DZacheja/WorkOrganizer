using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseConnection.Entities {
    public class Message {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1, TypeName = "serial")]
        public int MessageId { get; set; }
        public string Content{ get; set; }
        public DateTime Created { get; set; }
        
        public WorkComponent WorkComponents{ get; set; }
        public int WorkComponentsId { get; set; }

        public User Authors { get; set; }
        public int AuthorsId { get; set; }


    }
}
