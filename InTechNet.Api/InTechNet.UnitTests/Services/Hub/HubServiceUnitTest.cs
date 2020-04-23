using AutoFixture;
using InTechNet.DataAccessLayer.Context;
using InTechNet.DataAccessLayer.Entities.Hubs;
using InTechNet.Services.Attendee.Interfaces;
using InTechNet.Services.Hub;
using InTechNet.UnitTests.Extensions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xbehave;
using InTechNetHubs = InTechNet.DataAccessLayer.Entities.Hubs;
using InTechNetUsers = InTechNet.DataAccessLayer.Entities.Users;

namespace InTechNet.UnitTests.Services.Hub
{
    /// <summary>
    /// HubServiceUnit testing methods
    /// </summary>
    public class HubServiceUnitTest
    {
        /// <summary>
        /// Fixture object for dummy test data
        /// </summary>
        private readonly Fixture _fixture = new Fixture();

        /// <summary>
        /// Collection of Attendee objects representing the database
        /// </summary>
        private ICollection<Attendee> _attendees;

        /// <summary>
        /// InTechNet context mock
        /// </summary>
        private Mock<IInTechNetContext> _context;

        /// <summary>
        /// Collection of Hub objects representing the database
        /// </summary>
        private ICollection<InTechNetHubs.Hub> _hubs;

        /// <summary>
        /// Hub service
        /// </summary>
        private HubService _hubService;

        /// <summary>
        /// Collection of Moderator objects representing the database
        /// </summary>
        private ICollection<InTechNetUsers.Moderator> _moderators;

        /// <summary>
        /// Collection of Pupil objects representing the database
        /// </summary>
        private ICollection<InTechNetUsers.Pupil> _pupils;

        /// <summary>
        /// Default constructor to setup AutoFixture behavior
        /// </summary>
        public HubServiceUnitTest()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(_
                    => _fixture.Behaviors.Remove(_));

            _fixture.Behaviors
                .Add(new OmitOnRecursionBehavior());
        }

        /// <summary>
        /// The background method is executed exactly once before each scenario
        /// </summary>
        [Background]
        public void Background()
        {
            "Given an empty collection of attendees"
                .x(() => _attendees = new List<Attendee>());

            "And an empty collection of hubs"
                .x(() => _hubs = new List<InTechNetHubs.Hub>());

            "And various moderators"
                .x(()
                    => _moderators = _fixture.CreateMany<InTechNetUsers.Moderator>()
                        .ToList());

            "And various pupils"
                .x(()
                    => _pupils = _fixture.CreateMany<InTechNetUsers.Pupil>()
                        .ToList());

            "And a database using them as records"
                .x(() =>
                {
                    _context = new Mock<IInTechNetContext>();

                    // Setup Attendees property
                    var attendeeDbSet = _attendees.AsMockedDbSet();

                    _context.SetupGet(_ => _.Attendees)
                        .Returns(attendeeDbSet.Object.AsMockedDbSet().Object);

                    _context.Setup(_ => _.Attendees.Add(It.IsAny<Attendee>()))
                        .Callback<Attendee>(entity => _attendees.Add(entity));

                    _context.Setup(_ => _.Attendees.Remove(It.IsAny<Attendee>()))
                        .Callback<Attendee>(entity => _attendees.Remove(entity));

                    // Setup Hubs property
                    var hubDbSet = _hubs.AsMockedDbSet();

                    _context.SetupGet(_ => _.Hubs)
                        .Returns(hubDbSet.Object.AsMockedDbSet().Object);

                    _context.Setup(_ => _.Hubs.Add(It.IsAny<InTechNetHubs.Hub>()))
                        .Callback<InTechNetHubs.Hub>(entity => _hubs.Add(entity));

                    _context.Setup(_ => _.Hubs.Remove(It.IsAny<InTechNetHubs.Hub>()))
                        .Callback<InTechNetHubs.Hub>(entity => _hubs.Remove(entity));

                    // Setup Moderators property
                    var moderatorDbSet = _moderators.AsMockedDbSet();

                    _context.SetupGet(_ => _.Moderators)
                        .Returns(moderatorDbSet.Object.AsMockedDbSet().Object); 

                    // Setup Pupils property
                    var pupilDbSet = _pupils.AsMockedDbSet();

                    _context.SetupGet(_ => _.Pupils)
                        .Returns(pupilDbSet.Object.AsMockedDbSet().Object);
                });

            "And a subscription plans service"
                .x(() =>
                {
                    var attendeeService = new Mock<IAttendeeService>();
                    _hubService = new HubService(_context.Object, attendeeService.Object);
                });
        }
    }
}
