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
        public int IdTopic { get; set; }

        /// <summary>
        /// Id of the module that has this tag
        /// </summary>
        public int IdModule { get; set; }

        /// <summary>
        /// Module that has this tag
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// Id of the tag this module has
        /// </summary>
        public int IdTag { get; set; }

        /// <summary>
        /// The tag this module has
        /// </summary>
        public Tag Tag { get; set; }
    }
}
