﻿// <auto-generated />
using System;
using DatabaseConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WorkOrganizer.Migrations
{
    [DbContext(typeof(WorkOrganizerContext))]
    [Migration("20220715153332_defaultSubtaskStatus")]
    partial class defaultSubtaskStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DatabaseConnection.Entities.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("MessageId"));

                    b.Property<int>("AuthorsId")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("WorkComponentsId")
                        .HasColumnType("integer");

                    b.HasKey("MessageId");

                    b.HasIndex("AuthorsId");

                    b.HasIndex("WorkComponentsId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.Principal", b =>
                {
                    b.Property<int?>("PrincipalID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("PrincipalID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("PrincipalID");

                    b.ToTable("Principals");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.Subtask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MainTaskID")
                        .HasColumnType("integer");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.HasKey("Id");

                    b.HasIndex("MainTaskID");

                    b.ToTable("Subtasks");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.ToDoTask", b =>
                {
                    b.Property<int>("ToDoTaskID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ToDoTaskID"));

                    b.Property<int>("AuthorsID")
                        .HasColumnType("integer");

                    b.Property<int>("ComponentsId")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.HasKey("ToDoTaskID");

                    b.HasIndex("AuthorsID");

                    b.HasIndex("ComponentsId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.User", b =>
                {
                    b.Property<int?>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("UserID"));

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserLogin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.Work", b =>
                {
                    b.Property<int?>("WorkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("WorkId"));

                    b.Property<string>("ColorHTML")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("PrincipalsId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("WorkId");

                    b.HasIndex("PrincipalsId");

                    b.ToTable("Works");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.WorkComponent", b =>
                {
                    b.Property<int?>("ComponentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("ComponentId"));

                    b.Property<int>("WorkId")
                        .HasColumnType("integer");

                    b.Property<int>("WorkTypeId")
                        .HasColumnType("integer");

                    b.HasKey("ComponentId");

                    b.HasIndex("WorkId");

                    b.HasIndex("WorkTypeId");

                    b.ToTable("WorkComponents");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.WorkType", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("WorkTypes");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.Message", b =>
                {
                    b.HasOne("DatabaseConnection.Entities.User", "Authors")
                        .WithMany("Messages")
                        .HasForeignKey("AuthorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DatabaseConnection.Entities.WorkComponent", "WorkComponents")
                        .WithMany("Messages")
                        .HasForeignKey("WorkComponentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Authors");

                    b.Navigation("WorkComponents");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.Subtask", b =>
                {
                    b.HasOne("DatabaseConnection.Entities.ToDoTask", "MainTask")
                        .WithMany("Subtaskas")
                        .HasForeignKey("MainTaskID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MainTask");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.ToDoTask", b =>
                {
                    b.HasOne("DatabaseConnection.Entities.User", "Authors")
                        .WithMany("ToDoTasks")
                        .HasForeignKey("AuthorsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DatabaseConnection.Entities.WorkComponent", "Component")
                        .WithMany("Tasks")
                        .HasForeignKey("ComponentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Authors");

                    b.Navigation("Component");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.Work", b =>
                {
                    b.HasOne("DatabaseConnection.Entities.Principal", "Principals")
                        .WithMany("Works")
                        .HasForeignKey("PrincipalsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Principals");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.WorkComponent", b =>
                {
                    b.HasOne("DatabaseConnection.Entities.Work", "Works")
                        .WithMany()
                        .HasForeignKey("WorkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DatabaseConnection.Entities.WorkType", "WorkTypes")
                        .WithMany()
                        .HasForeignKey("WorkTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkTypes");

                    b.Navigation("Works");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.Principal", b =>
                {
                    b.Navigation("Works");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.ToDoTask", b =>
                {
                    b.Navigation("Subtaskas");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.User", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("ToDoTasks");
                });

            modelBuilder.Entity("DatabaseConnection.Entities.WorkComponent", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
