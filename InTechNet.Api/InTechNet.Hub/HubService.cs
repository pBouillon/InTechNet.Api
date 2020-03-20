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

        /// <inheritdoc cref="IHubService.GetModeratorHubs" />
        public IEnumerable<HubDto> GetModeratorHubs(int moderatorId)
        {
            return _context.Hubs
                .Select(_ => new HubDto
                {
                    Name = _.HubName,
                    Id = _.IdHub,
                    Attendees = _.Attendees.Select(attendee 
                        => new AttendeeDto
                    {
                            IdHub = attendee.IdHub,
                            Id = attendee.IdAttendee,
                            IdPupil = attendee.IdPupil
                    }),
                    Link = _.HubLink,
                    IdModerator = _.Moderator.IdModerator
                })
                .Where(_ => _.IdModerator == moderatorId);
        }
    }
}
