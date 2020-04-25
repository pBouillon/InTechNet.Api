using AutoFixture;
using FluentAssertions;
using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User.Attendee;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.DataAccessLayer.Context;
using InTechNet.DataAccessLayer.Entities.Hubs;
using InTechNet.Exception.Attendee;
using InTechNet.Exception.Authentication;
using InTechNet.Exception.Hub;
using InTechNet.Exception.Registration;
using InTechNet.Services.Attendee.Interfaces;
using InTechNet.Services.Hub;
using InTechNet.UnitTests.Extensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xbehave;
using InTechNetHubs = InTechNet.DataAccessLayer.Entities.Hubs;
using InTechNetUsers = InTechNet.DataAccessLayer.Entities.Users;

namespace InTechNet.UnitTests.Services.Hub
{
    /// <summary>
    /// HubService testing methods
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

                    attendeeService.Setup(_ => _.RemoveAttendance(It.IsAny<AttendeeDto>()))
                        .Callback<AttendeeDto>(entity =>
                        {
                            var hub = _hubs.Single(_ => _.Id == entity.IdHub);
                            hub.Attendees = hub.Attendees.Where(_ => _.Id != entity.Id);
                        });

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
        public void GetModeratorHub(ModeratorDto moderator, HubDto originalHub, HubDto hub)
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
                        .Without(_ => _.Attendees)
                        .With(_ => _.Moderator, _moderators.First(_ => _.Id == moderator.Id))
                        .Create();

