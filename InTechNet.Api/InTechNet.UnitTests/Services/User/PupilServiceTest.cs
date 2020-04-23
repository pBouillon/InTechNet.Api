using AutoFixture;
using FluentAssertions;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;
using InTechNet.DataAccessLayer.Context;
using InTechNet.DataAccessLayer.Entities.Users;
using InTechNet.Exception.Authentication;
using InTechNet.Services.User;
using InTechNet.UnitTests.Extensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xbehave;

namespace InTechNet.UnitTests.Services.User
{
    /// <summary>
    /// PupilService testing methods
    /// </summary>
    public class PupilServiceTest
    {
        /// <summary>
        /// Fixture object for dummy test data
        /// </summary>
        private readonly Fixture _fixture = new Fixture();

        private Mock<IInTechNetContext> _context;

        private ICollection<Pupil> _pupils;

        private PupilService _pupilService;

        /// <summary>
        /// Default constructor to setup AutoFixture behavior
        /// </summary>
        public PupilServiceTest()
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
            "Given various pupils"
                .x(()
                    => _pupils = _fixture.Build<Pupil>()
                        .Without(_ => _.Attendees)
                        .CreateMany()
                        .ToList());

            "And a database using them as records"
                .x(() =>
                {
                    _context = new Mock<IInTechNetContext>();

                    var pupilsDbSet = _pupils.AsMockedDbSet();

                    _context.SetupGet(_ => _.Pupils)
                        .Returns(pupilsDbSet.Object.AsMockedDbSet().Object);

                    _context.Setup(_ => _.Pupils.Remove(It.IsAny<Pupil>()))
                        .Callback<Pupil>((entity) => _pupils.Remove(entity));
                });

            "And a pupil service"
                .x(()
                    => _pupilService = new PupilService(_context.Object));
        }

        /// <summary>
        /// Assert the behavior of the pupil service when attempting to authenticate
        /// an unknown pupil with its email
        /// </summary>
        [Scenario]
        public void AuthenticateUnknownPupilByEmail(Pupil unknownPupil, Action authenticateUnknownPupil)
        {
            "Given an unknown pupil"
                .x(() => unknownPupil = _fixture.Create<Pupil>());

            "When I attempt to authenticate with its credentials"
                .x(() 
                    => authenticateUnknownPupil = ()
                        => _pupilService.AuthenticatePupil(new AuthenticationDto
                        {
                            Password = unknownPupil.PupilPassword,
                            Login = unknownPupil.PupilEmail
                        }));

            "Then the service should raise an exception"
                .x(()
                    => authenticateUnknownPupil.Should().Throw<UnknownUserException>());
        }

        /// <summary>
        /// Assert the behavior of the pupil service when attempting to authenticate
        /// an unknown pupil with its nickname
        /// </summary>
        [Scenario]
        public void AuthenticateUnknownPupilByNickname(Pupil unknownPupil, Action authenticateUnknownPupil)
        {
            "Given an unknown pupil"
                .x(() => unknownPupil = _fixture.Create<Pupil>());

            "When I attempt to authenticate with its credentials"
                .x(()
                    => authenticateUnknownPupil = ()
                        => _pupilService.AuthenticatePupil(new AuthenticationDto
                        {
                            Password = unknownPupil.PupilPassword,
                            Login = unknownPupil.PupilNickname
                        }));

            "Then the service should raise an exception"
                .x(()
                    => authenticateUnknownPupil.Should().Throw<UnknownUserException>());
        }

        /// <summary>
        /// Assert the behavior of the pupil service when attempting to
        /// remove a pupil
        /// </summary>
        [Scenario]
        public void DeletePupil(Pupil pickedPupil)
        {
            "When I pick a pupil from the recorded ones"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count());
                    pickedPupil = _pupils.ToList()[pickedPupilIndex];
                });

            "And remove it from the list"
                .x(() => _pupilService.DeletePupil(new PupilDto
                {
                    Email = pickedPupil.PupilEmail,
                    Nickname = pickedPupil.PupilNickname,
                    Id = pickedPupil.Id
                }));

            "Then it should no longer appear in the records"
                .x(() =>
                {
                    _context.Verify(_ => _.SaveChanges(), Times.Once);

                    _pupils.Should()
                        .NotContain(pickedPupil);
                });
        }

        /// <summary>
        /// Assert the behavior of the pupil service when attempting to
        /// remove a pupil that does not belong to the database
        /// </summary>
        [Scenario]
        public void DeleteUnknownPupil(Pupil pickedPupil, Action removeUnknownPupil)
        {
            "When I create an untracked pupil"
                .x(() =>
                {
                    const int unusedId = -1;

                    pickedPupil = _fixture.Build<Pupil>()
                        .Without(_ => _.Attendees)
                        .With(_ => _.Id, unusedId)
                        .Create();
                });

            "And attempt to remove it from the list"
                .x(() => removeUnknownPupil = ()
                    => _pupilService.DeletePupil(new PupilDto
                    {
                        Email = pickedPupil.PupilEmail,
                        Nickname = pickedPupil.PupilNickname,
                        Id = pickedPupil.Id
                    }));

            "Then the system should throw an exception"
                .x(() =>
                {
                    _context.Verify(_ => _.SaveChanges(), Times.Never);

                    removeUnknownPupil.Should()
                        .Throw<UnknownUserException>();
                });
        }

        /// <summary>
        /// Assert the behavior of the pupil service when attempting to
        /// retrieve a pupil
        /// </summary>
        [Scenario]
        public void GetPupil(Pupil pickedPupil, PupilDto retrievedPupil)
        {
            "When I pick a pupil from the recorded ones"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count());
                    pickedPupil = _pupils.ToList()[pickedPupilIndex];
                });

            "And attempt to fetch it from the its ID"
                .x(() => retrievedPupil = _pupilService.GetPupil(pickedPupil.Id));

            "Then it should retrieve the original record"
                .x(() =>
                {
                    retrievedPupil.Id
                        .Should()
                        .Be(pickedPupil.Id);

                    retrievedPupil.Email
                        .Should()
                        .Be(pickedPupil.PupilEmail);

                    retrievedPupil.Nickname
                        .Should()
                        .Be(pickedPupil.PupilNickname);
                });
        }

        /// <summary>
        /// Assert the behavior of the pupil service when attempting to
        /// retrieve a pupil
        /// </summary>
        [Scenario]
        public void GetUnknownPupil(Action getUnknownPupil)
        {
            "When I attempt to get an unknown pupil from the database"
                .x(() => {
                    const int unknownPupilId = -1;

                    getUnknownPupil = ()
                        => _pupilService.GetPupil(unknownPupilId);
                });

            "Then the system should throw an exception"
                .x(() =>
                {
                    getUnknownPupil.Should()
                        .Throw<UnknownUserException>();
                });
        }    
    }
}
