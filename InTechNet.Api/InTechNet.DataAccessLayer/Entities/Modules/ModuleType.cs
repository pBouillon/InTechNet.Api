using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTechNet.DataAccessLayer.Entities.Modules
{
    [Table("module_type", Schema = "public")]
    public class ModuleType
    {
        /// <summary>
        /// Unique ID of the type fo module
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdModule { get; set; }

        /// <summary>
        /// Type of the module
        /// </summary>
        [MaxLength(64)]
        public string Type { get; set; }

        /// <summary>
        /// Modules that have this type
        /// </summary>
        public IEnumerable<Module> Modules { get; set; }
    }
}
