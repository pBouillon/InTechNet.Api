using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User.Attendee;
using InTechNet.Common.Dto.User.Pupil;

namespace InTechNet.Services.Attendee.Interfaces
{

    /// <summary>
    /// Service for attendee's operations
    /// </summary>
    public interface IAttendeeService
    {
        /// <summary>
        /// Add an attendee to a hub
        /// </summary>
        /// <param name="pupilDto">The pupil that wil be added</param>
        /// <param name="link">The link of the hub</param>
        void AddAttendee(PupilDto pupilDto, string link);

        /// <summary>
        /// Remove the pupil's attendance to a hub
        /// </summary>
        /// <param name="attendeeDto">The attendee connection to remove</param>
        void RemoveAttendance(AttendeeDto attendeeDto);
    }
}
