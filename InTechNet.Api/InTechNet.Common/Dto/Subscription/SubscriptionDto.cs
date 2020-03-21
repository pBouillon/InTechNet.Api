using System;

namespace InTechNet.Common.Dto.Subscription
{
    public class SubscriptionDto
    {
        /// <summary>
        /// Unique ID of the subscription
        /// </summary>
        public int IdSubscription { get; set; }

        /// <summary>
        /// Name of the subscription
        /// </summary>
        public string SubscriptionName { get; set; }

        /// <summary>
        /// Maximum number of hub
        /// </summary>
        public int HubMaxNumber { get; set; }

        /// <summary>
        /// Price of the subscription
        /// </summary>
        public Decimal SubscriptionPrice { get; set; }
    }
}
