
namespace InTechNet.Common.Dto.Attendee
{
    /// <summary>
    /// <see cref="Attendee" /> DTO
    /// </summary>
    public class AttendeeDto
    {
        /// <summary>
        /// Id of the attendee
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id of the attendee's hub
        /// </summary>
        public int HubId { get; set; }

        /// <summary>
        /// Id of the pupil attending the hub
        /// </summary>
        public int PupilId { get; set; }
    }
}
