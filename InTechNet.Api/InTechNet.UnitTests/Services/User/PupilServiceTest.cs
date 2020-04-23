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
using System.Net.Mail;
using InTechNet.Exception.Registration;
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

        /// <summary>
        /// InTechNet context mock
        /// </summary>
        private Mock<IInTechNetContext> _context;

        /// <summary>
        /// Collection of Pupil objects representing the database
        /// </summary>
        private ICollection<Pupil> _pupils;

        /// <summary>
        /// Pupil service
        /// </summary>
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

                    _context.Setup(_ => _.Pupils.Add(It.IsAny<Pupil>()))
                        .Callback<Pupil>(entity => _pupils.Add(entity));

                    _context.Setup(_ => _.Pupils.Remove(It.IsAny<Pupil>()))
                        .Callback<Pupil>(entity => _pupils.Remove(entity));
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
                .x(() 
                    => removeUnknownPupil.Should()
                        .Throw<UnknownUserException>());

            "And never apply the changes"
                .x(()
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
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
                .x(() 
                    => retrievedPupil = _pupilService.GetPupil(pickedPupil.Id));

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

        /// <summary>
        /// Assert the behavior of the email duplication detection method on unique email
        /// </summary>
        [Scenario]
        public void IsEmailAlreadyInUse(string email, bool isEmailExisting)
        {
            "Given a new email"
                .x(()
                    => email = _fixture.Create<MailAddress>().Address);

            "When checking its existence in the database"
                .x(()
                    => isEmailExisting = _pupilService.IsEmailAlreadyInUse(email));

            "Then the result should be false"
                .x(()
                    => isEmailExisting.Should()
                        .BeFalse());
        }

        /// <summary>
        /// Assert the behavior of the email duplication detection method on duplicated email
        /// </summary>
        [Scenario]
        public void IsEmailAlreadyInUseWithExistingEmail(string email, bool isEmailExisting)
        {
            "Given an email in use"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count());
                    var pickedPupil = _pupils.ToList()[pickedPupilIndex];

                    email = pickedPupil.PupilEmail;
                });

            "When checking its existence in the database"
                .x(()
                    => isEmailExisting = _pupilService.IsEmailAlreadyInUse(email));

            "Then the result should be true"
                .x(()
                    => isEmailExisting.Should()
                        .BeTrue());
        }

        /// <summary>
        /// Assert the behavior of the nickname duplication detection method on unique nickname
        /// </summary>
        [Scenario]
        public void IsNicknameAlreadyInUse(string nickname, bool isNicknameExisting)
        {
            "Given a new email"
                .x(()
                    => nickname = _fixture.Create<string>());

            "When checking its existence in the database"
                .x(()
                    => isNicknameExisting = _pupilService.IsNicknameAlreadyInUse(nickname));

            "Then the result should be false"
                .x(()
                    => isNicknameExisting.Should()
                        .BeFalse());
        }

        /// <summary>
        /// Assert the behavior of the email nickname detection method on duplicated nickname
        /// </summary>
        [Scenario]
        public void IsNicknameAlreadyInUseWithExistingNickname(string nickname, bool isNicknameExisting)
        {
            "Given an email in use"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count());
                    var pickedPupil = _pupils.ToList()[pickedPupilIndex];

                    nickname = pickedPupil.PupilNickname;
                });

            "When checking its existence in the database"
                .x(()
                    => isNicknameExisting = _pupilService.IsNicknameAlreadyInUse(nickname));

            "Then the result should be true"
                .x(()
                    => isNicknameExisting.Should()
                        .BeTrue());
        }

        /// <summary>
        /// Assert the behavior of the pupil service when registering
        /// a new pupil
        /// </summary>
        [Scenario]
        public void RegisterPupil(PupilRegistrationDto pupilRegistration)
        {
            "Given a new pupil"
                .x(() 
                    => pupilRegistration = _fixture.Create<PupilRegistrationDto>());
            
            "When it registers"
                .x(() 
                    => _pupilService.RegisterPupil(pupilRegistration));

            "Then it should appear in the database"
                .x(() =>
                {
                    _context.Verify(_ => _.SaveChanges(), Times.Once);

                    _pupils.Should()
                        .ContainSingle(_ =>
                            _.PupilEmail == pupilRegistration.Email
                            && _.PupilNickname == pupilRegistration.Nickname);
                });

            "And its password should have been hashed"
                .x(() =>
                {
                    var insertedPupil = _pupils.FirstOrDefault(_ =>
                        _.PupilEmail == pupilRegistration.Email
                        && _.PupilNickname == pupilRegistration.Nickname);

                    insertedPupil.PupilPassword.Should()
                        .NotBe(pupilRegistration.Password);
                });
        }

        /// <summary>
        /// Assert the behavior of the pupil service when attempting to
        /// register a new pupil using an email already in use
        /// </summary>
        [Scenario]
        public void RegisterPupilWithExistingEmail(PupilRegistrationDto pupilRegistration,
            Action registerPupilWithExistingEmail)
        {
            "Given a new pupil using an existing email"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count());
                    var pickedPupil = _pupils.ToList()[pickedPupilIndex];

                    pupilRegistration = _fixture.Create<PupilRegistrationDto>();
                    pupilRegistration.Email = pickedPupil.PupilEmail;
                });

            "When it attempt to register"
                .x(()
                    => registerPupilWithExistingEmail = ()
                        => _pupilService.RegisterPupil(pupilRegistration));

            "Then the system should throw an exception"
                .x(()
                    => registerPupilWithExistingEmail.Should()
                        .Throw<DuplicatedEmailException>());

            "And never apply the changes"
                .x(()
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
        }

        /// <summary>
        /// Assert the behavior of the pupil service when attempting to
        /// register a new pupil using a nickname already in use
        /// </summary>
        [Scenario]
        public void RegisterPupilWithExistingNickname(PupilRegistrationDto pupilRegistration,
            Action registerPupilWithExistingNickname)
        {
            "Given a new pupil using an existing email"
                .x(() =>
                {
                    var pickedPupilIndex = new Random().Next(0, _pupils.Count());
                    var pickedPupil = _pupils.ToList()[pickedPupilIndex];

                    pupilRegistration = _fixture.Create<PupilRegistrationDto>();
                    pupilRegistration.Nickname = pickedPupil.PupilNickname;
                });

            "When it attempt to register"
                .x(()
                    => registerPupilWithExistingNickname = () 
                        => _pupilService.RegisterPupil(pupilRegistration));

            "Then the system should throw an exception"
                .x(() 
                    => registerPupilWithExistingNickname.Should()
                        .Throw<DuplicatedIdentifierException>());

            "And never apply the changes"
                .x(() 
                    => _context.Verify(_ => _.SaveChanges(), Times.Never));
        }
    }
}
