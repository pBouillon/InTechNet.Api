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
        /// Unique ID of the selected module
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdResource { get; set; }

        /// <summary>
        /// Id of the module this is resource is in
        /// </summary>
        public int IdModule { get; set; }

        /// <summary>
        /// The module this is resource is in
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// Content of this resource
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Id of the resource to display after this one
        /// Null if it is the last resource of a module
        /// </summary>
        public int IdNextResource { get; set; }

        /// <summary>
        /// The resource to display after this one
        /// Null if it is the last resource of a module
        /// </summary>
        public Resource NextResource { get; set; }

        /// <summary>
        /// References the states where this resource is used
        /// </summary>
        public IEnumerable<State> States { get; set; }
    }
}
