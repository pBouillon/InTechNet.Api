using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.Authentication.Jwt;

namespace InTechNet.Service.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtService _jwtService;

        public AuthenticationService(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public string GetToken()
        {
            return _jwtService.GetToken();
        }
    }
}
