using System;
using System.Security.Claims;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Common.Utils.Authentication.Jwt;
using InTechNet.Exception.Authentication;
using InTechNet.Services.Authentication.Interfaces;
using InTechNet.Services.User.Interfaces;
using Microsoft.AspNetCore.Http;

namespace InTechNet.Services.Authentication
{
    /// <inheritdoc cref="IAuthenticationService" />
    public class AuthenticationService : IAuthenticationService
    {
        /// <summary>
        /// The <see cref="IJwtService" /> to be used for the generation
        /// </summary>
        private readonly IJwtService _jwtService;

        /// <summary>
        /// The <see cref="IUserService" /> to be used for authentication checks and data retrieval
        /// </summary>
        private readonly IModeratorService _moderatorService;

        /// <summary>
        /// The <see cref="IUserService" /> to be used for authentication checks and data retrieval
        /// </summary>
        private readonly IPupilService _pupilService;

        /// <summary>
        /// The current HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Default AuthenticationService constructor
        /// </summary>
        /// <param name="moderatorService">The <see cref="IModeratorService" /> to be used for moderator authentication checks and data retrieval</param>
        /// <param name="pupilService">The <see cref="IPupilService" /> to be used for pupil authentication checks and data retrieval</param>
        /// <param name="jwtService">The <see cref="IJwtService" /> to be used for the generation</param>
        /// <param name="httpContextAccessor">The current HTTP context accessor</param>
        public AuthenticationService(IModeratorService moderatorService, IPupilService pupilService,
                IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
            => (_moderatorService, _pupilService, _jwtService, _httpContextAccessor) = (moderatorService, pupilService, jwtService, httpContextAccessor);

        /// <inheritdoc cref="IAuthenticationService.AreCredentialsAlreadyInUse" />
        public CredentialsCheckDto AreCredentialsAlreadyInUse(CredentialsCheckDto credentials)
        {
            credentials.AreUnique = !IsNicknameAlreadyInUse(credentials.Nickname)
                   && !IsEmailAlreadyInUse(credentials.Email);

            return credentials;
        }

        /// <inheritdoc cref="IAuthenticationService.GetAuthenticatedModerator" />
        public ModeratorDto GetAuthenticatedModerator(AuthenticationDto authenticationDto)
        {
            var moderator = _moderatorService.AuthenticateModerator(authenticationDto);

            moderator.Token = _jwtService.GetModeratorToken(moderator);

            return moderator;
        }

        /// <inheritdoc cref="IAuthenticationService.GetAuthenticatedPupil" />
        public PupilDto GetAuthenticatedPupil(AuthenticationDto authenticationDto)
        {
            var pupil = _pupilService.AuthenticatePupil(authenticationDto);

            pupil.Token = _jwtService.GetPupilToken(pupil);

            return pupil;
        }

        /// <inheritdoc cref="IAuthenticationService.GetCurrentModerator" />
        public ModeratorDto GetCurrentModerator()
        {
            if (_httpContextAccessor.HttpContext.User.HasClaim(_
                => _.Type == ClaimTypes.Role 
                    && _.Value != InTechNetRoles.Moderator))
            {
                throw new IllegalRoleException();
            }

            var moderatorId = _httpContextAccessor.HttpContext.User
                .FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnknownUserException();

            return _moderatorService.GetModerator(Convert.ToInt32(moderatorId));
        }

        /// <inheritdoc cref="IAuthenticationService.GetCurrentPupil" />
        public PupilDto GetCurrentPupil()
        {
            if (_httpContextAccessor.HttpContext.User.HasClaim(_ 
                => _.Type == ClaimTypes.Role
                    && _.Value != InTechNetRoles.Pupil))
            {
                throw new IllegalRoleException();
            }

            var moderatorId = _httpContextAccessor.HttpContext.User
                                  .FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? throw new UnknownUserException();

            return _pupilService.GetPupil(Convert.ToInt32(moderatorId));
        }

        /// <inheritdoc cref="IAuthenticationService.IsEmailAlreadyInUse" />
        private bool IsEmailAlreadyInUse(string email)
        {
            return _moderatorService.IsEmailAlreadyInUse(email)
                || _pupilService.IsEmailAlreadyInUse(email);
        }

        /// <inheritdoc cref="IAuthenticationService.IsNicknameAlreadyInUse" />
        private bool IsNicknameAlreadyInUse(string nickname)
        {
            return _moderatorService.IsNicknameAlreadyInUse(nickname)
                || _pupilService.IsNicknameAlreadyInUse(nickname);
        }

        /// <inheritdoc cref="IAuthenticationService.TryGetCurrentModerator" />
        public bool TryGetCurrentModerator(out ModeratorDto moderatorDto)
        {
            moderatorDto = null;

            try
            {
                moderatorDto = GetCurrentModerator();
            }
            // On exception, moderator will be null
            // Since this is a TryX method, errors are silenced
            catch { }

            return moderatorDto != null;
        }

        /// <inheritdoc cref="IAuthenticationService.TryGetCurrentPupil" />
        public bool TryGetCurrentPupil(out PupilDto pupilDto)
        {
            pupilDto = null;

            try
            {
                pupilDto = GetCurrentPupil();
            }
            // On exception, pupil will be null
            // Since this is a TryX method, errors are silenced
            catch { }

            return pupilDto != null;
        }
    }
}
