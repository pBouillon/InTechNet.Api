using AutoFixture;
using FluentAssertions;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;
using InTechNet.DataAccessLayer.Context;
using InTechNet.Exception.Authentication;
using InTechNet.Services.Authentication;
using InTechNet.Services.Authentication.Interfaces;
using InTechNet.Services.User.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Linq;
using System.Security.Claims;
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
        /// Mocked <see cref="IHttpContextAccessor"/>
        /// </summary>
        private Mock<IHttpContextAccessor> _httpContextAccessor;

        /// <summary>
        /// Mocked <see cref="IJwtService"/>
        /// </summary>
        private Mock<IJwtService> _jwtService;

        /// <summary>
        /// Mocked <see cref="IModeratorService"/>
        /// </summary>
        private Mock<IModeratorService> _moderatorService;

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
            "Given the required InTechNet services"
                .x(() =>
                {
                    // IHttpContextAccessor mock
                    _httpContextAccessor = new Mock<IHttpContextAccessor>();

                    // IJwtService mock
                    _jwtService = new Mock<IJwtService>();

                    // IModeratorService mock
                    _moderatorService = new Mock<IModeratorService>();

                    // IPupilService mock
                    _pupilService = new Mock<IPupilService>();
                });

            "And a authentication service"
                .x(()
                    => _authenticationService = new AuthenticationService(_moderatorService.Object,
                        _pupilService.Object, _jwtService.Object, _httpContextAccessor.Object));
        }

        /// <summary>
        /// Checks the behavior of the AuthenticationService when checking if credentials
        /// are unique when the email is duplicated with another moderator's email
        /// </summary>
        [Scenario]
        public void AreCredentialsAlreadyInUseOnEmailUsedByModerator(CredentialsCheckDto credentials)
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

            "And that the email is not used by a pupil but used by a moderator"
                .x(() =>
                {
                    _moderatorService.Setup(_
                            => _.IsEmailAlreadyInUse(It.IsAny<string>()))
                        .Returns(true);

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

            "Then the AreUnique flag should be false"
                .x(()
                    => credentials.AreUnique.Should()
                        .BeFalse());

            "And both nickname and email should have been checked in both user services"
                .x(() =>
                {
                    // Moderator service
                    _moderatorService.Verify(_
                        => _.IsEmailAlreadyInUse(It.IsAny<string>()), Times.Once);

                    _moderatorService.Verify(_
                        => _.IsNicknameAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);

                    // Pupil service
                    _pupilService.Verify(_
                        => _.IsEmailAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);

                    _pupilService.Verify(_
                        => _.IsNicknameAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);
                });
        }

        /// <summary>
        /// Checks the behavior of the AuthenticationService when checking if credentials
        /// are unique when the email is duplicated with another pupil's email
        /// </summary>
        [Scenario]
        public void AreCredentialsAlreadyInUseOnEmailUsedByPupil(CredentialsCheckDto credentials)
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

            "And that the email is not used by a moderator but used by a pupil"
                .x(() =>
                {
                    _moderatorService.Setup(_
                            => _.IsEmailAlreadyInUse(It.IsAny<string>()))
                        .Returns(false);

                    _pupilService.Setup(_
                            => _.IsEmailAlreadyInUse(It.IsAny<string>()))
                        .Returns(true);
                });

            "And credentials to be checked"
                .x(()
                    => credentials = _fixture.Create<CredentialsCheckDto>());

            "When the user checks if the credentials are in use"
                .x(()
                    => credentials = _authenticationService.AreCredentialsAlreadyInUse(credentials));

            "Then the AreUnique flag should be false"
                .x(()
                    => credentials.AreUnique.Should()
                        .BeFalse());

            "And both nickname and email should have been checked in both user services"
                .x(() =>
                {
                    // Moderator service
                    _moderatorService.Verify(_
                        => _.IsEmailAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);

                    _moderatorService.Verify(_
                        => _.IsNicknameAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);

                    // Pupil service
                    _pupilService.Verify(_
                        => _.IsEmailAlreadyInUse(It.IsAny<string>()), Times.Once);

                    _pupilService.Verify(_
                        => _.IsNicknameAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);
                });
        }

        /// <summary>
        /// Checks the behavior of the AuthenticationService when checking if credentials
        /// are unique when the nickname is duplicated with another moderator's nickname
        /// </summary>
        [Scenario]
        public void AreCredentialsAlreadyInUseOnNicknameUsedByModerator(CredentialsCheckDto credentials)
        {
            "Given that the nickname is not used by a pupil but used by a moderator"
                .x(() =>
                {
                    _moderatorService.Setup(_
                            => _.IsNicknameAlreadyInUse(It.IsAny<string>()))
                        .Returns(true);

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

            "Then the AreUnique flag should be false"
                .x(()
                    => credentials.AreUnique.Should()
                        .BeFalse());

            "And both nickname and email should have been checked in both user services"
                .x(() =>
                {
                    // Moderator service
                    _moderatorService.Verify(_
                        => _.IsEmailAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);

                    _moderatorService.Verify(_
                        => _.IsNicknameAlreadyInUse(It.IsAny<string>()), Times.Once);

                    // Pupil service
                    _pupilService.Verify(_
                        => _.IsEmailAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);

                    _pupilService.Verify(_
                        => _.IsNicknameAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);
                });
        }

        /// <summary>
        /// Checks the behavior of the AuthenticationService when checking if unique credentials
        /// are unique
        /// </summary>
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

        /// <summary>
        /// Checks the behavior of the AuthenticationService when checking if credentials
        /// are unique when the nickname is duplicated with another pupil's nickname
        /// </summary>
        [Scenario]
        public void AreCredentialsAlreadyInUseOnNicknameUsedByPupil(CredentialsCheckDto credentials)
        {
            "Given that the nickname is not used by a moderator but used by a pupil"
                .x(() =>
                {
                    _moderatorService.Setup(_
                            => _.IsNicknameAlreadyInUse(It.IsAny<string>()))
                        .Returns(false);

                    _pupilService.Setup(_
                            => _.IsNicknameAlreadyInUse(It.IsAny<string>()))
                        .Returns(true);
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

            "Then the AreUnique flag should be false"
                .x(()
                    => credentials.AreUnique.Should()
                        .BeFalse());

            "And both nickname and email should have been checked in both user services"
                .x(() =>
                {
                    // Moderator service
                    _moderatorService.Verify(_
                        => _.IsEmailAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);

                    _moderatorService.Verify(_
                        => _.IsNicknameAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);

                    // Pupil service
                    _pupilService.Verify(_
                        => _.IsEmailAlreadyInUse(It.IsAny<string>()), Times.AtMostOnce);

                    _pupilService.Verify(_
                        => _.IsNicknameAlreadyInUse(It.IsAny<string>()), Times.Once);
                });
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when authenticating with a moderator
        /// </summary>
        [Scenario]
        public void GetAuthenticatedModerator(ModeratorDto moderator)
        {
            "Given that the authentication is successful"
                .x(()
                    => _moderatorService
                        .Setup(_ => _.AuthenticateModerator(It.IsAny<AuthenticationDto>()))
                        .Returns(new ModeratorDto()));

            "When the moderator authenticate itself"
                .x(() =>
                {
                    _jwtService
                        .Setup(_ => _.GetModeratorToken(It.IsAny<ModeratorDto>()))
                        .Returns(_fixture.Create<string>());

                    moderator = _authenticationService.GetAuthenticatedModerator(
                        _fixture.Create<AuthenticationDto>());
                });

            "Then the moderator should have a token"
                .x(()
                    => moderator.Token.Should()
                        .NotBeNullOrEmpty());
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when the moderator's authentication fails
        /// </summary>
        [Scenario]
        public void GetAuthenticatedModeratorOnFailedAuthentication(Action getAuthenticatedModeratorOnAuthenticationFail)
        {
            "Given that the authentication fails"
                .x(()
                    => _moderatorService
                        .Setup(_ => _.AuthenticateModerator(It.IsAny<AuthenticationDto>()))
                        .Throws(new UnknownUserException()));

            "When the moderator authenticate itself"
                .x(() 
                    => getAuthenticatedModeratorOnAuthenticationFail = () 
                        => _authenticationService.GetAuthenticatedModerator(
                        _fixture.Create<AuthenticationDto>()));

            "Then the token should not have been generated"
                .x(()
                    => _jwtService.Verify(_ => _.GetModeratorToken(It.IsAny<ModeratorDto>()), Times.Never));
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when authenticating with a pupil
        /// </summary>
        [Scenario]
        public void GetAuthenticatedPupil(PupilDto pupil)
        {
            "Given that the authentication is successful"
                .x(()
                    => _pupilService
                        .Setup(_ => _.AuthenticatePupil(It.IsAny<AuthenticationDto>()))
                        .Returns(new PupilDto()));

            "When the pupil authenticate itself"
                .x(() =>
                {
                    _jwtService
                        .Setup(_ => _.GetPupilToken(It.IsAny<PupilDto>()))
                        .Returns(_fixture.Create<string>());

                    pupil = _authenticationService.GetAuthenticatedPupil(
                        _fixture.Create<AuthenticationDto>());
                });

            "Then the pupil should have a token"
                .x(()
                    => pupil.Token.Should()
                        .NotBeNullOrEmpty());
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when the pupil's authentication fails
        /// </summary>
        [Scenario]
        public void GetAuthenticatedPupilOnFailedAuthentication(Action getAuthenticatedPupilOnAuthenticationFail)
        {
            "Given that the authentication fails"
                .x(()
                    => _pupilService
                        .Setup(_ => _.AuthenticatePupil(It.IsAny<AuthenticationDto>()))
                        .Throws(new UnknownUserException()));

            "When the pupil authenticate itself"
                .x(()
                    => getAuthenticatedPupilOnAuthenticationFail = ()
                        => _authenticationService.GetAuthenticatedPupil(
                        _fixture.Create<AuthenticationDto>()));

            "Then the token should not have been generated"
                .x(()
                    => _jwtService.Verify(_ => _.GetPupilToken(It.IsAny<PupilDto>()), Times.Never));
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current moderator
        /// </summary>
        [Scenario]
        public void GetCurrentModerator()
        {
            "Given that the moderator has the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(false));

            "And that the moderator has the NameIdentifier claim"
                .x(() =>
                {
                    var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

                    mockedClaimsPrincipal
                        .Setup(_ => _.FindFirst(It.IsAny<string>()))
                        .Returns(
                            new Claim(ClaimTypes.NameIdentifier, _fixture.Create<int>().ToString()));

                    _httpContextAccessor
                        .SetupGet(_ => _.HttpContext.User)
                        .Returns(mockedClaimsPrincipal.Object);
                });


            "When the system retrieves the current moderator"
                .x(() =>
                {
                    _moderatorService
                        .Setup(_ => _.GetModerator(It.IsAny<int>()))
                        .Returns(new ModeratorDto());

                    _authenticationService.GetCurrentModerator();
                });

            "Then the system should have successfully queried the moderator service"
                .x(()
                    => _moderatorService.Verify(_ => _.GetModerator(It.IsAny<int>()), Times.Once));
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current moderator
        /// when the current user has a claim Role that does not match the role of moderator
        /// </summary>
        [Scenario]
        public void GetCurrentModeratorWithBadRole(Action retrieveUserWithBadClaim)
        {
            "Given that the moderator does not have the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(true));

            "When the system attempts to retrieve the current moderator"
                .x(()
                    => retrieveUserWithBadClaim = ()
                        => _authenticationService.GetCurrentModerator());

            "Then the system should throw an exception"
                .x(()
                    => retrieveUserWithBadClaim.Should()
                        .Throw<IllegalRoleException>());
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current moderator
        /// when the current user has the right claim but does not have a NameIdentifier claim
        /// </summary>
        [Scenario]
        public void GetCurrentModeratorWithUnknownUser(Action retrieveUnknownUser)
        {
            "Given that the moderator has the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(false));

            "And that the moderator identifier does not exists in its JWT"
                .x(() => 
                {
                    var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

                    mockedClaimsPrincipal
                        .Setup(_ => _.FindFirst(It.IsAny<string>()))
                        .Returns((Claim) null);

                    _httpContextAccessor
                        .SetupGet(_ => _.HttpContext.User)
                        .Returns(mockedClaimsPrincipal.Object);
                });

            "When the system attempts to retrieve the current moderator"
                .x(()
                    => retrieveUnknownUser = ()
                        => _authenticationService.GetCurrentModerator());

            "Then the system should throw an exception"
                .x(()
                    => retrieveUnknownUser.Should()
                        .Throw<UnknownUserException>());
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current pupil
        /// </summary>
        [Scenario]
        public void GetCurrentPupil()
        {
            "Given that the pupil has the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(false));

            "And that the pupil has the NameIdentifier claim"
                .x(() =>
                {
                    var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

                    mockedClaimsPrincipal
                        .Setup(_ => _.FindFirst(It.IsAny<string>()))
                        .Returns(
                            new Claim(ClaimTypes.NameIdentifier, _fixture.Create<int>().ToString()));

                    _httpContextAccessor
                        .SetupGet(_ => _.HttpContext.User)
                        .Returns(mockedClaimsPrincipal.Object);
                });


            "When the system retrieves the current moderator"
                .x(() =>
                {
                    _pupilService
                        .Setup(_ => _.GetPupil(It.IsAny<int>()))
                        .Returns(new PupilDto());

                    _authenticationService.GetCurrentPupil();
                });

            "Then the system should have successfully queried the moderator service"
                .x(()
                    => _pupilService.Verify(_ => _.GetPupil(It.IsAny<int>()), Times.Once));
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current pupil
        /// when the current user has a claim Role that does not match the role of pupil
        /// </summary>
        [Scenario]
        public void GetCurrentPupilWithBadRole(Action retrieveUserWithBadClaim)
        {
            "Given that the pupil does not have the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(true));

            "When the system attempts to retrieve the current pupil"
                .x(()
                    => retrieveUserWithBadClaim = ()
                        => _authenticationService.GetCurrentPupil());

            "Then the system should throw an exception"
                .x(()
                    => retrieveUserWithBadClaim.Should()
                        .Throw<IllegalRoleException>());
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current pupil
        /// when the current user has the right claim but does not have a NameIdentifier claim
        /// </summary>
        [Scenario]
        public void GetCurrentPupilWithUnknownUser(Action retrieveUnknownUser)
        {
            "Given that the pupil has the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(false));

            "And that the pupil identifier does not exists in its JWT"
                .x(() =>
                {
                    var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

                    mockedClaimsPrincipal
                        .Setup(_ => _.FindFirst(It.IsAny<string>()))
                        .Returns((Claim) null);

                    _httpContextAccessor
                        .SetupGet(_ => _.HttpContext.User)
                        .Returns(mockedClaimsPrincipal.Object);
                });

            "When the system attempts to retrieve the current pupil"
                .x(()
                    => retrieveUnknownUser = ()
                        => _authenticationService.GetCurrentPupil());

            "Then the system should throw an exception"
                .x(()
                    => retrieveUnknownUser.Should()
                        .Throw<UnknownUserException>());
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current moderator
        /// </summary>
        [Scenario]
        public void TryGetCurrentModerator(ModeratorDto moderator, bool success)
        {
            "Given that the moderator has the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(false));

            "And that the moderator has the NameIdentifier claim"
                .x(() =>
                {
                    var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

                    mockedClaimsPrincipal
                        .Setup(_ => _.FindFirst(It.IsAny<string>()))
                        .Returns(
                            new Claim(ClaimTypes.NameIdentifier, _fixture.Create<int>().ToString()));

                    _httpContextAccessor
                        .SetupGet(_ => _.HttpContext.User)
                        .Returns(mockedClaimsPrincipal.Object);
                });


            "When the system retrieves the current moderator"
                .x(() =>
                {
                    _moderatorService
                        .Setup(_ => _.GetModerator(It.IsAny<int>()))
                        .Returns(new ModeratorDto());

                    success = _authenticationService.TryGetCurrentModerator(out moderator);
                });

            "Then the system should not have been able to fetch the user"
                .x(() =>
                {
                    moderator.Should().NotBeNull();
                    success.Should().BeTrue();
                });
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current moderator
        /// when the current user has a claim Role that does not match the role of moderator
        /// </summary>
        [Scenario]
        public void TryGetCurrentModeratorWithBadRole(ModeratorDto moderator, bool success)
        {
            "Given that the moderator does not have the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(true));

            "When the system attempts to retrieve the current moderator"
                .x(()
                    => success = _authenticationService.TryGetCurrentModerator(out moderator));

            "Then the system should not have been able to fetch the user"
                .x(() =>
                {
                    moderator.Should().BeNull();
                    success.Should().BeFalse();
                });
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current moderator
        /// when the current user has the right claim but does not have a NameIdentifier claim
        /// </summary>
        [Scenario]
        public void TryGetCurrentModeratorWithUnknownUser(ModeratorDto moderator, bool success)
        {
            "Given that the moderator has the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(false));

            "And that the moderator identifier does not exists in its JWT"
                .x(() =>
                {
                    var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

                    mockedClaimsPrincipal
                        .Setup(_ => _.FindFirst(It.IsAny<string>()))
                        .Returns((Claim)null);

                    _httpContextAccessor
                        .SetupGet(_ => _.HttpContext.User)
                        .Returns(mockedClaimsPrincipal.Object);
                });

            "When the system attempts to retrieve the current moderator"
                .x(()
                    => success = _authenticationService.TryGetCurrentModerator(out moderator));

            "Then the system should not have been able to fetch the user"
                .x(() =>
                {
                    moderator.Should().BeNull();
                    success.Should().BeFalse();
                });
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current pupil
        /// </summary>
        [Scenario]
        public void TryGetCurrentPupil(PupilDto pupil, bool success)
        {
            "Given that the pupil has the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(false));

            "And that the pupil has the NameIdentifier claim"
                .x(() =>
                {
                    var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

                    mockedClaimsPrincipal
                        .Setup(_ => _.FindFirst(It.IsAny<string>()))
                        .Returns(
                            new Claim(ClaimTypes.NameIdentifier, _fixture.Create<int>().ToString()));

                    _httpContextAccessor
                        .SetupGet(_ => _.HttpContext.User)
                        .Returns(mockedClaimsPrincipal.Object);
                });


            "When the system retrieves the current moderator"
                .x(() =>
                {
                    _pupilService
                        .Setup(_ => _.GetPupil(It.IsAny<int>()))
                        .Returns(new PupilDto());

                    success = _authenticationService.TryGetCurrentPupil(out pupil);
                });

            "Then the system should have successfully fetch the moderator"
                .x(() =>
                {
                    pupil.Should().NotBeNull();
                    success.Should().BeTrue();
                });
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current pupil
        /// when the current user has a claim Role that does not match the role of pupil
        /// </summary>
        [Scenario]
        public void TryGetCurrentPupilWithBadRole(PupilDto pupil, bool success)
        {
            "Given that the pupil does not have the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(true));

            "When the system attempts to retrieve the current pupil"
                .x(()
                    => success = _authenticationService.TryGetCurrentPupil(out pupil));

            "Then the system should not have been able to fetch the user"
                .x(() =>
                {
                    pupil.Should().BeNull();
                    success.Should().BeFalse();
                });
        }

        /// <summary>
        /// Check the behavior of the AuthenticationService when retrieving the current pupil
        /// when the current user has the right claim but does not have a NameIdentifier claim
        /// </summary>
        [Scenario]
        public void TryGetCurrentPupilWithUnknownUser(PupilDto pupil, bool success)
        {
            "Given that the pupil has the required claim"
                .x(()
                    => _httpContextAccessor
                        .Setup(_ => _.HttpContext.User.HasClaim(It.IsAny<Predicate<Claim>>()))
                        .Returns(false));

            "And that the pupil identifier does not exists in its JWT"
                .x(() =>
                {
                    var mockedClaimsPrincipal = new Mock<ClaimsPrincipal>();

                    mockedClaimsPrincipal
                        .Setup(_ => _.FindFirst(It.IsAny<string>()))
                        .Returns((Claim)null);

                    _httpContextAccessor
                        .SetupGet(_ => _.HttpContext.User)
                        .Returns(mockedClaimsPrincipal.Object);
                });

            "When the system attempts to retrieve the current pupil"
                .x(()
                    => success = _authenticationService.TryGetCurrentPupil(out pupil));

            "Then the system should not have been able to fetch the user"
                .x(() =>
                {
                    pupil.Should().BeNull();
                    success.Should().BeFalse();
                });
        }

    }
}
