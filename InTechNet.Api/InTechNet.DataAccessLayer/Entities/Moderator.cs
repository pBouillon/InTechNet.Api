using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTechNet.DataAccessLayer.Entities
{
    [Table("moderator", Schema = "public")]
    public class Moderator
    {
        /// <summary>
        /// Unique ID of the Moderator
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdModerator { get; set; }

        /// <summary>
        /// Nickname of the moderator
        /// </summary>
        [Index("index_moderator_nickname", IsUnique = true)]
        [MaxLength(64)]
        public string ModeratorNickname { get; set; }

        /// <summary>
        /// Email of the moderator
        /// </summary>
        [Index("index_moderator_email", IsUnique = true)]
        [MaxLength(128)]
        [EmailAddress] 
        public string ModeratorEmail { get; set; }

        /// <summary>
        /// Password of the moderator
        /// </summary>
        public string ModeratorPassword { get; set; }

        /// <summary>
        /// Salt of the moderator
        /// </summary>
        public string ModeratorSalt { get; set; }

        /// <summary>
        /// Hubs of this Moderator
        /// </summary>
        public IEnumerable<Hub> Hubs { get; set; }
    }
}
