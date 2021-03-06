﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InTechNet.DataAccessLayer.Entities.Hubs;

namespace InTechNet.DataAccessLayer.Entities.Users
{
    [Table("pupil", Schema = "public")]
    public class Pupil
    {
        /// <summary>
        /// Unique ID of the Pupil
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Nickname of the Pupil
        /// </summary>
        [Index("index_pupil_nickname", IsUnique = true)]
        [MaxLength(64)] 
        public string PupilNickname { get; set; }

        /// <summary>
        /// Email of the Pupil
        /// </summary>
        [Index("index_pupil_email", IsUnique = true)]
        [MaxLength(128)]
        [EmailAddress] 
        public string PupilEmail { get; set; }

        /// <summary>
        /// Password of the Pupil
        /// </summary>
        public string PupilPassword { get; set; }

        /// <summary>
        /// Salt of the Pupil
        /// </summary>
        public string PupilSalt { get; set; }

        /// <summary>
        /// Hub of the pupil
        /// </summary>
        public IEnumerable<Attendee> Attendees { get; set; }
    }
}
