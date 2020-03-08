using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTechNet.DataAccessLayer.Entity
{
    [Table("attendee", Schema = "public")]
    public class Attendee
    {
        ///<summary>
        /// Unique ID of the Attendee
        ///</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAttendee { get; set; }

        ///<summary>
        /// Id the pupil for this Hub
        ///</summary>
        public int IdPupil { get; set; }

        ///<summary>
        /// Pupil for this Hub
        ///</summary>
        public Pupil Pupil{ get; set; }

        ///<summary>
        /// Id of the Hub for the pupil
        ///</summary>
        public int IdHub { get; set; }

        ///<summary>
        /// Hub for the pupil
        ///</summary>
        public Hub Hub { get; set; }
    }
}
