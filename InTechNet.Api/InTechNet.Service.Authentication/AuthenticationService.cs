﻿using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Common.Utils.Authentication.Jwt;
using InTechNet.Exception.Authentication;
using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.User.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

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
        /// <param name="userService">The <see cref="IUserService" /> to be used for authentication checks and data retrieval</param>
        /// <param name="jwtService">The <see cref="IJwtService" /> to be used for the generation</param>
        /// <param name="httpContextAccessor">The current HTTP context accessor</param>
        public AuthenticationService(IModeratorService moderatorService, IPupilService pupilService, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
            => (_moderatorService, _pupilService, _jwtService, _httpContextAccessor) = (moderatorService, pupilService, jwtService, httpContextAccessor);

        /// <inheritdoc cref="IAuthenticationService.AreCredentialsAlreadyInUse" />
        public CredentialsCheckDto AreCredentialsAlreadyInUse(CredentialsCheckDto credentials)
        {
            credentials.AreUnique = !this.IsNicknameAlreadyInUse(credentials.Nickname)
                   && !this.IsEmailAlreadyInUse(credentials.Email);

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
            if (_httpContextAccessor.HttpContext.User.HasClaim(_ =>
                _.Type == ClaimTypes.Role 
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
            if (_httpContextAccessor.HttpContext.User.HasClaim(_ =>
                _.Type == ClaimTypes.Role
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
    }
}
