using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTechNet.DataAccessLayer.Entities.Modules
{
    [Table("topic", Schema = "public")]
    public class Topic
    {
        /// <summary>
        /// Unique ID of the topic
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Module that has this tag
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// The tag this module has
        /// </summary>
        public Tag Tag { get; set; }
    }
}
