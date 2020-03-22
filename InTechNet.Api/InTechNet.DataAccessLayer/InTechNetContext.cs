using System.Collections.Generic;
using InTechNet.Common.Utils.SubscriptionPlan;
using InTechNet.DataAccessLayer.Entities;
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
        /// DbSet for the Subscription Entity
        /// </summary>
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriptionPlan>()
                .HasMany(_ => _.Moderators)
                .WithOne(_ => _.ModeratorSubscriptionPlan);

            modelBuilder.Entity<Hub>()
                .HasMany(_ => _.Attendees)
                .WithOne(_ => _.Hub);

            modelBuilder.Entity<Moderator>()
                .HasMany(_ => _.Hubs)
                .WithOne(_ => _.Moderator);

            modelBuilder.Entity<Moderator>()
                .HasIndex(b => b.ModeratorNickname)
                .HasName("index_moderator_nickname");

            modelBuilder.Entity<Moderator>()
                .HasIndex(b => b.ModeratorEmail)
                .HasName("index_moderator_email");

            modelBuilder.Entity<Pupil>()
                .HasIndex(b => b.PupilNickname)
                .HasName("index_pupil_nickname");

            modelBuilder.Entity<Pupil>()
                .HasIndex(b => b.PupilEmail)
                .HasName("index_pupil_email");

            modelBuilder.Entity<Hub>()
                .HasIndex(b => b.HubLink)
                .HasName("index_hub_link");

            PopulateSubscriptionPlans(modelBuilder);
        }

        private static void PopulateSubscriptionPlans(ModelBuilder modelBuilder)
        {
            var subscriptionPlans = new Queue<SubscriptionPlan>();

            // Free plan
            var freeSubscriptionPlan = new FreeSubscriptionPlan();
            subscriptionPlans.Enqueue(new SubscriptionPlan
            {
                Moderators = new List<Moderator>(),
                MaxAttendeesPerHub = freeSubscriptionPlan.MaxAttendeesPerHubCount,
                MaxHubPerModeratorAccount = freeSubscriptionPlan.MaxHubsCount,
                SubscriptionPlanName = freeSubscriptionPlan.SubscriptionPlanName,
                SubscriptionPlanPrice = freeSubscriptionPlan.Price
            });

            modelBuilder.Entity<Pupil>()
                .HasData(subscriptionPlans);
        }
    }
}
