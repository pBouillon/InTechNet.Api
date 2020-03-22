using InTechNet.Common.Dto.User.Attendee;

namespace InTechNet.Common.Dto.Hub
{
    /// <summary>
    /// Light version of the <see cref="HubDto"/>
    /// </summary>
    public class LightweightHubDto
    {
        /// <summary>
        /// Id of this hub
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the hub, must be unique
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the hub
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Link of the hub, must be unique
        /// </summary>
        /// <remarks>
        /// This will only be the last part of the URI as:
        /// https://[api_url]/hub/[Link]
        /// </remarks>
        public string Link { get; set; }

        /// <summary>
        /// Number of all <see cref="AttendeeDto" /> attending this hub
        /// </summary>
        public int AttendeesCount { get; set; }
    }
}
