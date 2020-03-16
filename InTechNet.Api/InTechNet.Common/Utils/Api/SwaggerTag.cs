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
        public const string Hub = "Hub";

        /// <summary>
        /// Endpoint relative to the moderator's logic
        /// </summary>
        public const string Moderator = "Moderator";

        /// <summary>
        /// Endpoint relative to the pupil's logic
        /// </summary>
        public const string Pupil = "Pupil";

        /// <summary>
        /// Endpoint relative to the registration process
        /// </summary>
        public const string Registration = "Registration";
    }
}
