using System;
using System.ComponentModel.DataAnnotations;
namespace DatabaseConnection.Entities {
    public class Message {
        [Key]
        public Guid MessageId { get; set; }
        public string Content{ get; set; }
        public DateTime Created { get; set; }
        
        public WorkComponent WorkComponents{ get; set; }
        public Guid WorkComponentsId { get; set; }

        public User Authors { get; set; }
        public Guid AuthorsId { get; set; }


    }
}
