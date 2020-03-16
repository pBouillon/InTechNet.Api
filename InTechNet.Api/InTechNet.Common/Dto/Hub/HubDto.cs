using System.Collections.Generic;
using InTechNet.Common.Dto.User;

namespace InTechNet.Common.Dto.Hub
{
    /// <summary>
    /// TODO
    /// </summary>
    public class HubDto
    {
        /// <summary>
        /// TODO
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public int IdModerator { get; set; }

        /// <summary>
        /// TODO
        /// </summary>
        public IEnumerable<AttendeeDto> Attendees { get; set; }

    }
}
