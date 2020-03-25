using InTechNet.Common.Dto.User.Attendee;

namespace InTechNet.Services.Attendee.Interfaces
{
    /// <summary>
    /// Service for attendee's operations
    /// </summary>
    public interface IAttendeeService
    {
        /// <summary>
        /// Remove the pupil's attendance to a hub
        /// </summary>
        /// <param name="attendeeDto">The attendee connection to remove</param>
        void RemoveAttendance(AttendeeDto attendeeDto);
    }
}
