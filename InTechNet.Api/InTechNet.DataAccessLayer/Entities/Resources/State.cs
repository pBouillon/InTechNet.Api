using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InTechNet.DataAccessLayer.Entities.Hubs;

namespace InTechNet.DataAccessLayer.Entities.Resources
{
    [Table("state", Schema = "public")]
    public class State
    {
        /// <summary>
        /// Unique ID of the state
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The resource the pupil is in
        /// </summary>
        public Resource Resource { get; set; }

        /// <summary>
        /// The attendee this state refers to
        /// </summary>
        public Attendee Attendee { get; set; }
    }
}
