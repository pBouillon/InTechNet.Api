using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTechNet.DataAccessLayer.Entity
{
    [Table("organisator", Schema = "public")]
    public class Organisator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdOrganisator { get; set; }
        
        public int IdModerator { get; set; }
        public Moderator Moderator { get; set; }

        public int IdHub { get; set; }
        public Hub Hub { get; set; }
    }
}
