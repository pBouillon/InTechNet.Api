using InTechNet.Service.Authentication.Interfaces;

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
        /// Default AuthenticationService constructor
        /// </summary>
        /// <param name="jwtService">The <see cref="IJwtService"/> to be used for the generation</param>
        public AuthenticationService(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        /// <inheritdoc cref="IAuthenticationService.EnsureTokenValidity"/>
        public void EnsureTokenValidity(string token)
        {
            _jwtService.EnsureTokenValidity(token);
        }

        /// <inheritdoc cref="IAuthenticationService.GetModeratorToken"/>
        public string GetModeratorToken()
        {
            return  _jwtService.GetModeratorToken();
        }

        /// <inheritdoc cref="IAuthenticationService.GetPupilToken"/>
        public string GetPupilToken()
        {
            return _jwtService.GetPupilToken();
        }
    }
}
