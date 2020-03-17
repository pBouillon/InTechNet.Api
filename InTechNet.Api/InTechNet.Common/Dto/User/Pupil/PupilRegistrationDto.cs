using System.ComponentModel.DataAnnotations;

namespace InTechNet.Common.Dto.User.Pupil
{
    /// <summary>
    /// <see cref="Pupil" /> specialized for pupil registration
    /// </summary>
    public class PupilRegistrationDto
    {
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
    }
}
