namespace InTechNet.Common.Utils.Api
{
    /// <summary>
    /// References tags for endpoint sorting
    /// </summary>
    public abstract class SwaggerTag
    {
        /// <summary>
        /// Endpoint relative to the authentication process
        /// </summary>
        public const string Authentication = "Authentication";

        /// <summary>
        /// Endpoint relative to the hub's logic
        /// </summary>
        public const string Hub = "Hubs";

        /// <summary>
        /// Endpoint relative to the moderator's logic
        /// </summary>
        public const string Moderator = "Moderators";

        /// <summary>
        /// Endpoint relative to the pupil's logic
        /// </summary>
        public const string Pupil = "Pupils";

        /// <summary>
        /// Endpoint relative to the registration process
        /// </summary>
        public const string Registration = "Registrations";

        /// <summary>
        /// Endpoint relative to the subscription plans available and their process
        /// </summary>
        public const string SubscriptionPlan = "SubscriptionPlans";
    }
}
