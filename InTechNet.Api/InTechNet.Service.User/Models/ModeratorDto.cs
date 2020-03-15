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
        public int Id { get; set; }

        /// <summary>
        /// Nickname of the moderator
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Email of the moderator
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Moderator password
        /// </summary>
        public string Password { get; set; }
    }
}
