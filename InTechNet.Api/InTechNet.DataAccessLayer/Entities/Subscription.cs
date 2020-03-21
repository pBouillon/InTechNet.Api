using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InTechNet.DataAccessLayer.Entities
{
    [Table("subscription", Schema = "public")]
    public class Subscription
    {
        /// <summary>
        /// Unique ID of the subscription
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSubscription { get; set; }

        /// <summary>
        /// Name of the subscription
        /// </summary>
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Maximum number of hub
        /// </summary>
        public int HubMaxNumber{ get; set; }

        /// <summary>
        /// Price of the subscription
        /// </summary>
        public Decimal SubscriptionPrice { get; set; }

        /// <summary>
        /// Maximum number of attendee per hub
        /// </summary>
        public string AttendeeMaxNumber { get; set; }

        /// <summary>
        /// Moderators using this subscription
        /// </summary>
        public IEnumerable<Moderator> Moderators { get; set; }


    }
}
