using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InTechNet.DataAccessLayer.Entity
{
    [Table("pupil", Schema = "public")]
    public class Pupil
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPupil { get; set; }
        public String PupilNickname { get; set; }
        public String PupilEmail { get; set; }
        public String PupilPassword{ get; set; }
        public String PupilSalt { get; set; }

        public List<Attendee> Attendees { get; set; }
    }
}
