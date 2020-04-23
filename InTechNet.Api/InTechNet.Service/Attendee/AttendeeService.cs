using InTechNet.Common.Dto.User.Attendee;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.DataAccessLayer.Context;
using InTechNet.Exception.Attendee;
using InTechNet.Exception.Hub;
using InTechNet.Services.Attendee.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InTechNet.Services.Attendee
{
    /// <inheritdoc cref="IAttendeeService"/>
    public class AttendeeService : IAttendeeService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IInTechNetContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public AttendeeService(IInTechNetContext context)
            => _context = context;

        /// <inheritdoc cref="IAttendeeService.AddAttendee"/>
        public void AddAttendee(PupilDto pupilDto, string link)
        {
            // Get the hub from its link
            var hub = _context.Hubs.Include(_ => _.Attendees)
                    .Include(_ => _.Moderator)
                    .ThenInclude(_ => _.ModeratorSubscriptionPlan)
                    .FirstOrDefault(_ =>
                        _.HubLink == link)
                ?? throw new UnknownHubException();

            // Ensure that the hub has not reached its full capacity
            var capacityMax = hub.Moderator.ModeratorSubscriptionPlan.MaxAttendeesPerHub;

            var isCapacityMaxReached = (hub.Attendees.Count() >= capacityMax);

            if (isCapacityMaxReached)
            {
                throw new HubMaxAttendeeCountReachedException();
            }

            // Check if the pupil is already an attendee of this hub
            var attendeeAlreadyExists = _context.Attendees.Any(_ =>
                    _.Hub.Id == hub.Id && _.Pupil.Id == pupilDto.Id);

            if (attendeeAlreadyExists)
            {
                throw new AttendeeAlreadyRegisteredException();
            }

            // Create the attendee to be added to this hub
            var pupil = _context.Pupils.First(_ =>
                _.Id == pupilDto.Id);

            _context.Attendees.Add(new DataAccessLayer.Entities.Hubs.Attendee
            {
                Hub = hub,
                Pupil = pupil,
            });

            _context.SaveChanges();
        }

        /// <inheritdoc cref="IAttendeeService.RemoveAttendance"/>
        public void RemoveAttendance(AttendeeDto attendeeDto)
        {
            // Retrieve the connection
            var attendee = _context.Attendees
                .FirstOrDefault(_ => _.Id == attendeeDto.Id)
                ?? throw new UnknownAttendeeException();

            // Remove the connection
            _context.Attendees.Remove(attendee);
            _context.SaveChanges();
        }
    }
}
