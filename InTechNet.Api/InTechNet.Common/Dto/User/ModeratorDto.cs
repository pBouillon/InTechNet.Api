using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InTechNet.Common.Dto.Hub;
using InTechNet.DataAccessLayer.Entity;

namespace InTechNet.Common.Dto.User
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
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Moderator password
        /// </summary>
        [Required]
        [MaxLength(64)] 
        public string Password { get; set; }

        public IEnumerable<HubDto> Hubs { get; set; }
    }
}
