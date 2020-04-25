using AutoFixture;
using InTechNet.DataAccessLayer.Context;
using InTechNet.DataAccessLayer.Entities.Users;
using InTechNet.Services.Authentication;
using InTechNet.UnitTests.Extensions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InTechNet.Common.Dto.User;
using InTechNet.Services.Authentication.Interfaces;
using InTechNet.Services.User.Interfaces;
using Xbehave;

namespace InTechNet.UnitTests.Services.Authentication
{
    /// <summary>
    /// AuthenticationService testing methods
    /// </summary>
    public class AuthenticationServiceTest
    {
        /// <summary>
        /// Fixture object for dummy test data
        /// </summary>
        private readonly Fixture _fixture = new Fixture();

        /// <summary>
        /// Authentication service
        /// </summary>
        private AuthenticationService _authenticationService;

        /// <summary>
        /// InTechNet context mock
        /// </summary>
        private Mock<IInTechNetContext> _context;

        /// <summary>
        /// Mocked <see cref="IJwtService"/>
        /// </summary>
        private Mock<IJwtService> _jwtService;

        /// <summary>
        /// Collection of Moderator objects representing the database
        /// </summary>
        private ICollection<Moderator> _moderators;

        /// <summary>
        /// Mocked <see cref="IModeratorService"/>
        /// </summary>
        private Mock<IModeratorService> _moderatorService;

        /// <summary>
        /// Collection of Pupil objects representing the database
        /// </summary>
        private ICollection<Pupil> _pupils;

        /// <summary>
        /// Mocked <see cref="IPupilService"/>
        /// </summary>
        private Mock<IPupilService> _pupilService;

        /// <summary>
        /// Default constructor to setup AutoFixture behavior
        /// </summary>
        public AuthenticationServiceTest()
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
            "Given various moderators"
                .x(()
                    => _moderators = _fixture.CreateMany<Moderator>()
                        .ToList());

            "And various pupils"
                .x(()
                    => _pupils = _fixture.CreateMany<Pupil>()
                        .ToList());

            "And a database using them as records"
                .x(() =>
                {
                    _context = new Mock<IInTechNetContext>();

                    // Setup Moderators property
                    var moderatorDbSet = _moderators.AsMockedDbSet();

                    _context.SetupGet(_ => _.Moderators)
                        .Returns(moderatorDbSet.Object);

                    // Setup Pupils property
                    var pupilDbSet = _pupils.AsMockedDbSet();

                    _context.SetupGet(_ => _.Pupils)
                        .Returns(pupilDbSet.Object);
                });

            "And the required InTechNet services"
                .x(() =>
                {
                    // IJwtService mock
                    _jwtService = new Mock<IJwtService>();

                    // IModeratorService mock
                    _moderatorService = new Mock<IModeratorService>();

                    // IPupilService mock
                    _pupilService = new Mock<IPupilService>();
                });

            "And a authentication service"
                .x(() =>
                {
                    var mockedHttpContextAccessor = new Mock<IHttpContextAccessor>();
                    _authenticationService = new AuthenticationService(_moderatorService.Object,
                        _pupilService.Object, _jwtService.Object, mockedHttpContextAccessor.Object);
                });
        }

        [Scenario]
        public void AreCredentialsAlreadyInUseOnUniqueCredentials(CredentialsCheckDto credentials)
        {
            "Given that the nickname is not used by a pupil or a moderator"
                .x(() =>
                {
                    _moderatorService.Setup(_
                            => _.IsNicknameAlreadyInUse(It.IsAny<string>()))
                        .Returns(false);

                    _pupilService.Setup(_
                            => _.IsNicknameAlreadyInUse(It.IsAny<string>()))
                        .Returns(false);
                });

            "And that the email is not used by a pupil or a moderator"
                .x(() =>
                {
                    _moderatorService.Setup(_
                            => _.IsEmailAlreadyInUse(It.IsAny<string>()))
                        .Returns(false);

                    _pupilService.Setup(_
                            => _.IsEmailAlreadyInUse(It.IsAny<string>()))
                        .Returns(false);
                });

            "And credentials to be checked"
                .x(()
                    => credentials = _fixture.Create<CredentialsCheckDto>());

            "When the user checks if the credentials are in use"
                .x(() 
                    => credentials = _authenticationService.AreCredentialsAlreadyInUse(credentials));

            "Then the AreUnique flag should be true"
                .x(() 
                    => credentials.AreUnique.Should()
                        .BeTrue());

            "And both nickname and email should have been checked in both user services"
                .x(() =>
                {
                    // Moderator service
                    _moderatorService.Verify(_ 
                        => _.IsEmailAlreadyInUse(It.IsAny<string>()), Times.Once);

                    _moderatorService.Verify(_
                        => _.IsNicknameAlreadyInUse(It.IsAny<string>()), Times.Once);

                    // Pupil service
                    _pupilService.Verify(_
                        => _.IsEmailAlreadyInUse(It.IsAny<string>()), Times.Once);

                    _pupilService.Verify(_
                        => _.IsNicknameAlreadyInUse(It.IsAny<string>()), Times.Once);
                });
        }
    }
}
