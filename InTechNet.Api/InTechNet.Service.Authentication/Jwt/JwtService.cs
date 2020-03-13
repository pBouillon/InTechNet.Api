using InTechNet.Common.Utils.Authentication.Jwt.Models;
using InTechNet.Service.Authentication.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace InTechNet.Service.Authentication.Jwt
{
    /// <inheritdoc cref="IJwtService"/>
    public class JwtService : IJwtService
    {
        private readonly JwtResourceHelper _jwtResource;

        public JwtService(JwtResourceHelper jwtResource)
        {
            _jwtResource = jwtResource;
        }

        /// <inheritdoc cref="IJwtService.GetModeratorToken"/>
        public string GetModeratorToken()
        {
            var token = new JwtSecurityToken(
                issuer: _jwtResource.Issuer,
                audience: _jwtResource.Audience,
                expires: _jwtResource.ValidUntil,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(_jwtResource.EncodedSecretKey),
                    _jwtResource.SigningAlgorithm));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <inheritdoc cref="IJwtService.GetPupilToken"/>
        public string GetPupilToken()
        {
            var token = new JwtSecurityToken(
                issuer: _jwtResource.Issuer,
                audience: _jwtResource.Audience,
                expires: _jwtResource.ValidUntil,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(_jwtResource.EncodedSecretKey),
                    _jwtResource.SigningAlgorithm));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
