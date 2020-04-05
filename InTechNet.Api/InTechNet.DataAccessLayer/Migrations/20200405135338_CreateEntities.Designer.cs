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
    [Migration("20200405135338_CreateEntities")]
    partial class CreateEntities
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
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("HubId")
                        .HasColumnType("integer");

                    b.Property<int?>("PupilId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("HubId");

                    b.HasIndex("PupilId");

                    b.ToTable("attendee","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Hubs.Hub", b =>
                {
                    b.Property<int>("Id")
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

                    b.Property<int?>("ModeratorId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("HubLink")
                        .HasName("index_hub_link");

                    b.HasIndex("ModeratorId");

                    b.ToTable("hub","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.AvailableModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("HubId")
                        .HasColumnType("integer");

                    b.Property<int?>("ModuleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("HubId");

                    b.HasIndex("ModuleId");

                    b.ToTable("available_module","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.CurrentModule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AttendeeId")
                        .HasColumnType("integer");

                    b.Property<int?>("ModuleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AttendeeId");

                    b.HasIndex("ModuleId");

                    b.ToTable("current_module","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.Module", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ModuleName")
                        .HasColumnType("character varying(32)")
                        .HasMaxLength(32);

                    b.Property<int?>("SubscriptionPlanId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionPlanId");

                    b.ToTable("module","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("character varying(32)")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .HasName("index_tag_name");

                    b.ToTable("tag","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ModuleId")
                        .HasColumnType("integer");

                    b.Property<int?>("TagId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId");

                    b.HasIndex("TagId");

                    b.ToTable("topic","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Resources.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<int?>("ModuleId")
                        .HasColumnType("integer");

                    b.Property<int?>("NextResourceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId");

                    b.HasIndex("NextResourceId");

                    b.ToTable("resource","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Resources.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AttendeeId")
                        .HasColumnType("integer");

                    b.Property<int?>("ResourceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AttendeeId");

                    b.HasIndex("ResourceId");

                    b.ToTable("state","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Users.Moderator", b =>
                {
                    b.Property<int>("Id")
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

                    b.Property<int?>("ModeratorSubscriptionPlanId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ModeratorEmail")
                        .HasName("index_moderator_email");

                    b.HasIndex("ModeratorNickname")
                        .HasName("index_moderator_nickname");

                    b.HasIndex("ModeratorSubscriptionPlanId");

                    b.ToTable("moderator","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Users.Pupil", b =>
                {
                    b.Property<int>("Id")
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

                    b.HasKey("Id");

                    b.HasIndex("PupilEmail")
                        .HasName("index_pupil_email");

                    b.HasIndex("PupilNickname")
                        .HasName("index_pupil_nickname");

                    b.ToTable("pupil","public");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Users.SubscriptionPlan", b =>
                {
                    b.Property<int>("Id")
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

                    b.HasKey("Id");

                    b.ToTable("subscription_plan","public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MaxAttendeesPerHub = 32,
                            MaxHubPerModeratorAccount = 3,
                            MaxModulePerHub = 3,
                            SubscriptionPlanName = "Standard",
                            SubscriptionPlanPrice = 0.0m
                        },
                        new
                        {
                            Id = 2,
                            MaxAttendeesPerHub = 50,
                            MaxHubPerModeratorAccount = 5,
                            MaxModulePerHub = 5,
                            SubscriptionPlanName = "Premium",
                            SubscriptionPlanPrice = 5.0m
                        },
                        new
                        {
                            Id = 3,
                            MaxAttendeesPerHub = 60,
                            MaxHubPerModeratorAccount = 10,
                            MaxModulePerHub = 15,
                            SubscriptionPlanName = "Platinum",
                            SubscriptionPlanPrice = 10.0m
                        });
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Hubs.Attendee", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Hubs.Hub", "Hub")
                        .WithMany("Attendees")
                        .HasForeignKey("HubId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Users.Pupil", "Pupil")
                        .WithMany("Attendees")
                        .HasForeignKey("PupilId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Hubs.Hub", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Users.Moderator", "Moderator")
                        .WithMany("Hubs")
                        .HasForeignKey("ModeratorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.AvailableModule", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Hubs.Hub", "Hub")
                        .WithMany("AvailableModules")
                        .HasForeignKey("HubId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Modules.Module", "Module")
                        .WithMany("AvailableModules")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.CurrentModule", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Hubs.Attendee", "Attendee")
                        .WithMany("CurrentModules")
                        .HasForeignKey("AttendeeId");

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Modules.Module", "Module")
                        .WithMany("CurrentModules")
                        .HasForeignKey("ModuleId");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.Module", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Users.SubscriptionPlan", "SubscriptionPlan")
                        .WithMany("Modules")
                        .HasForeignKey("SubscriptionPlanId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Modules.Topic", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Modules.Module", "Module")
                        .WithMany("Topics")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Modules.Tag", "Tag")
                        .WithMany("Topics")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Resources.Resource", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Modules.Module", "Module")
                        .WithMany("Resources")
                        .HasForeignKey("ModuleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Resources.Resource", "NextResource")
                        .WithMany()
                        .HasForeignKey("NextResourceId");
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Resources.State", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Hubs.Attendee", "Attendee")
                        .WithMany("States")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Resources.Resource", "Resource")
                        .WithMany("States")
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Users.Moderator", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Users.SubscriptionPlan", "ModeratorSubscriptionPlan")
                        .WithMany("Moderators")
                        .HasForeignKey("ModeratorSubscriptionPlanId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}