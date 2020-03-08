using InTechNet.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace InTechNet.DataAccessLayer
{
    public class InTechNetContext : DbContext
    {
        ///<summary>
        /// DbSet for the Moderator Entity
        ///</summary>
        public DbSet<Moderator> Moderators { get; set; }

        ///<summary>
        /// DbSet for the Pupil Entity
        ///</summary>
        public DbSet<Pupil> Pupils { get; set; }

        ///<summary>
        /// DbSet for the Hub Entity
        ///</summary>
        public DbSet<Hub> Hubs { get; set; }

        ///<summary>
        /// DbSet for the Attendee Entity
        ///</summary>
        public DbSet<Attendee> Attendees { get; set; }


        ///<summary>
        /// Basic constructor for IntechNetContext
        ///</summary>
        public InTechNetContext(DbContextOptions<InTechNetContext> options)
            : base(options) { }

        ///<summary>
        /// Initialize initial data 
        ///</summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moderator>()
                .HasIndex(b => b.ModeratorNickname);

            modelBuilder.Entity<Pupil>()
                .HasIndex(b => b.PupilNickname);

            modelBuilder.Entity<Hub>()
                .HasIndex(b => b.HubName);

            modelBuilder.Entity<Moderator>()
                .HasData(
                new Moderator
                {
                    IdModerator = 1,
                    ModeratorEmail = "test@test.com",
                    ModeratorNickname = "modeNick",
                    ModeratorPassword = "mdp123",
                    ModeratorSalt = "lesaltcestbien"
                }
            );

            modelBuilder.Entity<Pupil>().HasData(
                new Pupil
                {
                    IdPupil = 1,
                    PupilEmail = "pupil@pupil.com",
                    PupilNickname = "pupilNick",
                    PupilPassword = "mdp456",
                    PupilSalt = "leselcestdrole"
                }
            );

            modelBuilder.Entity<Hub>().HasData(
                new Hub
                {
                    IdHub = 1,
                    HubLink = "hublink1",
                    HubCreationDate = DateTime.Now,
                    HubName = "supername"
                }
            );
        }

    }
}
