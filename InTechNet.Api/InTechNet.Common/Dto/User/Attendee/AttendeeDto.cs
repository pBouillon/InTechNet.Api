namespace InTechNet.Common.Dto.User.Attendee
{
    /// <summary>
    /// <see cref="Attendee"/> DTO
    /// </summary>
    public class AttendeeDto
    {
        /// <summary>
        /// ID of this record in the junction table
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the pupil attending this hub
        /// </summary>
        public int IdPupil { get; set; }

        /// <summary>
        /// Id of the hub
        /// </summary>
        public int IdHub { get; set; }
    }
}
