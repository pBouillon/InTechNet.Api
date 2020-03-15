using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Common.Utils.Authentication.Jwt;
using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.User.Models;
using Microsoft.IdentityModel.Tokens;

namespace InTechNet.Service.Authentication.Jwt
{
    /// <inheritdoc cref="IJwtService" />
    public class JwtService : IJwtService
    {
        private readonly JwtResourceHelper _jwtResource;

        public JwtService(JwtResourceHelper jwtResource)
        {
            _jwtResource = jwtResource;
        }

        /// <inheritdoc cref="IJwtService.GetModeratorToken" />
        public string GetModeratorToken(ModeratorDto moderator)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, InTechNetRoles.Moderator),
                new Claim(ClaimTypes.UserData, moderator.Id.ToString())
            };

            var token = new JwtSecurityToken(
                _jwtResource.Issuer,
                _jwtResource.Audience,
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

        /// <inheritdoc cref="IJwtService.GetPupilToken" />
        public string GetPupilToken(AuthenticationDto authenticationDto)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, InTechNetRoles.Pupil)
            };

            var token = new JwtSecurityToken(
                _jwtResource.Issuer,
                _jwtResource.Audience,
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
