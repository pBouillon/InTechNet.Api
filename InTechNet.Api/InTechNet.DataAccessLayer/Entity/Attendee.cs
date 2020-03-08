using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InTechNet.DataAccessLayer.Entity
{
    [Table("attendee", Schema = "public")]
    public class Attendee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAttendee { get; set; }

        public int IdPupil { get; set; }
        public Pupil Pupil{ get; set; }

        public int IdHub { get; set; }
        public Hub Hub { get; set; }
    }
}
