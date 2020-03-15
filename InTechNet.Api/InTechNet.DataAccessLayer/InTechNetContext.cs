using System;
using InTechNet.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace InTechNet.DataAccessLayer
{
    public class InTechNetContext : DbContext
    {
        /// <summary>
        /// Basic constructor for InTechNetContext
        /// </summary>
        // ReSharper disable once SuggestBaseTypeForParameter
        public InTechNetContext(DbContextOptions<InTechNetContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet for the Moderator Entity
        /// </summary>
        public DbSet<Moderator> Moderators { get; set; }

        /// <summary>
        /// DbSet for the Pupil Entity
        /// </summary>
        public DbSet<Pupil> Pupils { get; set; }

        /// <summary>
        /// DbSet for the Hub Entity
        /// </summary>
        public DbSet<Hub> Hubs { get; set; }

        /// <summary>
        /// DbSet for the Attendee Entity
        /// </summary>
        public DbSet<Attendee> Attendees { get; set; }

        /// <summary>
        /// Initialize initial data
        /// </summary>
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
                    ModeratorEmail = "moderator@intechnet.io",
                    ModeratorNickname = "moderator",
                    // From raw: "moderator"
                    ModeratorPassword =
                        "720E39C10B81B3652B149FA74B3757AD1453F10FD4445F2A1AB4196BF2D23CE5D64A8DCD6DE157194853F35CC160F8A851155261B82B271BB81AD0B700AF9992",
                    ModeratorSalt = "moderator-salt"
                });

            modelBuilder.Entity<Pupil>()
                .HasData(new Pupil
                {
                    IdPupil = 1,
                    PupilEmail = "pupil@intechnet.io",
                    PupilNickname = "pupil",
                    // From raw: "pupil"
                    PupilPassword =
                        "CF28AF1039C0348CE7715232444454F47E085D6859913BFE531008D1BEF4992D27D7A3301E9CC70004F0F42513676FC01B941C848160351D389BBC3A264DC0E2",
                    PupilSalt = "pupil-salt"
                });

            modelBuilder.Entity<Hub>()
                .HasData(new Hub
                {
                    IdHub = 1,
                    HubLink = "hub-link",
                    HubCreationDate = DateTime.Now,
                    HubName = "hub-name"
                });
        }
    }
}
