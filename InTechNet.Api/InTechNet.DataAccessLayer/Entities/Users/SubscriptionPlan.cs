using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTechNet.DataAccessLayer.Entities.Users
{
    [Table("subscription_plan", Schema = "public")]
    public class SubscriptionPlan
    {
        /// <summary>
        /// Unique ID of the subscription
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSubscriptionPlan { get; set; }

        /// <summary>
        /// Name of the subscription
        /// </summary>
        public string SubscriptionPlanName { get; set; }

        /// <summary>
        /// Maximum number of hub
        /// </summary>
        public int MaxHubPerModeratorAccount { get; set; }

        /// <summary>
        /// Maximum number of module per hub
        /// </summary>
        public int MaxModulePerHub { get; set; }

        /// <summary>
        /// Price of the subscription
        /// </summary>
        public decimal SubscriptionPlanPrice { get; set; }

        /// <summary>
        /// Maximum number of attendee per hub
        /// </summary>
        public int MaxAttendeesPerHub { get; set; }

        /// <summary>
        /// Moderators using this subscription
        /// </summary>
        public IEnumerable<Moderator> Moderators { get; set; }
    }
}
