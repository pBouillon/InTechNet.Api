using System.ComponentModel.DataAnnotations;

namespace InTechNet.Common.Utils.Authentication
{
    /// <summary>
    /// Data Transfer Object holding login information
    /// </summary>
    public class AuthenticationDto
    {
        /// <summary>
        /// Login provided by the user
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Login { get; set; }

        /// <summary>
        /// Password provided by the user
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string Password { get; set; }
    }
}