                    _hubs.Add(hub);

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = moderator.Id,
                        Attendees = new List<LightweightPupilDto>(),
                        Description = hub.HubDescription,
                        Name = hub.HubName,
                        Link = hub.HubLink
                    };
                });

            "When the moderator requests it"
                .x(()
                    => hub = _hubService.GetModeratorHub(moderator, originalHub.Id));

            "Then it should receive its information"
                .x(() 
                    => hub.Should().BeEquivalentTo(originalHub));
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to retrieve the hub of an unknown user
        /// </summary>
        [Scenario]
        public void GetModeratorHubByUnknownModerator(ModeratorDto moderator, HubDto originalHub,
            ModeratorDto unknownModerator, Action requestHubByUnknownModerator)
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
                        .Without(_ => _.Attendees)
                        .With(_ => _.Moderator, _moderators.First(_ => _.Id == moderator.Id))
                        .Create();

                    _hubs.Add(hub);

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = moderator.Id,
                        Attendees = new List<LightweightPupilDto>(),
                        Description = hub.HubDescription,
                        Name = hub.HubName,
                        Link = hub.HubLink
                    };
                });

            "And an unknown moderator"
                .x(() =>
                {
                    const int unknownModeratorId = -1;

                    unknownModerator = new ModeratorDto
                    {
                        Id = unknownModeratorId
                    };
                });

            "When an unknown user requests it"
                .x(() 
                    => requestHubByUnknownModerator = () 
                        => _hubService.GetModeratorHub(unknownModerator, originalHub.Id));

            "Then the system should throw an exception"
                .x(() 
                    => requestHubByUnknownModerator.Should()
                        .Throw<UnknownUserException>());
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to retrieve a hub of another moderator
        /// </summary>
        [Scenario]
        public void GetModeratorHubOfAnotherModerator(ModeratorDto moderator, HubDto originalHub,
            ModeratorDto otherModerator, Action requestingTheHubOfAnotherModerator)
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
                        .Without(_ => _.Attendees)
                        .With(_ => _.Moderator, _moderators.First(_ => _.Id == moderator.Id))
                        .Create();

                    _hubs.Add(hub);

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = moderator.Id,
                        Attendees = new List<LightweightPupilDto>(),
                        Description = hub.HubDescription,
                        Name = hub.HubName,
                        Link = hub.HubLink
                    };
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

            "When the moderator request the hub of another one"
                .x(() 
                    => requestingTheHubOfAnotherModerator = ()
                        => _hubService.GetModeratorHub(otherModerator, originalHub.Id));

            "Then the system should throw an exception"
                .x(() 
                    => requestingTheHubOfAnotherModerator.Should()
                        .Throw<UnknownHubException>());
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving all hubs belonging to a moderator
        /// </summary>
        [Scenario]
        public void GetModeratorHubs(ModeratorDto moderator, List<LightweightHubDto> originalHubs,
            IEnumerable<LightweightHubDto> hubs)
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

            "And several hubs that belongs to it"
                .x(() =>
                {
                    var hubs = _fixture.Build<InTechNetHubs.Hub>()
                        .With(_ => _.Attendees, new List<Attendee>())
                        .With(_ => _.Moderator, _moderators.Single(_ => _.Id == moderator.Id))
                        .CreateMany();

                    originalHubs = new List<LightweightHubDto>();
                    foreach (var hub in hubs)
                    {
                        _hubs.Add(hub);
                        originalHubs.Add(new LightweightHubDto
                        {
                            Id = hub.Id,
                            Name = hub.HubName,
                            Description = hub.HubDescription,
                            Link = hub.HubLink
                        });
                    }
                });

            "When the moderator requests all its hubs"
                .x(() 
                    => hubs = _hubService.GetModeratorHubs(moderator));

            "Then it should receive them all"
                .x(() =>
                {
                    hubs.Should()
                        .HaveSameCount(originalHubs)
                        .And
                        .OnlyHaveUniqueItems();

                    foreach (var hub in hubs)
                    {
                        originalHubs.Should()
                            .ContainEquivalentOf(hub);
                    }
                });
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving all hubs using an unknown user 
        /// </summary>
        [Scenario]
        public void GetModeratorHubsByUnknownModerator(ModeratorDto moderator, ModeratorDto unknownModerator,
            Action getModeratorHubsFromUnknownModerator)
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

            "And several hubs that belongs to it"
                .x(() =>
                {
                    var hubs = _fixture.Build<InTechNetHubs.Hub>()
                        .Without(_ => _.Attendees)
                        .With(_ 
                            => _.Moderator, 
                            _moderators.Single(_ 
                                => _.Id == moderator.Id))
                        .CreateMany();

                    foreach (var hub in hubs)
                    {
                        _hubs.Add(hub);
                    }
                });

            "And an unknown moderator"
                .x(() =>
                {
                    const int unknownModeratorId = -1;

                    unknownModerator = new ModeratorDto
                    {
                        Id = unknownModeratorId
                    };
                });

            "When an unknown moderator request all its hubs"
                .x(() 
                    => getModeratorHubsFromUnknownModerator = ()
                        => _hubService.GetModeratorHubs(unknownModerator));

            "Then the system should throw an exception"
                .x(() 
                    => getModeratorHubsFromUnknownModerator.Should()
                        .Throw<UnknownUserException>());
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving a specific hub belonging to a pupil
        /// </summary>
        [Scenario]
        public void GetPupilHub(PupilDto pupil, HubDto originalHub, HubDto retrivedHub)
        {
            "Given a pupil"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count);
                    var pickedPupil = _pupils.ToList()[pickedPupilIndex];

                    pupil = new PupilDto
                    {
                        Id = pickedPupil.Id,
                        Nickname = pickedPupil.PupilNickname
                    };
                });

            "And a hub it is attending"
                .x(() =>
                {
                    var attendee = _fixture.Build<Attendee>()
                        .With(_ => _.Pupil, _pupils.Single(_
                            => _.Id == pupil.Id))
                        .Without(_ => _.CurrentModules)
                        .Without(_ => _.States)
                        .Create();

                    var hub = _fixture.Build<InTechNetHubs.Hub>()
                        .With(_ => _.Attendees, new List<Attendee>
                        {
                            attendee
                        })
                        .Create();
                    _hubs.Add(hub);

                    attendee.Hub = hub;
                    _attendees.Add(attendee);

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = hub.Moderator.Id,
                        Name = hub.HubName,
                        Attendees = new List<LightweightPupilDto>
                        {
                          new LightweightPupilDto
                          {
                              Id = pupil.Id,
                              Nickname = pupil.Nickname
                          }
                        },
                        Description = hub.HubDescription,
                        Link = hub.HubLink
                    };
                });

            "When the pupil request this hub"
                .x(() 
                    => retrivedHub = _hubService.GetPupilHub(pupil, originalHub.Id));

            "Then it should receive its information"
                .x(() =>
                    retrivedHub.Should()
                        .BeEquivalentTo(originalHub));
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to retrieve the hub of an unknown user
        /// </summary>
        [Scenario]
        public void GetPupilHubByUnknownPupil(PupilDto unknownPupil, Action requestHubWithUnknownPupil)
        {
            "Given an unknown pupil"
                .x(() =>
                {
                    const int unknownPupilId = -1;

                    unknownPupil = new PupilDto
                    {
                        Id = unknownPupilId
                    };
                });

            "When an unknown pupil requests any hub"
                .x(()
                    => requestHubWithUnknownPupil = ()
                        => _hubService.GetPupilHub(unknownPupil, _fixture.Create<int>()));

            "Then the system should throw an exception"
                .x(() 
                    => requestHubWithUnknownPupil.Should()
                        .Throw<UnknownUserException>());
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving a specific hub belonging to a pupil
        /// when the pupil is not attending this hub
        /// </summary>
        [Scenario]
        public void GetPupilHubWhenNotAttended(InTechNetHubs.Hub hub, PupilDto pupil,
            Action requestHubNotAttended)
        {
            "Given a hub"
                .x(() =>
                {
                    hub = _fixture.Build<InTechNetHubs.Hub>()
                        .With(_ => _.Attendees, new List<Attendee>())
                        .Create();

                    _hubs.Add(hub);
                });

            "And a pupil not attending this hub"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count);
                    var pickedPupil = _pupils.ToList()[pickedPupilIndex];

                    pupil = new PupilDto
                    {
                        Id = pickedPupil.Id,
                        Nickname = pickedPupil.PupilNickname
                    };
                });

            "When the pupil requests this hub"
                .x(() 
                    => requestHubNotAttended = ()
                        => _hubService.GetPupilHub(pupil, hub.Id));

            "Then the system should throw an exception"
                .x(() 
                    => requestHubNotAttended.Should()
                        .Throw<UnknownHubException>());
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving all hubs belonging to a pupil
        /// </summary>
        [Scenario]
        public void GetPupilHubs(PupilDto pupil, List<PupilHubDto> originalHubs,
            IEnumerable<PupilHubDto> hubs)
        {
            "Given a pupil"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count);
                    var pickedPupil = _pupils.ToList()[pickedPupilIndex];

                    pupil = new PupilDto
                    {
                        Id = pickedPupil.Id,
                        Nickname = pickedPupil.PupilNickname,
                    };
                });

            "And several hubs it is attending"
                .x(() =>
                {
                    // Create several attendance for this pupil
                    var attendees = _fixture.Build<Attendee>()
                        .With(_ => _.Pupil, _pupils.Single(_
                            => _.Id == pupil.Id))
                        .Without(_ => _.CurrentModules)
                        .Without(_ => _.States)
                        .CreateMany();

                    originalHubs = new List<PupilHubDto>();
                    foreach (var attendee in attendees)
                    {
                        // Create the hub attended
                        var hub = _fixture.Build<InTechNetHubs.Hub>()
                            .With(_ => _.Attendees, new List<Attendee>
                            {
                                attendee
                            })
                            .Create();
                        _hubs.Add(hub);

                        // Associate it to the attendee
                        attendee.Hub = hub;
                        _attendees.Add(attendee);

                        // Add its description
                        originalHubs.Add(new PupilHubDto
                        {
                            Id = hub.Id,
                            Name = hub.HubName,
                            Description = hub.HubDescription,
                            ModeratorNickname = hub.Moderator.ModeratorNickname
                        });
                    }
                });

            "When the pupil requests its hubs"
                .x(() 
                    => hubs = _hubService.GetPupilHubs(pupil));

            "Then it should receive their information"
                .x(() =>
                {
                    hubs.Should()
                        .HaveSameCount(originalHubs)
                        .And
                        .OnlyHaveUniqueItems();

                    foreach (var hub in hubs)
                    {
                        originalHubs.Should()
                            .ContainEquivalentOf(hub);
                    }
                });
        }

        /// <summary>
        /// Check the behavior of the hub service when retrieving all hubs using an unknown user 
        /// </summary>
        [Scenario]
        public void GetPupilHubsByUnknownPupil(PupilDto unknownPupil, Action retrievingAllHubsOfUnknownPupil)
        {
            "Given an unknown pupil"
                .x(() =>
                {
                    const int unknownPupilId = -1;

                    unknownPupil = new PupilDto
                    {
                        Id = unknownPupilId
                    };
                });

            "When it attempt to retrieve all its hubs"
                .x(()
                    => retrievingAllHubsOfUnknownPupil = ()
                        => _hubService.GetPupilHubs(unknownPupil));

            "Then the system should throw an exception"
                .x(() 
                    => retrievingAllHubsOfUnknownPupil.Should()
                        .Throw<UnknownUserException>());
        }

        /// <summary>
        /// Check the behavior of the hub service when removing the attendance of a pupil
        /// to a hub from a pupil
        /// </summary>
        [Scenario]
        public void RemoveAttendanceFromModerator(ModeratorDto moderator, AttendeeDto attendee, HubDto originalHub)
        {
            "Given a Moderator"
                .x(() =>
                {
                    var pickedModeratorIndex = new Random().Next(0, _moderators.Count);
                    var pickedModerator = _pupils.ToList()[pickedModeratorIndex];

                    moderator = new ModeratorDto
                    {
                        Id = pickedModerator.Id,
                    };
                });

            "And a hub it owns with an attendee"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count);
                    var pickedPupil = _pupils.ToList()[pickedPupilIndex];

                    var originalAttendee = _fixture.Build<Attendee>()
                        .With(_ => _.Pupil, _pupils.Single(_
                            => _.Id == pickedPupil.Id))
                        .Without(_ => _.CurrentModules)
                        .Without(_ => _.States)
                        .Create();

                    var hub = _fixture.Build<InTechNetHubs.Hub>()
                        .With(_ => _.Attendees, new List<Attendee>
                        {
                            originalAttendee
                        })
                        .Create();
                    _hubs.Add(hub);

                    originalAttendee.Hub = hub;
                    _attendees.Add(originalAttendee);

                    attendee = new AttendeeDto
                    {
                        IdHub = hub.Id,
                        IdPupil = pickedPupil.Id,
                        Id = originalAttendee.Id
                    };

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = hub.Moderator.Id,
                        Name = hub.HubName,
                        Attendees = new List<LightweightPupilDto>
                        {
                            new LightweightPupilDto
                            {
                                Id = pickedPupil.Id,
                                Nickname = pickedPupil.PupilNickname
                            }
                        },
                        Description = hub.HubDescription,
                        Link = hub.HubLink
                    };
                });

            "When the moderator remove an attendee from its hub"
                .x(()
                    => _hubService.RemoveAttendance(moderator, attendee));

            "Then it should no longer appear as an attendee of this hub"
                .x(() 
                    => _hubs.Should()
                        .NotContain(_ => _.Id == attendee.Id));
        }

        /// <summary>
        /// Check the behavior of the hub service when removing the attendance of a pupil
        /// to a hub from a pupil
        /// </summary>
        [Scenario]
        public void RemoveAttendanceFromPupil(PupilDto pupil, AttendeeDto attendee, HubDto originalHub)
        {
            "Given a pupil"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count);
                    var pickedPupil = _pupils.ToList()[pickedPupilIndex];

                    pupil = new PupilDto
                    {
                        Id = pickedPupil.Id,
                        Nickname = pickedPupil.PupilNickname
                    };
                });

            "And a hub it is attending"
                .x(() =>
                {
                    var originalAttendee = _fixture.Build<Attendee>()
                        .With(_ => _.Pupil, _pupils.Single(_
                            => _.Id == pupil.Id))
                        .Without(_ => _.CurrentModules)
                        .Without(_ => _.States)
                        .Create();

                    var hub = _fixture.Build<InTechNetHubs.Hub>()
                        .With(_ => _.Attendees, new List<Attendee>
                        {
                            originalAttendee
                        })
                        .Create();
                    _hubs.Add(hub);

                    originalAttendee.Hub = hub;
                    _attendees.Add(originalAttendee);

                    attendee = new AttendeeDto
                    {
                        IdHub = hub.Id,
                        IdPupil = pupil.Id,
                        Id = originalAttendee.Id
                    };

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = hub.Moderator.Id,
                        Name = hub.HubName,
                        Attendees = new List<LightweightPupilDto>
                        {
                            new LightweightPupilDto
                            {
                                Id = pupil.Id,
                                Nickname = pupil.Nickname
                            }
                        },
                        Description = hub.HubDescription,
                        Link = hub.HubLink
                    };
                });

            "When the pupil remove its attendance from this hub"
                .x(() 
                    => _hubService.RemoveAttendance(pupil, attendee));

            "Then it should no longer be an attendee of this hub"
                .x(()
                    => _hubs.Should()
                        .NotContain(_ => _.Id == attendee.Id));
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to remove the attendance of a pupil
        /// from an unknown pupil
        /// </summary>
        [Scenario]
        public void RemoveAttendanceByUnknownAttendee(PupilDto pupil, InTechNetHubs.Hub hub,
            AttendeeDto attendee, Action removeAttendanceOfUnknownAttendee)
        {
            "Given a pupil"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count);
                    var pickedPupil = _pupils.ToList()[pickedPupilIndex];

                    pupil = new PupilDto
                    {
                        Id = pickedPupil.Id
                    };
                });

            "And an existing hub"
                .x(() =>
                {
                    hub = _fixture.Create<InTechNetHubs.Hub>();
                    _hubs.Add(hub);
                });

            "And an unknown attendee"
                .x(() =>
                {
                    const int unknownAttendeeId = -1;
                    attendee = new AttendeeDto
                    {
                        Id = unknownAttendeeId,
                        IdPupil = pupil.Id,
                        IdHub = hub.Id
                    };
                });

            "When it attempts to remove its attendance from this hub"
                .x(() 
                    => removeAttendanceOfUnknownAttendee = ()
                        => _hubService.RemoveAttendance(pupil, attendee));

            "Then the system should throw an exception"
                .x(() 
                    => removeAttendanceOfUnknownAttendee.Should()
                        .Throw<UnknownAttendeeException>());
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to remove the attendance of a pupil
        /// from another pupil
        /// </summary>
        [Scenario]
        public void RemoveAttendanceOfAnotherPupil(PupilDto pupil, PupilDto otherPupil, AttendeeDto attendee,
            HubDto originalHub, Action removingOtherPupilAttendance)
        {
            "Given a pupil"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count);
                    var pickedPupil = _pupils.ToList()[pickedPupilIndex];

                    pupil = new PupilDto
                    {
                        Id = pickedPupil.Id,
                        Nickname = pickedPupil.PupilNickname
                    };
                });

            "And a hub it is attending"
                .x(() =>
                {
                    var originalAttendee = _fixture.Build<Attendee>()
                        .With(_ => _.Pupil, _pupils.Single(_
                            => _.Id == pupil.Id))
                        .Without(_ => _.CurrentModules)
                        .Without(_ => _.States)
                        .Create();

                    var hub = _fixture.Build<InTechNetHubs.Hub>()
                        .With(_ => _.Attendees, new List<Attendee>
                        {
                            originalAttendee
                        })
                        .Create();
                    _hubs.Add(hub);

                    originalAttendee.Hub = hub;
                    _attendees.Add(originalAttendee);

                    attendee = new AttendeeDto
                    {
                        IdHub = hub.Id,
                        IdPupil = pupil.Id,
                        Id = originalAttendee.Id
                    };

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = hub.Moderator.Id,
                        Name = hub.HubName,
                        Attendees = new List<LightweightPupilDto>
                        {
                            new LightweightPupilDto
                            {
                                Id = pupil.Id,
                                Nickname = pupil.Nickname
                            }
                        },
                        Description = hub.HubDescription,
                        Link = hub.HubLink
                    };
                });

            "And another pupil not attending this hub"
                .x(() =>
                {
                    var otherRegisteredPupil = _fixture.Create<InTechNetUsers.Pupil>();
                    _pupils.Add(otherRegisteredPupil);

                    otherPupil = new PupilDto
                    {
                        Id = otherRegisteredPupil.Id
                    };
                });

            "When it attempt to remove its attendance"
                .x(() 
                    => removingOtherPupilAttendance = ()
                        => _hubService.RemoveAttendance(otherPupil, attendee));

            "Then the system should raise an exception"
                .x(() 
                    => removingOtherPupilAttendance.Should()
                        .Throw<UnknownAttendeeException>());
        }

        /// <summary>
        /// Check the behavior of the hub service when updating the hub's data
        /// </summary>
        [Scenario]
        public void UpdateHub(ModeratorDto moderator, HubDto originalHub, HubUpdateDto hubUpdate)
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
                        .Without(_ => _.Attendees)
                        .With(_ 
                            => _.Moderator, 
                            _moderators.First(_ => _.Id == moderator.Id))
                        .Create();

                    _moderators.Single(_ => _.Id == moderator.Id).Hubs = new List<InTechNetHubs.Hub>
                    {
                        hub
                    };

                    _hubs.Add(hub);

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = moderator.Id,
                        Attendees = new List<LightweightPupilDto>(),
                        Description = hub.HubDescription,
                        Name = hub.HubName,
                        Link = hub.HubLink
                    };
                });

            "And new information for this hub"
                .x(()
                    => hubUpdate = _fixture.Create<HubUpdateDto>());
            
            "When it changes the hub's information"
                .x(() 
                    => _hubService.UpdateHub(moderator, originalHub.Id, hubUpdate));

            "Then the hub's information should have changed"
                .x(() =>
                {
                    _context.Verify(_ => _.SaveChanges(), Times.Once);

                    var hub = _hubs.First(_ => _.Id == originalHub.Id);

                    hub.HubDescription.Should()
                        .Be(hubUpdate.Description);

                    hub.HubName.Should()
                        .Be(hubUpdate.Name);
                });
        }

        /// <summary>
        /// Check the behavior of the hub service when updating the hub's description
        /// </summary>
        [Scenario]
        public void UpdateHubDescription(ModeratorDto moderator, HubDto originalHub, HubUpdateDto hubUpdate)
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
                        .Without(_ => _.Attendees)
                        .With(_ => _.Moderator, _moderators.First(_ => _.Id == moderator.Id))
                        .Create();

                    _moderators.Single(_ => _.Id == moderator.Id).Hubs = new List<InTechNetHubs.Hub>
                    {
                        hub
                    };

                    _hubs.Add(hub);

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = moderator.Id,
                        Attendees = new List<LightweightPupilDto>(),
                        Description = hub.HubDescription,
                        Name = hub.HubName,
                        Link = hub.HubLink
                    };
                });

            "And a new description for this hub"
                .x(()
                    => hubUpdate = _fixture.Build<HubUpdateDto>()
                        .With(_ => _.Name, originalHub.Name)
                        .Create());

            "When it changes the hub's description"
                .x(() => _hubService.UpdateHub(moderator, originalHub.Id, hubUpdate));

            "Then the hub's description should have changed"
                .x(() =>
                {
                    _context.Verify(_ => _.SaveChanges(), Times.Once);

                    _hubs.Single(_ => _.Id == originalHub.Id).HubDescription
                        .Should()
                        .Be(hubUpdate.Description);
                });

            "And the name should remain the same"
                .x(() 
                    => _hubs.Single(_ => _.Id == originalHub.Id).HubName
                        .Should()
                        .Be(hubUpdate.Name));
        }

        /// <summary>
        /// Check the behavior of the hub service when updating the hub's name
        /// </summary>
        [Scenario]
        public void UpdateHubName(ModeratorDto moderator, HubDto originalHub, HubUpdateDto hubUpdate)
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
                        .Without(_ => _.Attendees)
                        .With(_ => _.Moderator, _moderators.First(_ => _.Id == moderator.Id))
                        .Create();

                    _moderators.Single(_ => _.Id == moderator.Id).Hubs = new List<InTechNetHubs.Hub>
                    {
                        hub
                    };

                    _hubs.Add(hub);

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = moderator.Id,
                        Attendees = new List<LightweightPupilDto>(),
                        Description = hub.HubDescription,
                        Name = hub.HubName,
                        Link = hub.HubLink
                    };
                });

            "And a new name for this hub"
                .x(()
                    => hubUpdate = _fixture.Build<HubUpdateDto>()
                        .With(_ => _.Description, originalHub.Description)
                        .Create());

            "When it changes the hub's description"
                .x(() => _hubService.UpdateHub(moderator, originalHub.Id, hubUpdate));

            "Then the hub's name should have changed"
                .x(() =>
                {
                    _context.Verify(_ => _.SaveChanges(), Times.Once);

                    _hubs.Single(_ => _.Id == originalHub.Id).HubName
                        .Should()
                        .Be(hubUpdate.Name);
                });

            "And the description should remain the same"
                .x(()
                    => _hubs.Single(_ => _.Id == originalHub.Id).HubDescription
                        .Should()
                        .Be(hubUpdate.Description));
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to update the hub's data from
        /// an unknown moderator
        /// </summary>
        [Scenario]
        public void UpdateHubByUnknownModerator(ModeratorDto moderator, Action updateHubByUnknownModerator)
        {
            "Given an unknown moderator"
                .x(() =>
                {
                    const int unknownModeratorId  = -1;

                    moderator = new ModeratorDto
                    {
                        Id = unknownModeratorId
                    };
                });

            "When the unknown moderator attempts to update the hub"
                .x(() 
                    => updateHubByUnknownModerator = ()
                        => _hubService.UpdateHub(
                            moderator, _fixture.Create<int>(), _fixture.Create<HubUpdateDto>()));

            "Then the system should throw an exception"
                .x(() 
                    => updateHubByUnknownModerator.Should()
                        .Throw<UnknownUserException>());

            "And not perform any action"
                .x(() 
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to update the hub's data from
        /// another moderator
        /// </summary>
        [Scenario]
        public void UpdateHubOfAnotherModerator(ModeratorDto moderator, HubDto originalHub,
            ModeratorDto otherModerator, Action updateHubOfAnotherModerator)
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
                        .Without(_ => _.Attendees)
                        .With(_ => _.Moderator, _moderators.First(_ => _.Id == moderator.Id))
                        .Create();

                    _hubs.Add(hub);

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = moderator.Id,
                        Attendees = new List<LightweightPupilDto>(),
                        Description = hub.HubDescription,
                        Name = hub.HubName,
                        Link = hub.HubLink
                    };
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

            "When the moderator attempts to update the hub of another one"
                .x(() 
                    => updateHubOfAnotherModerator = ()
                        => _hubService.UpdateHub(
                            otherModerator, originalHub.Id, _fixture.Create<HubUpdateDto>()));

            "Then ths system should throw an exception"
                .x(() 
                    => updateHubOfAnotherModerator.Should()
                        .Throw<UnknownHubException>());

            "And not perform any action"
                .x(()
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to update un unknown hub
        /// </summary>
        [Scenario]
        public void UpdateHubOfUnknownHub(ModeratorDto moderator, HubDto hub, Action updateUnknownHub)
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

            "And an unknown hub"
                .x(() =>
                {
                    const int unknownHubId = -1;

                    hub = new HubDto
                    {
                        Id = unknownHubId
                    };
                });

            "When the moderator attempts to update it"
                .x(() 
                    => updateUnknownHub = ()
                        => _hubService.UpdateHub(moderator, hub.Id, _fixture.Create<HubUpdateDto>()));

            "Then the system should throw an exception"
                .x(()
                    => updateUnknownHub.Should()
                        .Throw<UnknownHubException>());

            "And not perform any action"
                .x(()
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to rename a hub with an existing
        /// name for the current moderator hubs
        /// </summary>
        [Scenario]
        public void UpdateHubWithDuplicatedNames(ModeratorDto moderator, HubDto hub, HubDto hubToRename,
            HubUpdateDto hubUpdate, Action updateHubWithDuplicatedName)
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

            "And two hubs it owns with different names"
                .x(() =>
                {
                    var firstHub = _fixture.Build<InTechNetHubs.Hub>()
                        .Without(_ => _.Attendees)
                        .With(_ => _.Moderator, _moderators.First(_ => _.Id == moderator.Id))
                        .Create();

                    var secondHub = _fixture.Build<InTechNetHubs.Hub>()
                        .Without(_ => _.Attendees)
                        .With(_ => _.Moderator, _moderators.First(_ => _.Id == moderator.Id))
                        .Create();

                    _moderators.Single(_ => _.Id == moderator.Id).Hubs = new List<InTechNetHubs.Hub>
                    {
                        firstHub,
                        secondHub
                    };

                    _hubs.Add(firstHub);
                    _hubs.Add(secondHub);

                    hub = new HubDto
                    {
                        Id = firstHub.Id,
                        IdModerator = moderator.Id,
                        Attendees = new List<LightweightPupilDto>(),
                        Description = firstHub.HubDescription,
                        Name = firstHub.HubName,
                        Link = firstHub.HubLink
                    };

                    hubToRename = new HubDto
                    {
                        Id = secondHub.Id,
                        IdModerator = moderator.Id,
                        Attendees = new List<LightweightPupilDto>(),
                        Description = secondHub.HubDescription,
                        Name = secondHub.HubName,
                        Link = secondHub.HubLink
                    };
                });

            "And an update request with an already existing hub name for this moderator"
                .x(() 
                    => hubUpdate = _fixture.Build<HubUpdateDto>()
                        .With(_ => _.Name, hub.Name)
                        .Create());

            "When it attempts to update the name of a hub with the other hub's name"
                .x(() 
                    => updateHubWithDuplicatedName = ()
                        => _hubService.UpdateHub(moderator, hubToRename.Id, hubUpdate));

            "Then the system should throw an exception"
                .x(() 
                    => updateHubWithDuplicatedName.Should()
                        .Throw<DuplicatedHubNameException>());

            "And not perform any action"
                .x(()
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
        }

        /// <summary>
        /// Check the behavior of the hub service when attempting to rename a hub with an existing
        /// name for other moderators hubs
        /// </summary>
        [Scenario]
        public void UpdateHubWithDuplicatedNamesForOthers(ModeratorDto moderator, HubDto originalHub, 
            HubDto otherModeratorHub, HubUpdateDto hubUpdate)
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
                        .Without(_ => _.Attendees)
                        .With(_ => _.Moderator, _moderators.First(_ => _.Id == moderator.Id))
                        .Create();

                    _moderators.Single(_ => _.Id == moderator.Id).Hubs = new List<InTechNetHubs.Hub>
                    {
                        hub
                    };

                    _hubs.Add(hub);

                    originalHub = new HubDto
                    {
                        Id = hub.Id,
                        IdModerator = moderator.Id,
                        Attendees = new List<LightweightPupilDto>(),
                        Description = hub.HubDescription,
                        Name = hub.HubName,
                        Link = hub.HubLink
                    };
                });

            "And a hub it does not own"
                .x(() =>
                {
                    var otherModerator = _fixture.Create<InTechNetUsers.Moderator>();
                    _moderators.Add(otherModerator);

                    var otherHub = _fixture.Build<InTechNetHubs.Hub>()
                        .Without(_ => _.Attendees)
                        .With(_
                                => _.Moderator,
                            _moderators.First(_ => _.Id == otherModerator.Id))
                        .Create();

                    _moderators.Single(_ => _.Id == otherModerator.Id).Hubs = new List<InTechNetHubs.Hub>
                    {
                        otherHub
                    };

                    _hubs.Add(otherHub);

                    otherModeratorHub = new HubDto
                    {
                        Id = otherHub.Id,
                        IdModerator = otherModerator.Id,
                        Attendees = new List<LightweightPupilDto>(),
                        Description = otherHub.HubDescription,
                        Name = otherHub.HubName,
                        Link = otherHub.HubLink
                    };
                });

            "And an update request with the name of a hub owned by another moderator"
                .x(() 
                    => hubUpdate = _fixture.Build<HubUpdateDto>()
                        .With(_ => _.Name, otherModeratorHub.Name)
                        .Create());

            "When it attempts to update the name of a hub with the other hub's name"
                .x(() 
                    => _hubService.UpdateHub(moderator, originalHub.Id, hubUpdate));

            "Then the name should have been successfully updated"
                .x(() =>
                {
                    _context.Verify(_ => _.SaveChanges(), Times.Once);

                    var hub = _hubs.Single(_ => _.Id == originalHub.Id);

                    hub.HubName.Should()
                        .Be(otherModeratorHub.Name);
                });
        }
    }
}
