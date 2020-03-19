using System.ComponentModel.DataAnnotations;

namespace InTechNet.Common.Dto.User.Pupil
{
    public class PupilDto
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
        /// Pupil's JWT
        /// </summary>
        public string Token { get; set; }
    }
}
