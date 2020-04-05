namespace InTechNet.Common.Dto.Subscription
{
    /// <summary>
    /// Lightweight representation of the <see cref="SubscriptionPlanDto"/>
    /// </summary>
    public class LightweightSubscriptionPlanDto
    {
        /// <summary>
        /// Unique ID of the subscription plan
        /// </summary>
        public int IdSubscriptionPlan { get; set; }

        /// <summary>
        /// Name of the subscription plan
        /// </summary>
        public string SubscriptionPlanName { get; set; }
    }
}
