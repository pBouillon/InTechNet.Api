using InTechNet.DataAccessLayer.Entities.Modules;
using InTechNet.DataAccessLayer.Entities.Resources;
using InTechNet.DataAccessLayer.Entities.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTechNet.DataAccessLayer.Entities.Hubs
{
    [Table("attendee", Schema = "public")]
    public class Attendee
    {
        /// <summary>
        /// Unique ID of the Attendee
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Pupil for this Hub
        /// </summary>
        public Pupil Pupil { get; set; }

        /// <summary>
        /// Hub for the pupil
        /// </summary>
        public Hub Hub { get; set; }

        /// <summary>
        /// The states of this attendee in a module
        /// </summary>
        public IEnumerable<State> States { get; set; }

        /// <summary>
        /// The current module of this attendee in a hub
        /// </summary>
        public IEnumerable<CurrentModule> CurrentModules{ get; set; }
    }
}
