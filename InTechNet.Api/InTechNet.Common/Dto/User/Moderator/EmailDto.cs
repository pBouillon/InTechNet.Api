using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InTechNet.Common.Dto.User.Moderator
{
    public class EmailDto
    {
        /// <summary>
        /// Login provided by the user
        /// </summary>
        [Required]
        public string Email { get; set; }
    }
}
