namespace InTechNet.Common.Utils.SubscriptionPlan
{
    /// <summary>
    /// Class for the premium subscription plan
    /// </summary>
    public class PremiumSubscriptionPlan : BaseSubscriptionPlan
    {
        /// <inheritdoc cref="BaseSubscriptionPlan.MaxAttendeesPerHubCount"/>
        public override int MaxAttendeesPerHubCount
            => 50;

        /// <inheritdoc cref="BaseSubscriptionPlan.MaxHubsCount"/>
        public override int MaxHubsCount
            => 5;

        /// <inheritdoc cref="BaseSubscriptionPlan.MaxModulePerHub"/>
        public override int MaxModulePerHub
            => 5;

        /// <inheritdoc cref="BaseSubscriptionPlan.Price"/>
        public override decimal Price
            => 5.0M;

        /// <inheritdoc cref="BaseSubscriptionPlan.SubscriptionPlanName"/>
        public override string SubscriptionPlanName
            => "Premium";
    }
}
