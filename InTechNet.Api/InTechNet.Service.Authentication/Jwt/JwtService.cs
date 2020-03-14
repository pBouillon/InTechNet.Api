using InTechNet.Common.Utils.Authentication.Jwt;
using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.User.Models;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        public string GetModeratorToken(ModeratorDto moderator)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Moderator"),
                new Claim(ClaimTypes.UserData, moderator.IdModerator.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtResource.Issuer,
                audience: _jwtResource.Audience,
                expires: _jwtResource.ValidUntil,
                signingCredentials: new SigningCredentials
                (
                    new SymmetricSecurityKey(_jwtResource.EncodedSecretKey),
                    _jwtResource.SigningAlgorithm
                ),
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <inheritdoc cref="IJwtService.GetPupilToken"/>
        public string GetPupilToken()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Pupil")
            };

            var token = new JwtSecurityToken(
                issuer: _jwtResource.Issuer,
                audience: _jwtResource.Audience,
                expires: _jwtResource.ValidUntil,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(_jwtResource.EncodedSecretKey),
                    _jwtResource.SigningAlgorithm
                ),
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
