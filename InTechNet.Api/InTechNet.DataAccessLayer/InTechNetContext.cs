using InTechNet.Common.Utils.SubscriptionPlan;
using InTechNet.DataAccessLayer.Entities.Hubs;
using InTechNet.DataAccessLayer.Entities.Modules;
using InTechNet.DataAccessLayer.Entities.Resources;
using InTechNet.DataAccessLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        /// <summary>
        /// DbSet for the Module Entity
        /// </summary>
        public DbSet<Module> Modules { get; set; }

        /// <summary>
        /// DbSet for the AvailableModule Entity
        /// </summary>
        public DbSet<AvailableModule> AvailableModules { get; set; }

        /// <summary>
        /// DbSet for the Tag Entity
        /// </summary>
        public DbSet<Tag> Tags{ get; set; }

        /// <summary>
        /// DbSet for the Topic Entity
        /// </summary>
        public DbSet<Topic> Topics{ get; set; }

        /// <summary>
        /// DbSet for the Resource Entity
        /// </summary>
        public DbSet<Resource> Resources{ get; set; }

        /// <summary>
        /// DbSet for the State Entity
        /// </summary>
        public DbSet<State> States{ get; set; }

        /// <summary>
        /// DbSet for the CurrentModule Entity
        /// </summary>
        public DbSet<CurrentModule> CurrentModules { get; set; }

        /// <summary>
        /// Build the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            InitializeRelationShips(modelBuilder);

            CreateIndexes(modelBuilder);

            PopulateSubscriptionPlans(modelBuilder);
        }

        /// <summary>
        /// Populate the subscription_plan table
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void PopulateSubscriptionPlans(ModelBuilder modelBuilder)
        {
            var subscriptionPlans = new Queue<SubscriptionPlan>();
            var subscriptionId = 0;

            // Free plan
            var freeSubscriptionPlan = new FreeSubscriptionPlan();
            subscriptionPlans.Enqueue(new SubscriptionPlan
            {
                IdSubscriptionPlan = ++subscriptionId,
                Moderators = new List<Moderator>(),
                MaxAttendeesPerHub = freeSubscriptionPlan.MaxAttendeesPerHubCount,
                MaxHubPerModeratorAccount = freeSubscriptionPlan.MaxHubsCount,
                MaxModulePerHub = freeSubscriptionPlan.MaxModulePerHub,
                SubscriptionPlanName = freeSubscriptionPlan.SubscriptionPlanName,
                SubscriptionPlanPrice = freeSubscriptionPlan.Price
            });

            // Premium plan
            var premiumSubscriptionPlan = new PremiumSubscriptionPlan();
            subscriptionPlans.Enqueue(new SubscriptionPlan
            {
                IdSubscriptionPlan = ++subscriptionId,
                Moderators = new List<Moderator>(),
                MaxAttendeesPerHub = premiumSubscriptionPlan.MaxAttendeesPerHubCount,
                MaxHubPerModeratorAccount = premiumSubscriptionPlan.MaxHubsCount,
                MaxModulePerHub = premiumSubscriptionPlan.MaxModulePerHub,
                SubscriptionPlanName = premiumSubscriptionPlan.SubscriptionPlanName,
                SubscriptionPlanPrice = premiumSubscriptionPlan.Price
            });

            // Platinum plan
            var platinumSubscriptionPlan = new PlatinumSubscriptionPlan();
            subscriptionPlans.Enqueue(new SubscriptionPlan
            {
                IdSubscriptionPlan = ++subscriptionId,
                Moderators = new List<Moderator>(),
                MaxAttendeesPerHub = platinumSubscriptionPlan.MaxAttendeesPerHubCount,
                MaxHubPerModeratorAccount = platinumSubscriptionPlan.MaxHubsCount,
                MaxModulePerHub = platinumSubscriptionPlan.MaxModulePerHub,
                SubscriptionPlanName = platinumSubscriptionPlan.SubscriptionPlanName,
                SubscriptionPlanPrice = platinumSubscriptionPlan.Price
            });

            modelBuilder.Entity<SubscriptionPlan>()
                .HasData(subscriptionPlans);
        }

        /// <summary>
        /// Initialize relationships in the database
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void InitializeRelationShips(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriptionPlan>()
                .HasMany(_ => _.Moderators)
                .WithOne(_ => _.ModeratorSubscriptionPlan)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<SubscriptionPlan>()
                .HasMany(_ => _.Modules)
                .WithOne(_ => _.SubscriptionPlan)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Moderator>()
                .HasMany(_ => _.Hubs)
                .WithOne(_ => _.Moderator)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendee>()
                .HasOne(_ => _.Hub)
                .WithMany(_ => _.Attendees)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendee>()
                .HasOne(_ => _.Pupil)
                .WithMany(_ => _.Attendees)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AvailableModule>()
                .HasOne(_ => _.Hub)
                .WithMany(_ => _.AvailableModules)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AvailableModule>()
                .HasOne(_ => _.Module)
                .WithMany(_ => _.AvailableModules)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Topic>()
                .HasOne(_ => _.Tag)
                .WithMany(_ => _.Topics)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Topic>()
                .HasOne(_ => _.Module)
                .WithMany(_ => _.Topics)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resource>()
                .HasOne(_ => _.Module)
                .WithMany(_ => _.Resources)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<State>()
                .HasOne(_ => _.Resource)
                .WithMany(_ => _.States)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<State>()
                .HasOne(_ => _.Attendee)
                .WithMany(_ => _.States)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CurrentModule>()
                .HasOne(_ => _.Attendee)
                .WithMany(_ => _.CurrentModules);

            modelBuilder.Entity<CurrentModule>()
                .HasOne(_ => _.Module)
                .WithMany(_ => _.CurrentModules);
        }

        /// <summary>
        /// Create the indexes on the database
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void CreateIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Moderator>()
                .HasIndex(_ => _.ModeratorNickname)
                .HasName("index_moderator_nickname");

            modelBuilder.Entity<Moderator>()
                .HasIndex(_ => _.ModeratorEmail)
                .HasName("index_moderator_email");

            modelBuilder.Entity<Pupil>()
                .HasIndex(_ => _.PupilNickname)
                .HasName("index_pupil_nickname");

            modelBuilder.Entity<Pupil>()
                .HasIndex(_ => _.PupilEmail)
                .HasName("index_pupil_email");

            modelBuilder.Entity<Hub>()
                .HasIndex(_ => _.HubLink)
                .HasName("index_hub_link");

            modelBuilder.Entity<Tag>()
                .HasIndex(_ => _.Name)
                .HasName("index_tag_name");
        }
    }
}
