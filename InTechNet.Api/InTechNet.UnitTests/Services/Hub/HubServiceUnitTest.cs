using System;
using AutoFixture;
using InTechNet.DataAccessLayer.Context;
using InTechNet.DataAccessLayer.Entities.Hubs;
using InTechNet.Services.Attendee.Interfaces;
using InTechNet.Services.Hub;
using InTechNet.UnitTests.Extensions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Exception.Authentication;
using InTechNet.Exception.Hub;
using InTechNet.Exception.Registration;
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
                .x(() 
                    => _attendees = new List<Attendee>());

            "And an empty collection of hubs"
                .x(() 
                    => _hubs = new List<InTechNetHubs.Hub>());

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
                        .Returns(attendeeDbSet.Object);

                    _context.Setup(_ => _.Attendees.Add(It.IsAny<Attendee>()))
                        .Callback<Attendee>(entity => _attendees.Add(entity));

                    _context.Setup(_ => _.Attendees.Remove(It.IsAny<Attendee>()))
                        .Callback<Attendee>(entity => _attendees.Remove(entity));

                    // Setup Hubs property
                    var hubDbSet = _hubs.AsMockedDbSet();

                    _context.SetupGet(_ => _.Hubs)
                        .Returns(hubDbSet.Object);

                    _context.Setup(_ => _.Hubs.Add(It.IsAny<InTechNetHubs.Hub>()))
                        .Callback<InTechNetHubs.Hub>(entity => _hubs.Add(entity));

                    _context.Setup(_ => _.Hubs.Remove(It.IsAny<InTechNetHubs.Hub>()))
                        .Callback<InTechNetHubs.Hub>(entity => _hubs.Remove(entity));

                    // Setup Moderators property
                    var moderatorDbSet = _moderators.AsMockedDbSet();

                    _context.SetupGet(_ => _.Moderators)
                        .Returns(moderatorDbSet.Object); 

                    // Setup Pupils property
                    var pupilDbSet = _pupils.AsMockedDbSet();

                    _context.SetupGet(_ => _.Pupils)
                        .Returns(pupilDbSet.Object);
                });

            "And a hub service"
                .x(() =>
                {
                    var attendeeService = new Mock<IAttendeeService>();
                    _hubService = new HubService(_context.Object, attendeeService.Object);
                });
        }

        /// <summary>
        /// Check the behavior of the hub service on hub creation
        /// </summary>
        [Scenario]
        public void CreateHub(ModeratorDto moderator, HubCreationDto newHubDto)
        {
            "Given an existing moderator"
                .x(() => 
                {
                    var pickedModeratorIndex = new Random().Next(0, _moderators.Count);
                    var registeredModerator = _moderators.ToList()[pickedModeratorIndex];

                    moderator = new ModeratorDto
                    {
                        Id = registeredModerator.Id
                    };
                });

            "And a new hub to be created"
                .x(() 
                    => newHubDto = _fixture.Create<HubCreationDto>());

            "When the moderator creates its new hub"
                .x(() 
                    => _hubService.CreateHub(moderator, newHubDto));

            "Then it should have been stored"
                .x(() =>
                {
                    _context.Verify(_ => _.SaveChanges(), Times.Once);

                    _hubs.Count.Should()
                        .Be(1);
                });

            "And has the same data as the ones specified"
                .x(() =>
                {
                    var hub = _hubs.ToList()[0];

                    hub.HubDescription
                        .Should()
                        .Be(newHubDto.Description);

                    hub.HubName
                        .Should()
                        .Be(newHubDto.Name);
                });
        }

        [Scenario]
        public void CreateHubWhenMaxHubCountReached(ModeratorDto moderator, HubCreationDto newHubDto,
            Action createHubWhenMaxHubCountReached)
        {
            "Given an existing moderator with a plan allowing only 0 hub"
                .x(() =>
                {
                    var pickedModeratorIndex = new Random().Next(0, _moderators.Count);
                    var registeredModerator = _moderators.ToList()[pickedModeratorIndex];

                    registeredModerator.ModeratorSubscriptionPlan.MaxHubPerModeratorAccount = 0;

                    moderator = new ModeratorDto
                    {
                        Id = registeredModerator.Id
                    };
                });

            "And a new hub to be created"
                .x(() 
                    => newHubDto = _fixture.Create<HubCreationDto>());

            "When the moderator attempt to create a new hub"
                .x(()
                    => createHubWhenMaxHubCountReached = ()
                        => _hubService.CreateHub(moderator, newHubDto));

            "Then the system should throw an exception"
                .x(()
                    => createHubWhenMaxHubCountReached.Should()
                        .Throw<HubMaxCountReachedException>());

            "And no operation should have been performed"
                .x(()
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
        }

        /// <summary>
        /// Check the behavior of the hub service on hub creation from an unknown user
        /// </summary>
        [Scenario]
        public void CreateHubWithAnUnknownModerator(ModeratorDto moderator, HubCreationDto newHubDto,
            Action createHubFromUnknownUser)
        {
            "Given an unknown moderator"
                .x(() =>
                {
                    const int unknownId = -1;

                    moderator = new ModeratorDto
                    {
                        Id = unknownId
                    };
                });

            "And a new hub to be created"
                .x(() 
                    => newHubDto = _fixture.Create<HubCreationDto>());

            "When the moderator attempts to create its new hub"
                .x(() 
                    => createHubFromUnknownUser = ()
                        => _hubService.CreateHub(moderator, newHubDto));

            "Then the system should raise an exception"
                .x(() 
                    => createHubFromUnknownUser.Should()
                        .Throw<UnknownUserException>());

            "And never perform any changes"
                .x(()
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
        }

        /// <summary>
        /// Check the behavior of the hub service on hub creation with a name already in use
        /// by another hub of this moderator
        /// </summary>
        [Scenario]
        public void CreateHubWithDuplicatedName(ModeratorDto moderator, InTechNetHubs.Hub existingHub,
            HubCreationDto newHubDto, Action createHubWithDuplicatedName)
        {
            "Given an existing moderator"
                .x(() =>
                {
                    var pickedModeratorIndex = new Random().Next(0, _moderators.Count);
                    var registeredModerator = _moderators.ToList()[pickedModeratorIndex];

                    moderator = new ModeratorDto
                    {
                        Id = registeredModerator.Id
                    };
                });

            "And an existing hub that belong to it"
                .x(() =>
                {
                    existingHub = _fixture.Build<InTechNetHubs.Hub>()
                        .With(_ 
                            => _.Moderator, _moderators.First(_ 
                                => _.Id == moderator.Id))
                        .Create();

                    _hubs.Add(existingHub);
                });

            "And a new hub to be created with the same name"
                .x(() 
                    => newHubDto = _fixture.Build<HubCreationDto>()
                        .With(_ => _.Name, existingHub.HubName)
                        .Create());

            "When the moderator creates its new hub"
                .x(() 
                    => createHubWithDuplicatedName = () 
                        => _hubService.CreateHub(moderator, newHubDto));

            "Then the trow should throw an exception"
                .x(() 
                    => createHubWithDuplicatedName.Should()
                        .Throw<DuplicatedIdentifierException>());

            "And no operation should have been performed"
                .x(() =>
                {
                    _context.Verify(_ => _.SaveChanges(), Times.Never);

                    // Only the previous hub should have been registered
                    _hubs.Count.Should()
                        .Be(1);
                });
        }

        /// <summary>
        /// Check the behavior of the hub service on hub creation with a name already in use
        /// by another hub of another moderator
        /// </summary>
        [Scenario]
        public void CreateHubWithSameNameAsAnotherHub(ModeratorDto moderator, ModeratorDto otherModerator,
            InTechNetHubs.Hub existingHub, HubCreationDto newHubDto)
        {
            "Given an existing hub belonging to a moderator"
                .x(() =>
                {
                    var existingModeratorIndex = new Random().Next(0, _moderators.Count);
                    var registeredModerator = _moderators.ToList()[existingModeratorIndex];

                    moderator = new ModeratorDto
                    {
                        Id = registeredModerator.Id
                    };

                    existingHub = _fixture.Build<InTechNetHubs.Hub>()
                        .With(_
                            => _.Moderator, registeredModerator)
                        .Create();

                    _hubs.Add(existingHub);
                });

            "And another moderator"
                .x(() =>
                {
                    InTechNetUsers.Moderator registeredModerator;
                    int existingModeratorIndex;
                    do
                    {
                        existingModeratorIndex = new Random().Next(0, _moderators.Count);
                        registeredModerator = _moderators.ToList()[existingModeratorIndex];
                    } while (registeredModerator.Id == moderator.Id);

                    otherModerator = new ModeratorDto
                    {
                        Id = registeredModerator.Id
                    };
                });

            "And a hub with the same name as the one already created"
                .x(()
                    => newHubDto = _fixture.Build<HubCreationDto>()
                        .With(_
                            => _.Name, existingHub.HubName)
                        .Create());

            "When the moderator creates its new hub"
                .x(()
                    => _hubService.CreateHub(otherModerator, newHubDto));

            "It should have been create without any error"
                .x(() =>
                {
                    _context.Verify(_ => _.SaveChanges(), Times.Once);
                    _hubs.Count.Should()
                        .Be(2);
                });
        }
    }
}
