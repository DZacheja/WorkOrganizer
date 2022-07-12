using DatabaseConnection.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnection {
    public class WorkOrganizerContext : DbContext {

        public DbSet<Work> Works { get; set; }
        public DbSet<Principal> Principals { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<WorkComponent> WorkComponents { get; set; }
        public DbSet<WorkType> WorkTypes { get; set; }
        public DbSet<ToDoTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //optionsBuilder.UseNpgsql(@"Server=localhost;Database=DatabaseWorkOrganizerDb;Port=5432;User Id=postgres;Password=admin");
            optionsBuilder.UseNpgsql(@"Server=127.0.0.1;Database=DatabaseWorkOrganizerDb;Port=5432;User Id=postgres;Password=admin");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //Users table
            modelBuilder.Entity<User>(us => {
                us.HasMany(u => u.ToDoTasks)
                .WithOne(t => t.Authors)
                .HasForeignKey(a => a.AuthorsID);

                us.HasMany(m => m.Messages)
                .WithOne(u => u.Authors)
                .HasForeignKey(a => a.AuthorsId);

            });
            //Message

            //Principals table
            modelBuilder.Entity<Principal>(p => {
                p.HasMany(w => w.Works)
                    .WithOne(w => w.Principals)
                    .HasForeignKey(p => p.PrincipalsId);
            });

            //Works tabel
            modelBuilder.Entity<Work>().HasMany(w => w.Components)
                .WithMany(w => w.Works)
                .UsingEntity<WorkComponent>(
                w => w.HasOne(wi => wi.WorkTypes)
                .WithMany()
                .HasForeignKey(wi => wi.WorkTypeId),

                w => w.HasOne(wi => wi.Works)
                .WithMany()
                .HasForeignKey(wi => wi.WorkId)
                );

            //Work Component
            modelBuilder.Entity<WorkComponent>(wc => {
                wc.HasMany(w => w.Tasks)
                .WithOne(t => t.Component)
                .HasForeignKey(c => c.ComponentsId);

                wc.HasMany(w => w.Messages)
                .WithOne(m => m.WorkComponents)
                .HasForeignKey(k => k.WorkComponentsId);

            });

            modelBuilder.Entity<ToDoTask>().Property(x => x.Status).HasDefaultValue(false);

            modelBuilder.Entity<Message>().Property(w => w.Created).HasDefaultValueSql("now()");
            modelBuilder.Entity<Work>().Property(x => x.StartDate).HasDefaultValueSql("now()");

            modelBuilder.Entity<Message>().Property(x => x.MessageId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Principal>().Property(x => x.PrincipalID).ValueGeneratedOnAdd();
            modelBuilder.Entity<ToDoTask>().Property(x => x.ToDoTaskID).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(x => x.UserID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Work>().Property(x => x.WorkId).ValueGeneratedOnAdd();
            modelBuilder.Entity<WorkComponent>().Property(x => x.ComponentId).ValueGeneratedOnAdd();
            modelBuilder.Entity<WorkType>().Property(x => x.Id).ValueGeneratedOnAdd();
        }




    }
}
