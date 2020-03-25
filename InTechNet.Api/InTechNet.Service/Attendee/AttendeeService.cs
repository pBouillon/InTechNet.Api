using InTechNet.Common.Dto.User.Attendee;
using InTechNet.DataAccessLayer;
using InTechNet.Exception.Attendee;
using InTechNet.Services.Attendee.Interfaces;
using System.Linq;

namespace InTechNet.Services.Attendee
{
    /// <inheritdoc cref="IAttendeeService"/>
    public class AttendeeService : IAttendeeService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly InTechNetContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public AttendeeService(InTechNetContext context)
            => _context = context;

        /// <inheritdoc cref="IAttendeeService.RemoveAttendance"/>
        public void RemoveAttendance(AttendeeDto attendeeDto)
        {
            // Retrieve the connection
            var attendee = _context.Attendees
                .FirstOrDefault(_ => _.IdAttendee == attendeeDto.Id)
                ?? throw new UnknownAttendeeException();

            // Remove the connection
            _context.Attendees.Remove(attendee);
            _context.SaveChanges();
        }
    }
}
