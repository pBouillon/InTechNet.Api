using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User;
using InTechNet.Service.Hub.Interfaces;
using System.Collections.Generic;
using System.Linq;
using InTechNet.DataAccessLayer;
using InTechNet.Exception.Registration;
using InTechNet.Service.Hub.Helpers;
using System;
using InTechNet.Exception.Authentication;
using InTechNet.DataAccessLayer.Entity;

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
        /// TODO
        /// </summary>
        /// <param name="context">Database context</param>
        public HubService(InTechNetContext context)
        {
            _context = context;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="hubData"></param>
        public void CreateHub(HubDto newHubData, ModeratorDto moderatorDto)
        {

            var moderator = _context.Moderators.FirstOrDefault(
                _ => _.IdModerator == moderatorDto.Id);

            if (null == moderator)
            {
                throw new UnknownUserException();
            }

            var isDuplicateTracked = _context.Hubs.Any(_ =>
                _.Moderator.IdModerator == moderator.IdModerator
                && _.HubName == newHubData.Name);

            if (isDuplicateTracked)
            {
                throw new DuplicateIdentifierException();
            }

            var hubLinkGenerated = HubLinkHelper.GenerateLink(newHubData, moderatorDto);

            // Record the new hub
            _context.Hubs.Add(new DataAccessLayer.Entity.Hub
            {
                HubName = newHubData.Name,
                HubLink = hubLinkGenerated,
                HubCreationDate = DateTime.Now,
                Moderator = moderator,
                Attendees = new List<Attendee>()
            });

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
