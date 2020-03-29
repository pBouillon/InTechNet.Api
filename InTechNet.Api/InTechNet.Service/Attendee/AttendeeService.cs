using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User.Attendee;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.DataAccessLayer;
using InTechNet.Exception.Attendee;
using InTechNet.Exception.Hub;
using InTechNet.Services.Attendee.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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
                    _.IdHub == hub.IdHub && _.IdPupil == pupilDto.Id);

            if (attendeeAlreadyExists)
            {
                throw new AttendeeAlreadyRegisteredException();
            }

            // Create the attendee to be added to this hub
            var pupil = _context.Pupils.First(_ =>
                _.IdPupil == pupilDto.Id);

            var moderatorDto = new ModeratorDto
            {
                Id = hub.Moderator.IdModerator,
            };

            _context.Attendees.Add(new DataAccessLayer.Entities.Attendee
            {
                Hub = hub,
                IdHub = hub.IdHub,
                Pupil = pupil,
                IdPupil = pupil.IdPupil,
            });

            _context.SaveChanges();
        }

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
