using InTechNet.Common.Utils.Authentication.Jwt;
using InTechNet.Common.Utils.Security;
using InTechNet.Service.Authentication.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;

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
                new Claim(ClaimTypes.NameIdentifier, moderator.Id.ToString())
            };

            var token = new JwtSecurityToken(
                _jwtResource.Issuer,
                _jwtResource.Audience,
                expires: _jwtResource.ValidUntil,
                signingCredentials: new SigningCredentials
                (
                    new SymmetricSecurityKey(_jwtResource.EncodedSecretKey),
                    InTechNetSecurity.JwtSigningAlgorithm
                ),
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <inheritdoc cref="IJwtService.GetPupilToken" />
        public string GetPupilToken(PupilDto pupil)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, InTechNetRoles.Pupil),
                new Claim(ClaimTypes.NameIdentifier, pupil.Id.ToString())
            };

            var token = new JwtSecurityToken(
                _jwtResource.Issuer,
                _jwtResource.Audience,
                expires: _jwtResource.ValidUntil,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(_jwtResource.EncodedSecretKey),
                    InTechNetSecurity.JwtSigningAlgorithm
                ),
                claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
