using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InTechNet.DataAccessLayer.Entities.Hubs;

namespace InTechNet.DataAccessLayer.Entities.Modules
{
    [Table("current_module", Schema = "public")]
    public class CurrentModule
    {
        /// <summary>
        /// Unique ID of the current module
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Current module of the attendee
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// The attendee the current module refers to
        /// </summary>
        public Attendee Attendee { get; set; }
    }
}
