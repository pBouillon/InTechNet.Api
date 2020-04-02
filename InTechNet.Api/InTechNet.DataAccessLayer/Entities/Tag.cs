using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTechNet.DataAccessLayer.Entities
{
    [Table("tag", Schema = "public")]
    public class Tag
    {
        /// <summary>
        /// Unique ID of the tag
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTag { get; set; }

        /// <summary>
        /// The name of the tag
        /// </summary>
        [MaxLength(32)]
        public string Name { get; set; }
    }
}
