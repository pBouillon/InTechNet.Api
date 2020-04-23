using InTechNet.Common.Utils.SubscriptionPlan;
using InTechNet.DataAccessLayer.Entities.Hubs;
using InTechNet.DataAccessLayer.Entities.Modules;
using InTechNet.DataAccessLayer.Entities.Resources;
using InTechNet.DataAccessLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using InTechNet.DataAccessLayer.Context;

namespace InTechNet.DataAccessLayer
{
    public class InTechNetContext : DbContext, IInTechNetContext
    {
        /// <inheritdoc cref="IInTechNetContext.Moderators"/>
        public DbSet<Moderator> Moderators { get; set; }

        /// <inheritdoc cref="IInTechNetContext.Pupils"/>
        public DbSet<Pupil> Pupils { get; set; }

        /// <inheritdoc cref="IInTechNetContext.Hubs"/>
        public DbSet<Hub> Hubs { get; set; }

        /// <inheritdoc cref="IInTechNetContext.Attendees"/>
        public DbSet<Attendee> Attendees { get; set; }

        /// <inheritdoc cref="IInTechNetContext.SubscriptionPlans"/>
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

        /// <inheritdoc cref="IInTechNetContext.Modules"/>
        public DbSet<Module> Modules { get; set; }

        /// <inheritdoc cref="IInTechNetContext.AvailableModules"/>
        public DbSet<AvailableModule> AvailableModules { get; set; }

        /// <inheritdoc cref="IInTechNetContext.Tags"/>
        public DbSet<Tag> Tags { get; set; }

        /// <inheritdoc cref="IInTechNetContext.Topics"/>
        public DbSet<Topic> Topics { get; set; }

        /// <inheritdoc cref="IInTechNetContext.Resources"/>
        public DbSet<Resource> Resources { get; set; }

        /// <inheritdoc cref="IInTechNetContext.States"/>
        public DbSet<State> States { get; set; }

        /// <inheritdoc cref="IInTechNetContext.CurrentModules"/>
        public DbSet<CurrentModule> CurrentModules { get; set; }

        /// <summary>
        /// Basic constructor for InTechNetContext
        /// </summary>
        // ReSharper disable once SuggestBaseTypeForParameter
        public InTechNetContext(DbContextOptions<InTechNetContext> options)
            : base(options) { }

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
                Id = ++subscriptionId,
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
                Id = ++subscriptionId,
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
                Id = ++subscriptionId,
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
                .WithMany(_ => _.CurrentModules)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CurrentModule>()
                .HasOne(_ => _.Module)
                .WithMany(_ => _.CurrentModules)
                .OnDelete(DeleteBehavior.Cascade);
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
