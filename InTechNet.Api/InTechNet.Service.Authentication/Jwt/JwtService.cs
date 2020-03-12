using InTechNet.Common.Utils.Authentication.Jwt.Models;
using InTechNet.Service.Authentication.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace InTechNet.Service.Authentication.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtResourcesDto _jwtResource;

        public JwtService(JwtResourcesDto jwtResource)
        {
            _jwtResource = jwtResource;
        }

        public void EnsureTokenValidity(string token)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtResource.SecretKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtResource.Issuer,
                ValidAudience = _jwtResource.Audience,
                IssuerSigningKey = key
            }, out _);
        }

        public string GetToken()
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtResource.SecretKey));

            var token = new JwtSecurityToken(
                issuer: _jwtResource.Issuer,
                audience: _jwtResource.Audience,
                expires: _jwtResource.ValidUntil,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
