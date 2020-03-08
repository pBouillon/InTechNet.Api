using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InTechNet.DataAccessLayer.Entity
{
    [Table("hub", Schema = "public")]
    public class Hub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdHub { get; set; }
        public String HubName { get; set; }
        public String HubLink { get; set; }
        public DateTime HubCreationDate { get; set; }

        public List<Organisator> Organisators { get; set; }
        public List<Attendee> Attendees { get; set; }

    }
}
