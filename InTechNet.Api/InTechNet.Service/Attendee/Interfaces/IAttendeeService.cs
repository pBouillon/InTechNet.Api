using InTechNet.Common.Dto.Attendee;

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
        /// <param name="attendee"></param>
        void RemoveAttendance(AttendeeDto attendee);
    }
}
