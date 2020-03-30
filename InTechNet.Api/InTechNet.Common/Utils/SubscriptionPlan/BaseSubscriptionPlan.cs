namespace InTechNet.Common.Utils.SubscriptionPlan
{
    /// <summary>
    /// Abstract class for all subscription plans
    /// </summary>
    public abstract class BaseSubscriptionPlan
    {
        /// <summary>
        /// Maximum number of attendees per hub
        /// </summary>
        public abstract int MaxAttendeesPerHubCount { get; }

        /// <summary>
        /// Maximum number of hubs allowed
        /// </summary>
        public abstract int MaxHubsCount { get; }

        /// <summary>
        ///  Maximum number of module allowed
        /// </summary>
        public abstract int MaxModulePerHub { get; }

        /// <summary>
        /// Monthly price of the subscription plan
        /// </summary>
        public abstract decimal Price { get; }

        /// <summary>
        /// Name of the subscription plan
        /// </summary>
        public abstract string SubscriptionPlanName { get; }
    }
}
