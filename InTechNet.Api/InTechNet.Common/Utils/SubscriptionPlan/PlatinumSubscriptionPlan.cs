namespace InTechNet.Common.Utils.SubscriptionPlan
{
    /// <summary>
    /// Class for the platinium subscription plan
    /// </summary>
    public class PlatinumSubscriptionPlan : BaseSubscriptionPlan
    {
        /// <inheritdoc cref="BaseSubscriptionPlan.MaxAttendeesPerHubCount"/>
        public override int MaxAttendeesPerHubCount
            => 60;

        /// <inheritdoc cref="BaseSubscriptionPlan.MaxHubsCount"/>
        public override int MaxHubsCount
            => 10;

        /// <inheritdoc cref="BaseSubscriptionPlan.MaxModulePerHub"/>
        public override int MaxModulePerHub
            => 15;

        /// <inheritdoc cref="BaseSubscriptionPlan.Price"/>
        public override decimal Price
            => 10.0M;

        /// <inheritdoc cref="BaseSubscriptionPlan.SubscriptionPlanName"/>
        public override string SubscriptionPlanName
            => "Platinum";
    }
}
