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
        public DbSet<Subtask> Subtasks { get; set; }

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

                us.Property(x => x.UserID).ValueGeneratedOnAdd();
            });
            //Message

            //Principals table
            modelBuilder.Entity<Principal>(p => {
                p.HasMany(w => w.Works)
                    .WithOne(w => w.Principals)
                    .HasForeignKey(p => p.PrincipalsId);
                p.Property(x => x.PrincipalID).ValueGeneratedOnAdd();
            });

            //ToDoTask table
            modelBuilder.Entity<ToDoTask>(e => {
                e.Property(x => x.Status).HasDefaultValue(false);

                e.HasMany(x => x.Subtaskas)
                .WithOne(x => x.MainTask)
                .HasForeignKey(x => x.MainTaskID);

                e.Property(x => x.ToDoTaskID).ValueGeneratedOnAdd();
                //e.HasOne(x => x.ConfirmedPersonTask).WithMany(x => x.ConfirmedTasks).HasForeignKey(x=>x.ConfirmedPersonTaskID);

            });

            //Works tabel
            modelBuilder.Entity<Work>(x => {
                x.HasMany(w => w.Components)
                .WithMany(w => w.Works)
                .UsingEntity<WorkComponent>(
                w => w.HasOne(wi => wi.WorkTypes)
                .WithMany()
                .HasForeignKey(wi => wi.WorkTypeId),

                w => w.HasOne(wi => wi.Works)
                .WithMany()
                .HasForeignKey(wi => wi.WorkId)
                );

                x.Property(x => x.StartDate).HasDefaultValueSql("now()");
                x.Property(x => x.WorkId).ValueGeneratedOnAdd();

            });

            //Work Component
            modelBuilder.Entity<WorkComponent>(wc => {
                wc.HasMany(w => w.Tasks)
                .WithOne(t => t.Component)
                .HasForeignKey(c => c.ComponentsId);

                wc.HasMany(w => w.Messages)
                .WithOne(m => m.WorkComponents)
                .HasForeignKey(k => k.WorkComponentsId);

            });


            modelBuilder.Entity<Message>(m => {
                m.Property(w => w.Created).HasDefaultValueSql("now()");
                m.Property(x => x.MessageId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<WorkComponent>().Property(x => x.ComponentId).ValueGeneratedOnAdd();
            modelBuilder.Entity<WorkType>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Subtask>(s => {
                s.Property(x => x.Id).ValueGeneratedOnAdd();
                s.Property(x => x.Status).HasDefaultValue(false);
                //s.HasOne(x => x.ConfirmedPersonSubtask).WithMany();
            });



        }
    }
}