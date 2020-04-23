using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User.Attendee;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.DataAccessLayer;
using InTechNet.DataAccessLayer.Context;
using InTechNet.Exception.Attendee;
using InTechNet.Exception.Authentication;
using InTechNet.Exception.Hub;
using InTechNet.Exception.Registration;
using InTechNet.Services.Attendee.Interfaces;
using InTechNet.Services.Hub.Helpers;
using InTechNet.Services.Hub.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InTechNet.Services.Hub
{
    /// <inheritdoc cref="IHubService" />
    public class HubService : IHubService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IInTechNetContext _context;

        /// <summary>
        /// Attendee service
        /// </summary>
        private readonly IAttendeeService _attendeeService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="attendeeService">Attendee service</param>
        public HubService(IInTechNetContext context, IAttendeeService attendeeService)
            => (_context, _attendeeService) = (context, attendeeService);

        /// <inheritdoc cref="IHubService.CreateHub" />
        public void CreateHub(ModeratorDto moderatorDto, HubCreationDto newHubDto)
        {
            // Retrieve the associated moderator to `moderatorDto`
            var moderator = _context.Moderators
                .Include(_ => _.ModeratorSubscriptionPlan)
                .FirstOrDefault(
                    _ => _.Id == moderatorDto.Id)
                ?? throw new UnknownUserException();

            var moderatorSubscription = moderator.ModeratorSubscriptionPlan;

            // Assert that this moderator does not have a hub of the same name
            var isDuplicateTracked = _context.Hubs.Any(_ =>
                _.Moderator.Id == moderator.Id
                && _.HubName == newHubDto.Name);

            if (isDuplicateTracked)
            {
                throw new DuplicatedIdentifierException();
            }

            // Assert that the moderator does not exceed its allowed hub count
            var ownedHubsCount = _context.Hubs.Select(_ 
                    => _.Moderator.Id == moderator.Id)
                .Count();

            if (ownedHubsCount + 1 >= moderatorSubscription.MaxHubPerModeratorAccount)
            {
                throw new HubMaxCountReachedException();
            }

            // Generate a unique link for this hub
            var hubLinkGenerated = HubLinkHelper.GenerateLink(newHubDto, moderatorDto);

            // Record the new hub
            _context.Hubs.Add(new DataAccessLayer.Entities.Hubs.Hub
            {
                HubName = newHubDto.Name,
                HubLink = hubLinkGenerated,
                HubCreationDate = DateTime.Now,
                Moderator = moderator,
                HubDescription = newHubDto.Description,
                Attendees = new List<DataAccessLayer.Entities.Hubs.Attendee>()
            });

            _context.SaveChanges();
        }

        /// <inheritdoc cref="IHubService.DeleteHub" />
        public void DeleteHub(ModeratorDto moderatorDto, int hubId)
        {
            // Retrieve the associated moderator to `moderatorDto`
            var moderator = _context.Moderators.FirstOrDefault(_ =>
                                _.Id == moderatorDto.Id)
                            ?? throw new UnknownUserException();

            // Retrieve the current hub
            var hub = _context.Hubs.FirstOrDefault(_ =>
                          _.Id == hubId)
                      ?? throw new UnknownHubException();

            // Assert that the moderator is allowed to delete this hub
            if (moderator.Id != hub.Moderator.Id)
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
                        _.Id == hubId 
                        && _.Moderator.Id == moderatorDto.Id);

                // Retrieve the attending pupils from the junction table `Attendee`
                var hubAttendees = hub.Attendees?.Join(
                    _context.Pupils,
                    attendee => attendee.Pupil.Id,
                    pupil => pupil.Id,
                    (_, pupil) => new LightweightPupilDto
                    {
                        Nickname = pupil.PupilNickname,
                        Id = pupil.Id
                    }).ToList() ?? new List<LightweightPupilDto>();

                // Return the agglomerated data
                return new HubDto
                {
                    IdModerator = hub.Moderator.Id,
                    Attendees = hubAttendees,
                    Description = hub.HubDescription,
                    Id = hub.Id,
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
                    _.Moderator.Id == moderatorDto.Id);

            return moderatorsHubs.Select(_ => new LightweightHubDto
            {
                Name = _.HubName,
                Link = _.HubLink,
                Id = _.Id,
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
                        _.Id == hubId
                        && _.Attendees.Any(_ => 
                            _.Hub.Id == hubId 
                            && _.Pupil.Id == currentPupil.Id));

                // Retrieve the attending pupils from the junction table `Attendee`
                var hubAttendees = hub.Attendees?.Join(
                    _context.Pupils,
                    attendee => attendee.Pupil.Id,
                    pupil => pupil.Id,
                    (_, pupil) => new LightweightPupilDto
                    {
                        Nickname = pupil.PupilNickname,
                        Id = pupil.Id
                    }).ToList() ?? new List<LightweightPupilDto>();

                // Return the agglomerated data
                return new HubDto
                {
                    IdModerator = hub.Moderator.Id,
                    Attendees = hubAttendees,
                    Description = hub.HubDescription,
                    Id = hub.Id,
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
                    _.Pupil.Id == currentPupil.Id);

            return pupilsAttendance.Select(_ => new PupilHubDto
            {
                Id = _.Hub.Id,
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
                    _.Hub.Id == attendeeDto.IdHub && _.Pupil.Id == attendeeDto.IdPupil)
                ?? throw new UnknownAttendeeException();

            attendeeDto.Id = attendee.Id;

            // Remove the attendance
            _attendeeService.RemoveAttendance(attendeeDto);
        }

        /// <inheritdoc cref="IHubService.RemoveAttendance" />
        public void RemoveAttendance(PupilDto currentPupil, AttendeeDto attendeeDto)
        {
            // Fetch the related hub attended by the current moderator
            var attendee = _context.Attendees.FirstOrDefault(_ =>
                    _.Hub.Id == attendeeDto.IdHub && _.Pupil.Id == attendeeDto.IdPupil)
                ?? throw new UnknownAttendeeException();

            attendeeDto.Id = attendee.Id;

            // Remove the attendance
            _attendeeService.RemoveAttendance(attendeeDto);
        }

        /// <inheritdoc cref="IHubService.UpdateHub" />
        public void UpdateHub(ModeratorDto moderatorDto, int hubId, HubUpdateDto hubUpdateDto)
        {
            // Retrieve the associated moderator to `moderatorDto`
            var moderator = _context.Moderators.FirstOrDefault(_ =>
                                _.Id == moderatorDto.Id)
                            ?? throw new UnknownUserException();

            // Retrieve the current hub
            var hub = _context.Hubs.FirstOrDefault(_ =>
                          _.Id == hubId)
                      ?? throw new UnknownHubException();

            // Assert that the moderator is allowed to update this hub
            if (moderator.Id != hub.Moderator.Id)
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
