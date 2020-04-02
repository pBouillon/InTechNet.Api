using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTechNet.DataAccessLayer.Entities
{
    [Table("module", Schema = "public")]
    public class Module
    {

        /// <summary>
        /// Unique ID of the module
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdModule { get; set; }

        /// <summary>
        /// Type of the module
        /// </summary>
        [MaxLength(64)]
        public string ModuleType { get; set; }

        /// <summary>
        /// Name of the module
        /// </summary>
        [MaxLength(32)]
        public string ModuleName { get; set; }
    }
}
