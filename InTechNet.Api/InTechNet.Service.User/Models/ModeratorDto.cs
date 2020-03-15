using System.ComponentModel.DataAnnotations;
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
        [Required]
        [MaxLength(64)]
        public string Nickname { get; set; }

        /// <summary>
        /// Email of the moderator
        /// </summary>
        [Required]
        [MaxLength(128)] 
        public string Email { get; set; }

        /// <summary>
        /// Moderator password
        /// </summary>
        [Required]
        [MaxLength(64)] 
        public string Password { get; set; }
    }
}
