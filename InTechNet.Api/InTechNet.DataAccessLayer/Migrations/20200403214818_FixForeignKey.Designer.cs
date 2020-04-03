﻿// <auto-generated />
using System;
using InTechNet.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace InTechNet.DataAccessLayer.Migrations
{
    [DbContext(typeof(InTechNetContext))]
    [Migration("20200403214818_FixForeignKey")]
    partial class FixForeignKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Hubs.Attendee", b =>
                {
                    b.Property<int>("IdAttendee")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("HubIdHub")
                        .HasColumnType("integer");

                    b.Property<int>("IdHub")
                        .HasColumnType("integer");

                    b.Property<int>("IdPupil")
                        .HasColumnType("integer");

                    b.Property<int?>("PupilIdPupil")
                        .HasColumnType("integer");

                    b.HasKey("IdAttendee");

                    b.HasIndex("HubIdHub");

                    b.HasIndex("PupilIdPupil");

                    b.ToTable("attendee","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Hubs.Hub", b =>
                {
                    b.Property<int>("IdHub")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("HubCreationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("HubDescription")
                        .HasColumnType("text");

                    b.Property<string>("HubLink")
                        .HasColumnType("text");

                    b.Property<string>("HubName")
                        .HasColumnType("text");

                    b.Property<int?>("ModeratorIdModerator")
                        .HasColumnType("integer");

                    b.HasKey("IdHub");

                    b.HasIndex("HubLink")
                        .HasName("index_hub_link");

                    b.HasIndex("ModeratorIdModerator");

                    b.ToTable("hub","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.CurrentModule", b =>
                {
                    b.Property<int>("IdCurrentModule")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AttendeeIdAttendee")
                        .HasColumnType("integer");

                    b.Property<int>("IdAttendee")
                        .HasColumnType("integer");

                    b.Property<int>("IdModule")
                        .HasColumnType("integer");

                    b.Property<int?>("ModuleIdModule")
                        .HasColumnType("integer");

                    b.HasKey("IdCurrentModule");

                    b.HasIndex("AttendeeIdAttendee");

                    b.HasIndex("ModuleIdModule");

                    b.ToTable("CurrentModules");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.Module", b =>
                {
                    b.Property<int>("IdModule")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ModuleName")
                        .HasColumnType("character varying(32)")
                        .HasMaxLength(32);

                    b.Property<string>("ModuleType")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.HasKey("IdModule");

                    b.ToTable("module","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.SelectedModule", b =>
                {
                    b.Property<int>("IdSelectedModule")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("HubIdHub")
                        .HasColumnType("integer");

                    b.Property<int>("IdHub")
                        .HasColumnType("integer");

                    b.Property<int>("IdModule")
                        .HasColumnType("integer");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("boolean");

                    b.Property<int?>("ModuleIdModule")
                        .HasColumnType("integer");

                    b.HasKey("IdSelectedModule");

                    b.HasIndex("HubIdHub");

                    b.HasIndex("ModuleIdModule");

                    b.ToTable("selected_module","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.Tag", b =>
                {
                    b.Property<int>("IdTag")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("character varying(32)")
                        .HasMaxLength(32);

                    b.HasKey("IdTag");

                    b.ToTable("tag","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.Topic", b =>
                {
                    b.Property<int>("IdTopic")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("IdModule")
                        .HasColumnType("integer");

                    b.Property<int>("IdTag")
                        .HasColumnType("integer");

                    b.Property<int?>("ModuleIdModule")
                        .HasColumnType("integer");

                    b.Property<int?>("TagIdTag")
                        .HasColumnType("integer");

                    b.HasKey("IdTopic");

                    b.HasIndex("ModuleIdModule");

                    b.HasIndex("TagIdTag");

                    b.ToTable("topic","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Resources.Resource", b =>
                {
                    b.Property<int>("IdResource")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<int>("IdModule")
                        .HasColumnType("integer");

                    b.Property<int?>("IdNextResource")
                        .HasColumnType("integer");

                    b.Property<int?>("ModuleIdModule")
                        .HasColumnType("integer");

                    b.Property<int?>("fk_next_resource")
                        .HasColumnType("integer");

                    b.HasKey("IdResource");

                    b.HasIndex("ModuleIdModule");

                    b.HasIndex("fk_next_resource");

                    b.ToTable("resource","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Resources.State", b =>
                {
                    b.Property<int>("IdState")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AttendeeIdAttendee")
                        .HasColumnType("integer");

                    b.Property<int>("IdAttendee")
                        .HasColumnType("integer");

                    b.Property<int>("IdResource")
                        .HasColumnType("integer");

                    b.Property<int?>("ResourceIdResource")
                        .HasColumnType("integer");

                    b.HasKey("IdState");

                    b.HasIndex("AttendeeIdAttendee");

                    b.HasIndex("ResourceIdResource");

                    b.ToTable("state","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Users.Moderator", b =>
                {
                    b.Property<int>("IdModerator")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ModeratorEmail")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ModeratorNickname")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.Property<string>("ModeratorPassword")
                        .HasColumnType("text");

                    b.Property<string>("ModeratorSalt")
                        .HasColumnType("text");

                    b.Property<int?>("ModeratorSubscriptionPlanIdSubscriptionPlan")
                        .HasColumnType("integer");

                    b.HasKey("IdModerator");

                    b.HasIndex("ModeratorEmail")
                        .HasName("index_moderator_email");

                    b.HasIndex("ModeratorNickname")
                        .HasName("index_moderator_nickname");

                    b.HasIndex("ModeratorSubscriptionPlanIdSubscriptionPlan");

                    b.ToTable("moderator","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Users.Pupil", b =>
                {
                    b.Property<int>("IdPupil")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("PupilEmail")
                        .HasColumnType("character varying(128)")
                        .HasMaxLength(128);

                    b.Property<string>("PupilNickname")
                        .HasColumnType("character varying(64)")
                        .HasMaxLength(64);

                    b.Property<string>("PupilPassword")
                        .HasColumnType("text");

                    b.Property<string>("PupilSalt")
                        .HasColumnType("text");

                    b.HasKey("IdPupil");

                    b.HasIndex("PupilEmail")
                        .HasName("index_pupil_email");

                    b.HasIndex("PupilNickname")
                        .HasName("index_pupil_nickname");

                    b.ToTable("pupil","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Users.SubscriptionPlan", b =>
                {
                    b.Property<int>("IdSubscriptionPlan")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("MaxAttendeesPerHub")
                        .HasColumnType("integer");

                    b.Property<int>("MaxHubPerModeratorAccount")
                        .HasColumnType("integer");

                    b.Property<int>("MaxModulePerHub")
                        .HasColumnType("integer");

                    b.Property<string>("SubscriptionPlanName")
                        .HasColumnType("text");

                    b.Property<decimal>("SubscriptionPlanPrice")
                        .HasColumnType("numeric");

                    b.HasKey("IdSubscriptionPlan");

                    b.ToTable("subscription_plan","public");

                    b.HasData(
                        new
                        {
                            IdSubscriptionPlan = 1,
                            MaxAttendeesPerHub = 32,
                            MaxHubPerModeratorAccount = 3,
                            MaxModulePerHub = 3,
                            SubscriptionPlanName = "Standard",
                            SubscriptionPlanPrice = 0.0m
                        },
                        new
                        {
                            IdSubscriptionPlan = 2,
                            MaxAttendeesPerHub = 50,
                            MaxHubPerModeratorAccount = 5,
                            MaxModulePerHub = 5,
                            SubscriptionPlanName = "Premium",
                            SubscriptionPlanPrice = 5.0m
                        },
                        new
                        {
                            IdSubscriptionPlan = 3,
                            MaxAttendeesPerHub = 60,
                            MaxHubPerModeratorAccount = 10,
                            MaxModulePerHub = 15,
                            SubscriptionPlanName = "Platinium",
                            SubscriptionPlanPrice = 10.0m
                        });
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Hubs.Attendee", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Hubs.Hub", "Hub")
                        .WithMany("Attendees")
                        .HasForeignKey("HubIdHub")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Users.Pupil", "Pupil")
                        .WithMany("Attendees")
                        .HasForeignKey("PupilIdPupil")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Hubs.Hub", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Users.Moderator", "Moderator")
                        .WithMany("Hubs")
                        .HasForeignKey("ModeratorIdModerator")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.CurrentModule", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Hubs.Attendee", "Attendee")
                        .WithMany()
                        .HasForeignKey("AttendeeIdAttendee");

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Modules.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleIdModule");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.SelectedModule", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Hubs.Hub", "Hub")
                        .WithMany("SelectedModules")
                        .HasForeignKey("HubIdHub")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Modules.Module", "Module")
                        .WithMany("SelectedModules")
                        .HasForeignKey("ModuleIdModule")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.Topic", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Modules.Module", "Module")
                        .WithMany("Topics")
                        .HasForeignKey("ModuleIdModule")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Modules.Tag", "Tag")
                        .WithMany("Topics")
                        .HasForeignKey("TagIdTag")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Resources.Resource", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Modules.Module", "Module")
                        .WithMany("Resources")
                        .HasForeignKey("ModuleIdModule")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Resources.Resource", "NextResource")
                        .WithMany()
                        .HasForeignKey("fk_next_resource");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Resources.State", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Hubs.Attendee", "Attendee")
                        .WithMany("States")
                        .HasForeignKey("AttendeeIdAttendee")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Resources.Resource", "Resource")
                        .WithMany("States")
                        .HasForeignKey("ResourceIdResource")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Users.Moderator", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Users.SubscriptionPlan", "ModeratorSubscriptionPlan")
                        .WithMany("Moderators")
                        .HasForeignKey("ModeratorSubscriptionPlanIdSubscriptionPlan")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
