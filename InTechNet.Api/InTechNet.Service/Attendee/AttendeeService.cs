using InTechNet.Common.Dto.Attendee;
using InTechNet.Services.Attendee.Interfaces;
using System;
using InTechNet.DataAccessLayer;

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
        public void RemoveAttendance(AttendeeDto attendee)
        {
            throw new NotImplementedException();
        }
    }
}
