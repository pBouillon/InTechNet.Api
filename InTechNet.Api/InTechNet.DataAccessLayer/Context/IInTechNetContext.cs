using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTechNet.DataAccessLayer.Entities.Hubs;
using InTechNet.DataAccessLayer.Entities.Modules;
using InTechNet.DataAccessLayer.Entities.Resources;
using InTechNet.DataAccessLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace InTechNet.DataAccessLayer.Context
{
    public interface IInTechNetContext
    {
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
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// DbSet for the Topic Entity
        /// </summary>
        public DbSet<Topic> Topics { get; set; }

        /// <summary>
        /// DbSet for the Resource Entity
        /// </summary>
        public DbSet<Resource> Resources { get; set; }

        /// <summary>
        /// DbSet for the State Entity
        /// </summary>
        public DbSet<State> States { get; set; }

        /// <summary>
        /// DbSet for the CurrentModule Entity
        /// </summary>
        public DbSet<CurrentModule> CurrentModules { get; set; }

        /// <summary>
        /// Apply the pending changes in the database
        /// </summary>
        /// <returns>The operation's status code</returns>
        int SaveChanges();
    }
}
