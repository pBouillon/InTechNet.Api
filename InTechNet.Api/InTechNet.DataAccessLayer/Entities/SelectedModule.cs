using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTechNet.DataAccessLayer.Entities
{
    [Table("selected_module", Schema = "public")]
    public class SelectedModule
    {
        /// <summary>
        /// Unique ID of the selected module
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSelectedModule { get; set; }

        /// <summary>
        /// Id of the hub the module is selected in
        /// </summary>
        public int IdHub { get; set; }

        /// <summary>
        /// Hub the module is selected in
        /// </summary>
        public Hub Hub { get; set; }

        /// <summary>
        /// Id of the selected module
        /// </summary>
        public int IdModule { get; set; }

        /// <summary>
        /// Module selected
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// The state of the module, wether it is selected or not
        /// True if the module is selected, false otherwise
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
