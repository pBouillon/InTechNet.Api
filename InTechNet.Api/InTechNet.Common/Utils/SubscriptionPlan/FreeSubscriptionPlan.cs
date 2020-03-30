namespace InTechNet.Common.Utils.SubscriptionPlan
{
    /// <summary>
    /// Class for the free subscription plan
    /// </summary>
    public class FreeSubscriptionPlan : BaseSubscriptionPlan
    {
        /// <inheritdoc cref="BaseSubscriptionPlan.MaxAttendeesPerHubCount"/>
        public override int MaxAttendeesPerHubCount
            => 32;

        /// <inheritdoc cref="BaseSubscriptionPlan.MaxHubsCount"/>
        public override int MaxHubsCount
            => 3;

        /// <inheritdoc cref="BaseSubscriptionPlan.MaxModulePerHub"/>
        public override int MaxModulePerHub
            => 3;

        /// <inheritdoc cref="BaseSubscriptionPlan.Price"/>
        public override decimal Price
            => 0.0M;

        /// <inheritdoc cref="BaseSubscriptionPlan.SubscriptionPlanName"/>
        public override string SubscriptionPlanName
            => "Standard";
    }
}
