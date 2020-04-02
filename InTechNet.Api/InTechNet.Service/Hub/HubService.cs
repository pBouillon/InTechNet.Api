using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User.Attendee;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.DataAccessLayer;
using InTechNet.Exception.Authentication;
using InTechNet.Exception.Hub;
using InTechNet.Exception.Registration;
using InTechNet.Services.Attendee.Interfaces;
using InTechNet.Services.Hub.Helpers;
using InTechNet.Services.Hub.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using InTechNet.Exception.Attendee;

namespace InTechNet.Services.Hub
{
    /// <inheritdoc cref="IHubService" />
    public class HubService : IHubService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly InTechNetContext _context;

        /// <summary>
        /// Attendee service
        /// </summary>
        private readonly IAttendeeService _attendeeService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="attendeeService">Attendee service</param>
        public HubService(InTechNetContext context, IAttendeeService attendeeService)
            => (_context, _attendeeService) = (context, attendeeService);

        /// <inheritdoc cref="IHubService.CreateHub" />
        public void CreateHub(ModeratorDto moderatorDto, HubCreationDto newHubDto)
        {
            // Retrieve the associated moderator to `moderatorDto`
            var moderator = _context.Moderators.FirstOrDefault(
                                _ => _.IdModerator == moderatorDto.Id)
                            ?? throw new UnknownUserException();

            // Assert that this moderator does not have a hub of the same name
            var isDuplicateTracked = _context.Hubs.Any(_ =>
                _.Moderator.IdModerator == moderator.IdModerator
                && _.HubName == newHubDto.Name);

            if (isDuplicateTracked)
            {
                throw new DuplicatedIdentifierException();
            }

            // Generate a unique link for this hub
            var hubLinkGenerated = HubLinkHelper.GenerateLink(newHubDto, moderatorDto);

            // Record the new hub
            _context.Hubs.Add(new DataAccessLayer.Entities.Hub
            {
                HubName = newHubDto.Name,
                HubLink = hubLinkGenerated,
                HubCreationDate = DateTime.Now,
                Moderator = moderator,
                HubDescription = newHubDto.Description,
                Attendees = new List<DataAccessLayer.Entities.Attendee>()
            });

            _context.SaveChanges();
        }

        /// <inheritdoc cref="IHubService.DeleteHub" />
        public void DeleteHub(ModeratorDto moderatorDto, int hubId)
        {
            // Retrieve the associated moderator to `moderatorDto`
            var moderator = _context.Moderators.FirstOrDefault(_ =>
                                _.IdModerator == moderatorDto.Id)
                            ?? throw new UnknownUserException();

            // Retrieve the current hub
            var hub = _context.Hubs.FirstOrDefault(_ =>
                          _.IdHub == hubId)
                      ?? throw new UnknownHubException();

            // Assert that the moderator is allowed to delete this hub
            if (moderator.IdModerator != hub.Moderator.IdModerator)
            {
                throw new IllegalHubOperationException();
            }

            // Deleting the hub
            _context.Hubs.Remove(hub);

            _context.SaveChanges();
        }

        /// <inheritdoc cref="IHubService.GetModeratorHub" />
        public HubDto GetModeratorHub(ModeratorDto moderatorDto, int hubId)
        {
            try
            {
                // Retrieve the requested hub
                var hub = _context.Hubs
                    .Include(_ => _.Moderator)
                    .Include(_ => _.Attendees)
                        .ThenInclude(_ => _.Pupil)
                    .Single(_ =>
                        _.IdHub == hubId 
                        && _.Moderator.IdModerator == moderatorDto.Id);

                // Retrieve the attending pupils from the junction table `Attendee`
                var hubAttendees = hub.Attendees?.Join(
                    _context.Pupils,
                    attendee => attendee.IdPupil,
                    pupil => pupil.IdPupil,
                    (_, pupil) => new LightweightPupilDto
                    {
                        Nickname = pupil.PupilNickname,
                        Id = pupil.IdPupil
                    }).ToList() ?? new List<LightweightPupilDto>();

                // Return the agglomerated data
                return new HubDto
                {
                    IdModerator = hub.Moderator.IdModerator,
                    Attendees = hubAttendees,
                    Description = hub.HubDescription,
                    Id = hub.IdHub,
                    Name = hub.HubName,
                    Link = hub.HubLink,
                };
            }
            catch (InvalidOperationException ex)
            {
                throw new UnknownHubException(ex);
            }
        }

        /// <inheritdoc cref="IHubService.GetModeratorHubs" />
        public IEnumerable<LightweightHubDto> GetModeratorHubs(ModeratorDto moderatorDto)
        {
            var moderatorsHubs = _context.Hubs.Where(_ =>
                    _.Moderator.IdModerator == moderatorDto.Id);

            return moderatorsHubs.Select(_ => new LightweightHubDto
            {
                Name = _.HubName,
                Link = _.HubLink,
                Id = _.IdHub,
                Description = _.HubDescription,
                AttendeesCount = _.Attendees.Count()
            });
        }

