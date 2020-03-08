using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InTechNet.DataAccessLayer.Entity
{
    [Table("moderator", Schema = "public")]
    public class Moderator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdModerator{ get; set; }
        public String ModeratorNickname{ get; set; }
        public String ModeratorEmail { get; set; }
        public String ModeratorPassword { get; set; }
        public String ModeratorSalt { get; set; }

        public List<Organisator> Organisators { get; set; }

    }
}
