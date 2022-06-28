using DatabaseConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrganizer.DatabaseConnections.Entities {
    public class WorkOrganizerContextFactory: IDesignTimeDbContextFactory<WorkOrganizerContext> {
        public WorkOrganizerContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<WorkOrganizerContext>();
            optionsBuilder.UseNpgsql(@"Server=localhost;Database=DatabaseWorkOrganizerDb;Port=5432;User Id=postgres;Password=admin");

            return new WorkOrganizerContext(optionsBuilder.Options);
        }
    }
}