        /// <inheritdoc cref="IHubService.GetPupilHub" />
        public HubDto GetPupilHub(PupilDto currentPupil, int hubId)
        {
            try
            {
                // Retrieve the requested hub
                var hub = _context.Hubs
                    .Include(_ => _.Moderator)
                    .Include(_ => _.Attendees)
                        .ThenInclude(_ => _.Pupil)
                    .Single(_ =>
                        _.IdHub == hubId
                        && _.Attendees.Any(_ => 
                            _.IdHub == hubId 
                            && _.IdPupil == currentPupil.Id));

                // Retrieve the attending pupils from the junction table `Attendee`
                var hubAttendees = hub.Attendees?.Join(
                    _context.Pupils,
                    attendee => attendee.IdPupil,
                    pupil => pupil.IdPupil,
                    (_, pupil) => new LightweightPupilDto
                    {
                        Nickname = pupil.PupilNickname,
                        Id = pupil.IdPupil
                    }).ToList() ?? new List<LightweightPupilDto>();

                // Return the agglomerated data
                return new HubDto
                {
                    IdModerator = hub.Moderator.IdModerator,
                    Attendees = hubAttendees,
                    Description = hub.HubDescription,
                    Id = hub.IdHub,
                    Name = hub.HubName,
                    Link = hub.HubLink,
                };
            }
            catch (InvalidOperationException ex)
            {
                throw new UnknownHubException(ex);
            }
        }

        /// <inheritdoc cref="IHubService.GetPupilHubs" />
        public IEnumerable<PupilHubDto> GetPupilHubs(PupilDto currentPupil)
        {
            var pupilsAttendance = _context.Attendees.Include(_ => _.Hub)
                .Where(_ =>
                    _.IdPupil == currentPupil.Id);

            return pupilsAttendance.Select(_ => new PupilHubDto
            {
                Id = _.IdHub,
                Description = _.Hub.HubDescription,
                Name = _.Hub.HubName,
                ModeratorNickname = _.Hub.Moderator.ModeratorNickname
            });
        }

        /// <inheritdoc cref="IHubService.RemoveAttendance" />
        public void RemoveAttendance(ModeratorDto currentModerator, AttendeeDto attendeeDto)
        {
            // Fetching the exact record to be removed
            var attendee = _context.Attendees.FirstOrDefault(_ => 
                    _.IdHub == attendeeDto.IdHub && _.IdPupil == attendeeDto.IdPupil)
                ?? throw new UnknownAttendeeException();

            attendeeDto.Id = attendee.IdAttendee;

            // Remove the attendance
            _attendeeService.RemoveAttendance(attendeeDto);
        }

        /// <inheritdoc cref="IHubService.RemoveAttendance" />
        public void RemoveAttendance(PupilDto currentPupil, AttendeeDto attendeeDto)
        {
            // Fetch the related hub attended by the current moderator
            var attendee = _context.Attendees.FirstOrDefault(_ =>
                    _.IdHub == attendeeDto.IdHub && _.IdPupil == attendeeDto.IdPupil)
                ?? throw new UnknownAttendeeException();

            attendeeDto.Id = attendee.IdAttendee;

            // Remove the attendance
            _attendeeService.RemoveAttendance(attendeeDto);
        }

        /// <inheritdoc cref="IHubService.UpdateHub" />
        public void UpdateHub(ModeratorDto moderatorDto, int hubId, HubUpdateDto hubUpdateDto)
        {
            // Retrieve the associated moderator to `moderatorDto`
            var moderator = _context.Moderators.FirstOrDefault(_ =>
                                _.IdModerator == moderatorDto.Id)
                            ?? throw new UnknownUserException();

            // Retrieve the current hub
            var hub = _context.Hubs.FirstOrDefault(_ =>
                          _.IdHub == hubId)
                      ?? throw new UnknownHubException();

            // Assert that the moderator is allowed to update this hub
            if (moderator.IdModerator != hub.Moderator.IdModerator)
            {
                throw new IllegalHubOperationException();
            }

            // Assert that the name is unique
            var moderatorHubs = GetModeratorHubs(moderatorDto);

            if (moderatorHubs.Any(_ => 
                _.Name == hubUpdateDto.Name
                && _.Id != hubId))
            {
                throw new DuplicatedHubNameException();
            }

            // Update hub information
            hub.HubDescription = hubUpdateDto.Description;
            hub.HubName = hubUpdateDto.Name;

            // Deleting the hub
            _context.Hubs.Update(hub);

            _context.SaveChanges();
        }
    }
}
