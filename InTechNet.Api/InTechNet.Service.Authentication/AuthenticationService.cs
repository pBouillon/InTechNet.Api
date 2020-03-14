using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.Authentication.Models.Dto;
using InTechNet.Service.User.Interfaces;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.Authentication
{
    /// <inheritdoc cref="IAuthenticationService"/>
    public class AuthenticationService : IAuthenticationService
    {
        /// <summary>
        /// The <see cref="IJwtService"/> to be used for the generation
        /// </summary>
        private readonly IJwtService _jwtService;

        /// <summary>
        /// The <see cref="IUserService"/> to be used for authentication checks and data retrieval
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Default AuthenticationService constructor
        /// </summary>
        /// <param name="userService">The <see cref="IUserService"/> to be used for authentication checks and data retrieval</param>
        /// <param name="jwtService">The <see cref="IJwtService"/> to be used for the generation</param>
        public AuthenticationService(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        /// <inheritdoc cref="IAuthenticationService.GetModeratorToken"/>
        public string GetModeratorToken(AuthenticationDto authenticationDto)
        {
            var moderator = _userService.AuthenticateModerator(authenticationDto);

            return _jwtService.GetModeratorToken(moderator);
        }

        /// <inheritdoc cref="IAuthenticationService.GetPupilToken"/>
        public string GetPupilToken(AuthenticationDto authenticationDto)
        {
            // var pupil = _userService.AuthenticatePupil(authenticationDto);

            return _jwtService.GetPupilToken();
        }
    }
}
