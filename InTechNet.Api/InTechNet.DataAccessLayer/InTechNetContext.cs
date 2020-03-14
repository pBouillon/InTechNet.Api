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
        /// Basic constructor for InTechNetContext
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
                .HasData(new Moderator
                {
                    IdModerator = 1,
                    ModeratorEmail = "test@test.com",
                    ModeratorNickname = "modeNick",
                    // From raw: "mdp123"
                    ModeratorPassword = "CC3827BF052E6B257CE6FBE896077A132448552CA6746CD538A11039950636ABD7440927318E5D9EBBD151C6A93364B8F5AD761A871403227395F4D99D01E34A",
                    ModeratorSalt = "lesaltcestbien"
                });

            modelBuilder.Entity<Pupil>()
                .HasData(new Pupil
                {
                    IdPupil = 1,
                    PupilEmail = "pupil@pupil.com",
                    PupilNickname = "pupilNick",
                    // From raw: "mdp456"
                    PupilPassword = "4230B63D16DCEF8861AA9BE6F93B46F2E2ED20EC6C3E7E6001CDEC44DE1186BA015D98F19D3D5C43D38F84CBD00FDC977058066791A2AF7ACFE8863F92C71F8B",
                    PupilSalt = "leselcestdrole"
                });

            modelBuilder.Entity<Hub>()
                .HasData(new Hub
                {
                    IdHub = 1,
                    HubLink = "hublink1",
                    HubCreationDate = DateTime.Now,
                    HubName = "supername"
                });
        }

    }
}
