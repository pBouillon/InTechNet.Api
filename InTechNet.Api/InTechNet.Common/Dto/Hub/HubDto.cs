﻿using InTechNet.Common.Dto.User.Attendee;
using System.Collections.Generic;

namespace InTechNet.Common.Dto.Hub
{
    /// <summary>
    /// <see cref="Hub" /> DTO
    /// </summary>
    public class HubDto
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
        /// Link of the hub, must be unique
        /// </summary>
        /// <remarks>
        /// This will only be the last part of the URI as:
        /// https://[api_url]/hub/[Link]
        /// </remarks>
        public string Link { get; set; }

        /// <summary>
        /// Id of the moderator managing this hub
        /// </summary>
        public int IdModerator { get; set; }

        /// <summary>
        /// Collection of all <see cref="AttendeeDto" /> attending this hub
        /// </summary>
        public IEnumerable<AttendeeDto> Attendees { get; set; }
    }
}
