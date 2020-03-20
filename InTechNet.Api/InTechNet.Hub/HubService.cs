using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User.Attendee;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.DataAccessLayer;
using InTechNet.DataAccessLayer.Entities;
using InTechNet.Exception.Authentication;
using InTechNet.Exception.Hub;
using InTechNet.Exception.Registration;
using InTechNet.Service.Hub.Helpers;
using InTechNet.Service.Hub.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InTechNet.Service.Hub
{
    /// <inheritdoc cref="IHubService" />
    public class HubService : IHubService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly InTechNetContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public HubService(InTechNetContext context)
            => _context = context;

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
                Attendees = new List<Attendee>()
            });

            _context.SaveChanges();
        }

        /// <inheritdoc cref="IHubService.DeleteHub" />
        public void DeleteHub(ModeratorDto moderatorDto, HubDeletionDto hubDeletionData)
        {
            // Retrieve the associated moderator to `moderatorDto`
            var moderator = _context.Moderators.FirstOrDefault(_ =>
                                _.IdModerator == moderatorDto.Id)
                            ?? throw new UnknownUserException();

            // Retrieve the current hub
            var hub = _context.Hubs.FirstOrDefault(_ =>
                          _.IdHub == hubDeletionData.Id)
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

        /// <inheritdoc cref="IHubService.GetModeratorHub"/>
        public HubDto GetModeratorHub(ModeratorDto moderatorDto, int hubId)
        {
            try
            {
                var hub = _context.Hubs.Single(_ =>
                    _.Moderator.IdModerator == moderatorDto.Id
                    && _.IdHub == hubId);

                return new HubDto
                {
                    IdModerator = hub.IdHub,
                    Attendees = hub.Attendees.Select(_ => new AttendeeDto
                    {
                        IdHub = _.IdHub,
                        Id = _.IdAttendee,
                        IdPupil = _.IdPupil
                    }),
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

        /// <inheritdoc cref="IHubService.GetModeratorHubs"/>
        public IEnumerable<LightweightHubDto> GetModeratorHubs(ModeratorDto moderatorDto)
        {
            var moderatorsHubs = _context.Hubs.Where(_ =>
                    _.Moderator.IdModerator == moderatorDto.Id);

            return moderatorsHubs.Select(_ => new LightweightHubDto
            {
                Name = _.HubName,
                Link = _.HubLink,
                Id = _.IdHub,
                AttendeesCount = _.Attendees.Count()
            });
        }
    }
}
