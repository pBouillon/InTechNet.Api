namespace InTechNet.Common.Dto.Subscription
{
    /// <summary>
    /// Subscription plan details
    /// </summary>
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
        /// Maximum module per hub
        /// </summary>
        public int MaxModulePerHub { get; set; }

        /// <summary>
        /// Maximum number of attendee per hub
        /// </summary>
        public int MaxAttendeesPerHub { get; set; }

        /// <summary>
        /// Price of the subscription plan
        /// </summary>
        public decimal SubscriptionPlanPrice { get; set; }
    }
}
