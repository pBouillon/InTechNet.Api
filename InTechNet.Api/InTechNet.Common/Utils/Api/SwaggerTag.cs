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
        public const string Hubs = "Hubs";

        /// <summary>
        /// Endpoint relative to the moderator's logic
        /// </summary>
        public const string Moderators = "Moderators";

        /// <summary>
        /// Endpoint relative to the pupil's logic
        /// </summary>
        public const string Pupils = "Pupils";

        /// <summary>
        /// Endpoint relative to the registration process
        /// </summary>
        public const string Registrations = "Registrations";

        /// <summary>
        /// Endpoint relative to the subscription plans available and their process
        /// </summary>
        public const string SubscriptionPlans = "SubscriptionPlans";
    }
}
