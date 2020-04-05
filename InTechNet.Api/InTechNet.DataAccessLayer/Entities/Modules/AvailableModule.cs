using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InTechNet.DataAccessLayer.Entities.Hubs;

namespace InTechNet.DataAccessLayer.Entities.Modules
{
    /// <summary>
    /// Represent a module selected by the moderator of a hub; which will be available for its attendees
    /// </summary>
    [Table("available_module", Schema = "public")]
    public class AvailableModule
    {
        /// <summary>
        /// Unique ID of the selected module
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Hub the module is selected in
        /// </summary>
        public Hub Hub { get; set; }

        /// <summary>
        /// Module selected
        /// </summary>
        public Module Module { get; set; }
    }
}
