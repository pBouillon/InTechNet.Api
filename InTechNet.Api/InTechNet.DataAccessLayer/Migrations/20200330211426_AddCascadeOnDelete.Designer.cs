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
    [Migration("20200330211426_AddCascadeOnDelete")]
    partial class AddCascadeOnDelete
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Attendee", b =>
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

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Hub", b =>
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

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Moderator", b =>
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

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Pupil", b =>
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

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.SubscriptionPlan", b =>
                {
                    b.Property<int>("IdSubscriptionPlan")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("MaxAttendeesPerHub")
                        .HasColumnType("integer");

                    b.Property<int>("MaxHubPerModeratorAccount")
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
                            SubscriptionPlanName = "Standard",
                            SubscriptionPlanPrice = 0.0m
                        });
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Attendee", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Hub", "Hub")
                        .WithMany("Attendees")
                        .HasForeignKey("HubIdHub")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InTechNet.DataAccessLayer.Entities.Pupil", "Pupil")
                        .WithMany("Attendees")
                        .HasForeignKey("PupilIdPupil")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Hub", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.Moderator", "Moderator")
                        .WithMany("Hubs")
                        .HasForeignKey("ModeratorIdModerator")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("InTechNet.DataAccessLayer.Entities.Moderator", b =>
                {
                    b.HasOne("InTechNet.DataAccessLayer.Entities.SubscriptionPlan", "ModeratorSubscriptionPlan")
                        .WithMany("Moderators")
                        .HasForeignKey("ModeratorSubscriptionPlanIdSubscriptionPlan")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
