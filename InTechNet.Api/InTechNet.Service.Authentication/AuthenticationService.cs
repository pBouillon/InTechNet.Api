using InTechNet.Service.Authentication.Interfaces;

namespace InTechNet.Service.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtService _jwtService;

        public AuthenticationService(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public void EnsureTokenValidity(string token)
        {
            _jwtService.EnsureTokenValidity(token);
        }

        public string GetToken()
        {
            return _jwtService.GetToken();
        }
    }
}
