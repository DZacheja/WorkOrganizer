using DatabaseConnection.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrganizer.DatabaseConnections.Entities {
    public class FavoriteFilter {
        public Principal FavPrincipal { get; set; }
        public Work FavWork { get; set; }
        public WorkType FavWorkType { get; set; }
        public string FavStringMatch { get; set; }
        public string FilterName { get; set; }
    }
}
