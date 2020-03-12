using System;
using System.Text;
using InTechNet.Common.Utils.Authentication.Jwt.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

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
            throw new System.NotImplementedException();
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
