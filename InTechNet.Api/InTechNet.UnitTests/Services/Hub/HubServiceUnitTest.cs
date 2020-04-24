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

        /// <summary>
        /// Check the behavior of the hub service on hub creation when the moderator has reached
        /// its maximum allowed hubs
        /// </summary>
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

        /// <summary>
        /// Check the behavior of the hub service when removing an existing hub
        /// </summary>
        [Scenario]
        public void DeleteHub(ModeratorDto moderator, int hubToDeleteId)
        {
            "Given a moderator"
                .x(() =>
                {
                    var registeredModerator = _fixture.Create<InTechNetUsers.Moderator>();
                    _moderators.Add(registeredModerator);

                    moderator = new ModeratorDto
                    {
                        Id = registeredModerator.Id
                    };
                });

            "And a hub that belongs to it"
                .x(() =>
                {
                    var hub = _fixture.Build<InTechNetHubs.Hub>()
                        .With(_ 
                            => _.Moderator, 
                            _moderators.FirstOrDefault(registered 
                                => registered.Id == moderator.Id))
                        .Create();
                    _hubs.Add(hub);

                    hubToDeleteId = hub.Id;
                });

            "When the moderator delete its hub"
                .x(() => _hubService.DeleteHub(moderator, hubToDeleteId));

            "Then the hub should no longer appear in database"
                .x(() =>
                {
                    _context.Verify(_ => _.SaveChanges(), Times.Once);

                    _hubs.Should().BeEmpty();
                });
        }

        /// <summary>
        /// Check the behavior of the hub service when an unknown user attempt to
        /// remove a hub
        /// </summary>
        [Scenario]
        public void DeleteHubByUnknownModerator(ModeratorDto moderator, Action deleteHubFromUnknownModerator)
        {
            "Given an unknown moderator"
                .x(() =>
                {
                    const int unknownModeratorId = -1;

                    moderator = new ModeratorDto
                    {
                        Id = unknownModeratorId
                    };
                });

            "When it attempts to delete a hub"
                .x(()
                    => deleteHubFromUnknownModerator = ()
                        => _hubService.DeleteHub(moderator, _fixture.Create<int>()));

            "Then the service should throw an exception"
                .x(()
                    => deleteHubFromUnknownModerator.Should()
                        .Throw<UnknownUserException>());

            "And no operation should have been performed on the database"
                .x(() 
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
        }

        /// <summary>
        /// Check the behavior of the hub service when a moderator attempt to delete
        /// a hub that is not its
        /// </summary>
        [Scenario]
        public void DeleteHubThatDoesNotBelongToModerator(ModeratorDto moderator, ModeratorDto otherModerator,
            int moderatorHubId, Action deleteHubFromOtherModerator)
        {
            "Given a moderator"
                .x(() =>
                {
                    var registeredModerator = _fixture.Create<InTechNetUsers.Moderator>();
                    _moderators.Add(registeredModerator);

                    moderator = new ModeratorDto
                    {
                        Id = registeredModerator.Id
                    };
                });

            "And a hub that belongs to it"
                .x(() =>
                {
                    var hub = _fixture.Build<InTechNetHubs.Hub>()
                        .With(_
                                => _.Moderator,
                            _moderators.FirstOrDefault(registered
                                => registered.Id == moderator.Id))
                        .Create();
                    _hubs.Add(hub);

                    moderatorHubId = hub.Id;
                });

            "And another moderator"
                .x(() =>
                {
                    var registeredModerator = _fixture.Create<InTechNetUsers.Moderator>();
                    _moderators.Add(registeredModerator);

                    otherModerator = new ModeratorDto
                    {
                        Id = registeredModerator.Id
                    };
                });

            "When the other moderator attempts to delete the hub of the first one"
                .x(() 
                    => deleteHubFromOtherModerator = () 
                        =>_hubService.DeleteHub(otherModerator, moderatorHubId));

            "Then the service should throw an exception"
                .x(()
                    => deleteHubFromOtherModerator.Should()
                        .Throw<IllegalHubOperationException>());

            "And no operation should have been performed on the database"
                .x(()
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to delete an unknown hub
        /// </summary>
        [Scenario]
        public void DeleteUnknownHub(ModeratorDto moderator, Action deleteUnknownHub)
        {
            "Given a moderator"
                .x(() =>
                {
                    var registeredModerator = _fixture.Create<InTechNetUsers.Moderator>();
                    _moderators.Add(registeredModerator);

                    moderator = new ModeratorDto
                    {
                        Id = registeredModerator.Id
                    };
                });

            "When the moderator delete a hub that does not exists"
                .x(() 
                    => deleteUnknownHub = () 
                        => _hubService.DeleteHub(moderator, _fixture.Create<int>()));

            "Then the service should throw an exception"
                .x(()
                    => deleteUnknownHub.Should()
                        .Throw<UnknownHubException>());

            "And no operation should have been performed on the database"
                .x(()
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving a specific hub belonging to a moderator
        /// </summary>
        [Scenario]
        public void GetModeratorHub()
        {
            "Given a moderator"
                .x(() => { });

            "And a hub that belongs to it"
                .x(() => { });

            "When the moderator requests it"
                .x(() => { });

            "Then it should receive its information"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to retrieve the hub of an unknown user
        /// </summary>
        [Scenario]
        public void GetModeratorHubByUnknownModerator()
        {
            "Given a moderator"
                .x(() => { });

            "And a hub that belongs to it"
                .x(() => { });

            "When an unknown user requests it"
                .x(() => { });

            "Then the system should throw an exception"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to retrieve a hub of another moderator
        /// </summary>
        [Scenario]
        public void GetModeratorHubOfAnotherModerator()
        {
            "Given a moderator"
                .x(() => { });

            "And hub that belongs to it"
                .x(() => { });

            "And another moderator"
                .x(() => { });

            "When the moderator request the hub of another one"
                .x(() => { });

            "Then the system should throw an exception"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving all hubs belonging to a moderator
        /// </summary>
        [Scenario]
        public void GetModeratorHubs()
        {
            "Given a moderator"
                .x(() => { });

            "And several hubs that belongs to it"
                .x(() => { });

            "When the moderator requests all its hubs"
                .x(() => { });

            "Then it should receive them all"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving all hubs using an unknown user 
        /// </summary>
        [Scenario]
        public void GetModeratorHubsByUnknownModerator()
        {
            "Given a moderator"
                .x(() => { });

            "And several hubs that belongs to it"
                .x(() => { });

            "When an unknown moderator request all its hubs"
                .x(() => { });

            "Then the system should throw an exception"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving a specific hub belonging to a pupil
        /// </summary>
        [Scenario]
        public void GetPupilHub()
        {
            "Given a hub"
                .x(() => { });

            "And a pupil attending this hub"
                .x(() => { });

            "When the pupil request this hub"
                .x(() => { });

            "Then it should receive its information"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to retrieve the hub of an unknown user
        /// </summary>
        [Scenario]
        public void GetPupilHubByUnknownPupil()
        {
            "Given a hub"
                .x(() => { });

            "And a pupil attending this hub"
                .x(() => { });

            "When an unknown pupil requests this hub"
                .x(() => { });

            "Then the system should throw an exception"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving a specific hub belonging to a pupil
        /// when the pupil is not attending this hub
        /// </summary>
        [Scenario]
        public void GetPupilHubWhenNotAttended()
        {
            "Given a hub"
                .x(() => { });

            "And a pupil not attending this hub"
                .x(() => { });

            "When the pupil requests this hub"
                .x(() => { });

            "Then the system should throw an exception"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving all hubs belonging to a pupil
        /// </summary>
        [Scenario]
        public void GetPupilHubs()
        {
            "Given a pupil"
                .x(() => { });

            "And several hubs it is attending"
                .x(() => { });

            "When the pupil requests its hubs"
                .x(() => { });

            "Then it should receive their information"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving all hubs using an unknown user 
        /// </summary>
        [Scenario]
        public void GetPupilHubsByUnknownPupil()
        {
            "Given an unknown pupil"
                .x(() => { });

            "When it attempt to retrieve all its hubs"
                .x(() => { });

            "Then the system should throw an exception"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when removing the attendance of a pupil
        /// to a hub
        /// </summary>
        [Scenario]
        public void RemoveAttendance()
        {
            "Given a hub"
                .x(() => { });

            "And a pupil attending this hub"
                .x(() => { });

            "When the pupil remove its attendance from this hub"
                .x(() => { });

            "Then it should no longer be an attendee of this hub"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to remove the attendance of a pupil
        /// from an unknown pupil
        /// </summary>
        [Scenario]
        public void RemoveAttendanceByUnknownPupil()
        {
            "Given a hub"
                .x(() => { });

            "And an unknown pupil"
                .x(() => { });

            "When it attempts to remove its attendance from this hub"
                .x(() => { });

            "Then the system should throw an exception"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to remove the attendance of a pupil
        /// from another pupil
        /// </summary>
        [Scenario]
        public void RemoveAttendanceOfAnotherPupil()
        {
            "Given a hub"
                .x(() => { });

            "And a pupil attending this hub"
                .x(() => { });

            "And another pupil not attending this hub"
                .x(() => { });

            "When it attempt to remove its attendance"
                .x(() => { });

            "Then the system should raise an exception"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when updating the hub's data
        /// </summary>
        [Scenario]
        public void UpdateHub()
        {
            "Given a moderator"
                .x(() => { });

            "And a hub that belongs to it"
                .x(() => { });

            "When it changes the hub's information"
                .x(() => { });

            "Then the hub's information should have changed"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when updating the hub's description
        /// </summary>
        [Scenario]
        public void UpdateHubDescription()
        {
            "Given a moderator"
                .x(() => { });

            "And a hub that belongs to it"
                .x(() => { });

            "When it changes the hub's description"
                .x(() => { });

            "Then the hub's description should have changed"
                .x(() => { });

            "And the name should remain the same"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when updating the hub's name
        /// </summary>
        [Scenario]
        public void UpdateHubName()
        {
            "Given a moderator"
                .x(() => { });

            "And a hub that belongs to it"
                .x(() => { });

            "When it changes the hub's name"
                .x(() => { });

            "Then the hub's name should have changed"
                .x(() => { });

            "And the description should remain the same"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to update the hub's data from
        /// an unknown moderator
        /// </summary>
        [Scenario]
        public void UpdateHubByUnknownModerator()
        {
            "Given a moderator"
                .x(() => { });

            "And a hub that belongs to it"
                .x(() => { });

            "And an unknown moderator"
                .x(() => { });

            "When the unknown moderator attempts to update the hub"
                .x(() => { });

            "Then the system should throw an exception"
                .x(() => { });

            "And not perform any action"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to update the hub's data from
        /// another moderator
        /// </summary>
        [Scenario]
        public void UpdateHubOfAnotherModerator()
        {
            "Given a moderator"
                .x(() => { });

            "And a hub that belongs to it"
                .x(() => { });

            "And another moderator"
                .x(() => { });

            "When the moderator attempts to update the hub of another one"
                .x(() => { });

            "Then ths system should throw an exception"
                .x(() => { });

            "And not perform any action"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to update un unknown hub
        /// </summary>
        [Scenario]
        public void UpdateHubOfUnknownHub()
        {
            "Given a moderator"
                .x(() => { });

            "And an unknown hub"
                .x(() => { });

            "When the moderator attempts to update it"
                .x(() => { });

            "Then the system should throw an exception"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to rename a hub with an existing
        /// name for the current moderator hubs
        /// </summary>
        [Scenario]
        public void UpdateHubWithDuplicatedNames()
        {
            "Given a moderator"
                .x(() => { });

            "And two hubs it owns with different names"
                .x(() => { });

            "When it attempts to update the name of a hub with the other hub's name"
                .x(() => { });

            "Then the system should throw an exception"
                .x(() => { });

            "And not perform any change"
                .x(() => { });
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to rename a hub with an existing
        /// name for other moderators hubs
        /// </summary>
        [Scenario]
        public void UpdateHubWithDuplicatedNamesForOthers()
        {
            "Given a moderator"
                .x(() => { });

            "And a hub that belongs to it"
                .x(() => { });

            "And a hub it does not own"
                .x(() => { });

            "When it attempts to update the name of a hub with the other hub's name"
                .x(() => { });

            "Then the name should have been successfully updated"
                .x(() => { });
        }
    }
}
