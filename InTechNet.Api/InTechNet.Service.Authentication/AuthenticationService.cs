using System;
using System.Security.Claims;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Common.Utils.Authentication.Jwt;
using InTechNet.Exception.Authentication;
using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.User.Interfaces;
using Microsoft.AspNetCore.Http;

namespace InTechNet.Service.Authentication
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
        private readonly IUserService _userService;

        /// <summary>
        /// The current HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Default AuthenticationService constructor
        /// </summary>
        /// <param name="userService">The <see cref="IUserService" /> to be used for authentication checks and data retrieval</param>
        /// <param name="jwtService">The <see cref="IJwtService" /> to be used for the generation</param>
        /// <param name="httpContextAccessor">The current HTTP context accessor</param>
        public AuthenticationService(IUserService userService, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc cref="IAuthenticationService.GetAuthenticatedModerator" />
        public ModeratorDto GetAuthenticatedModerator(AuthenticationDto authenticationDto)
        {
            var moderator = _userService.AuthenticateModerator(authenticationDto);

            moderator.Token = _jwtService.GetModeratorToken(moderator);

            return moderator;
        }

        /// <inheritdoc cref="IAuthenticationService.GetAuthenticatedPupil" />
        public PupilDto GetAuthenticatedPupil(AuthenticationDto authenticationDto)
        {
            var pupil = _userService.AuthenticatePupil(authenticationDto);

            pupil.Token = _jwtService.GetPupilToken(pupil);

            return pupil;
        }

        /// <inheritdoc cref="IAuthenticationService.GetCurrentModerator" />
        public ModeratorDto GetCurrentModerator()
        {
            if (_httpContextAccessor.HttpContext.User.HasClaim(_ =>
                _.Type == ClaimTypes.Role 
                && _.Value != InTechNetRoles.Moderator))
            {
                throw new IllegalRoleException();
            }

            var moderatorId = _httpContextAccessor.HttpContext.User
                .FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnknownUserException();

            return _userService.GetModerator(Convert.ToInt32(moderatorId));
        }

        /// <inheritdoc cref="IAuthenticationService.GetCurrentPupil" />
        public PupilDto GetCurrentPupil()
        {
            if (_httpContextAccessor.HttpContext.User.HasClaim(_ =>
                _.Type == ClaimTypes.Role
                && _.Value != InTechNetRoles.Pupil))
            {
                throw new IllegalRoleException();
            }

            var moderatorId = _httpContextAccessor.HttpContext.User
                                  .FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? throw new UnknownUserException();

            return _userService.GetPupil(Convert.ToInt32(moderatorId));
        }

        /// <inheritdoc cref="IAuthenticationService.CheckEmail(EmailDto)" />
        public bool CheckEmail(EmailDto emailDto)
        {
            return _userService.CheckEmail(emailDto);
        }
    }
}
