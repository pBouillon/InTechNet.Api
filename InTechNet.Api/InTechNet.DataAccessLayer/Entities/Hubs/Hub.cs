using InTechNet.DataAccessLayer.Entities.Modules;
using InTechNet.DataAccessLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InTechNet.DataAccessLayer.Entities.Hubs
{
    [Table("hub", Schema = "public")]
    public class Hub
    {
        /// <summary>
        /// Unique ID of the Hub
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdHub { get; set; }

        /// <summary>
        /// Name of the Hub
        /// </summary>
        public string HubName { get; set; }

        /// <summary>
        /// Link of the Hub
        /// </summary>
        [Index("index_hub_link", IsUnique = true)]
        public string HubLink { get; set; }

        /// <summary>
        /// Description of the Hub
        /// </summary>
        public string HubDescription { get; set; }

        /// <summary>
        /// Creation date of the Hub
        /// </summary>
        public DateTime HubCreationDate { get; set; }

        /// <summary>
        /// Moderator of the Hub
        /// </summary>
        public Moderator Moderator { get; set; }

        /// <summary>
        /// Attendees of the Hub
        /// </summary>
        public IEnumerable<Attendee> Attendees { get; set; }

        /// <summary>
        /// Selected modules of the hub
        /// </summary>
        public IEnumerable<AvailableModule> AvailableModules { get; set; }
    }
}
