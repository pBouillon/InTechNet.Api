using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InTechNet.DataAccessLayer.Entities.Modules;

namespace InTechNet.DataAccessLayer.Entities.Resources
{
    [Table("resource", Schema = "public")]
    public class Resource
    {
        /// <summary>
        /// Unique ID of the selected resource
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// The module this resource is in
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// Content of this resource
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The resource to display after this one
        /// Null if it is the last resource of a module
        /// </summary>
        public virtual Resource NextResource { get; set; }

        /// <summary>
        /// References the states where this resource is used
        /// </summary>
        public IEnumerable<State> States { get; set; }
    }
}
