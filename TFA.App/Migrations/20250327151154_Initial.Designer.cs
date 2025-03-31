﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TFA.App.Database.Context;

#nullable disable

namespace TFA.App.Migrations
{
    [DbContext(typeof(ApplicationDataContext))]
    [Migration("20250327151154_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProjectUser", b =>
                {
                    b.Property<Guid>("ProjectsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("ProjectsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserProjects", (string)null);
                });

            modelBuilder.Entity("TFA.App.Domain.Models.Projects.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("ProjectDescription");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("ProjectName");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid")
                        .HasColumnName("ProjectOwnerId");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("TFA.App.Domain.Models.Tasks.TaskObject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("TaskComment");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("TaskCompletedAt");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("TaskCreatedAt");

                    b.Property<Guid>("CurrentOwnerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Deadline")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("TaskDeadline");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("TaskName");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("ParentTaskId")
                        .HasColumnType("uuid")
                        .HasColumnName("TaskParentTaskId");

                    b.Property<int>("Priority")
                        .HasColumnType("integer")
                        .HasColumnName("TaskPriority");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uuid");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("TaskState");

                    b.Property<Guid?>("TaskObjectId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CurrentOwnerId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("TaskObjectId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TFA.App.Domain.Models.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("UserEmail");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("UserFirstName");

                    b.Property<string>("IdentityId")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("UserIdentityId");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("UserLastName");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ProjectUser", b =>
                {
                    b.HasOne("TFA.App.Domain.Models.Projects.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TFA.App.Domain.Models.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TFA.App.Domain.Models.Tasks.TaskObject", b =>
                {
                    b.HasOne("TFA.App.Domain.Models.Users.User", "CurrentOwner")
                        .WithMany("CurrentOwnedTasks")
                        .HasForeignKey("CurrentOwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TFA.App.Domain.Models.Users.User", "Owner")
                        .WithMany("OwnedTasks")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TFA.App.Domain.Models.Projects.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TFA.App.Domain.Models.Tasks.TaskObject", null)
                        .WithMany("Subtasks")
                        .HasForeignKey("TaskObjectId");

                    b.Navigation("CurrentOwner");

                    b.Navigation("Owner");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("TFA.App.Domain.Models.Projects.Project", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("TFA.App.Domain.Models.Tasks.TaskObject", b =>
                {
                    b.Navigation("Subtasks");
                });

            modelBuilder.Entity("TFA.App.Domain.Models.Users.User", b =>
                {
                    b.Navigation("CurrentOwnedTasks");

                    b.Navigation("OwnedTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
