using System.ComponentModel.DataAnnotations;

namespace InTechNet.Common.Dto.User.Moderator
{
    public class EmailDuplicationCheckDto
    {
        /// <summary>
        /// Email provided by the user
        /// </summary>
        [Required]
        public string Email { get; set; }
    }
}
