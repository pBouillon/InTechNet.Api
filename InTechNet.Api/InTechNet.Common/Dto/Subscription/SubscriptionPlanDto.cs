using System;

namespace InTechNet.Common.Dto.Subscription
{
    public class SubscriptionPlanDto
    {
        /// <summary>
        /// Unique ID of the subscription plan
        /// </summary>
        public int IdSubscriptionPlan { get; set; }

        /// <summary>
        /// Name of the subscription plan
        /// </summary>
        public string SubscriptionPlanName { get; set; }

        /// <summary>
        /// Maximum number of hub
        /// </summary>
        public int MaxHubPerModeratorAccount { get; set; }

        /// <summary>
        /// Maximum number of attendee per hub
        /// </summary>
        public string MaxAttendeesPerHub { get; set; }

        /// <summary>
        /// Price of the subscription plan
        /// </summary>
        public Decimal SubscriptionPlanPrice { get; set; }
    }
}
