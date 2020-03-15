using InTechNet.DataAccessLayer.Entity;

namespace InTechNet.Service.User.Models
{
    /// <summary>
    /// <see cref="Moderator" /> DTO
    /// </summary>
    public class ModeratorDto
    {
        /// <summary>
        /// Database ID
        /// </summary>
        public int IdModerator { get; set; }

        /// <summary>
        /// Nickname of the moderator
        /// </summary>
        public string ModeratorNickname { get; set; }

        /// <summary>
        /// Email of the moderator
        /// </summary>
        public string ModeratorEmail { get; set; }
    }
}
