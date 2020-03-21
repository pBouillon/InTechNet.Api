using System.ComponentModel.DataAnnotations;

namespace InTechNet.Common.Dto.User.Moderator
{
    public class NicknameDuplicationCheckDto
    {
        /// <summary>
        /// Nickname provided by the user
        /// </summary>
        [Required]
        public string Nickname { get; set; }
    }
}
