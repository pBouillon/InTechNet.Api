using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User;
using InTechNet.Service.Hub.Interfaces;
using System.Collections.Generic;
using System.Linq;
using InTechNet.DataAccessLayer;

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
        public void CreateHub(HubDto hubData)
        {
            // TODO: ModeratorService call
            throw new System.NotImplementedException();
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
