using InTechNet.DataAccessLayer.Entities.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InTechNet.DataAccessLayer.Entities.Users;

namespace InTechNet.DataAccessLayer.Entities.Modules
{
    [Table("module", Schema = "public")]
    public class Module
    {
        /// <summary>
        /// Unique ID of the module
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Module description
        /// </summary>
        [MinLength(8)]
        [MaxLength(256)]
        public string ModuleDescription { get; set; }

        /// <summary>
        /// Subscription plan required to see this module
        /// </summary>
        public SubscriptionPlan SubscriptionPlan { get; set; }

        /// <summary>
        /// Name of the module
        /// </summary>
        [MaxLength(32)]
        public string ModuleName { get; set; }

        /// <summary>
        /// References hubs where this module is selected
        /// </summary>
        public IEnumerable<AvailableModule> AvailableModules { get; set; }

        /// <summary>
        /// The topics of this hub
        /// </summary>
        public IEnumerable<Topic> Topics { get; set; }

        /// <summary>
        /// The resources of this hub
        /// </summary>
        public IEnumerable<Resource> Resources { get; set; }

        /// <summary>
        /// The current module of this hub for a pupil
        /// </summary>
        public IEnumerable<CurrentModule> CurrentModules { get; set; }
    }
}
