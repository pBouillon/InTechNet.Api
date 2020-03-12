using InTechNet.Common.Utils.Api;
using InTechNet.Service.Authentication.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using InTechNet.Service.Authentication.Interfaces;

namespace InTechNet.Api.Controllers.Users
{
    /// <summary>
    /// Authentication controller for InTechNet API for moderators
    /// </summary>
    [Route("api/v1/moderator/[controller]")]
    [ApiController]
    public class ModeratorController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly IConfiguration _config;

        public ModeratorController(IAuthenticationService authenticationService, IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Login end point for a moderator
        /// </summary>
        /// <param name="loginDto">The login parameters as <see cref="LoginDto"/></param>
        /// <returns>A valid JWT on success</returns>
        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse(200, "Successful authentication")]
        [SwaggerResponse(400, "Invalid credentials")]
        [SwaggerOperation(
            Summary = "Login end point for a moderator",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Moderator
            }
        )]
        public ActionResult<string> Login([FromBody] LoginDto loginDto)
        {
            string GetToken() {
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["JwtToken:SecretKey"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["JwtToken:Issuer"],
                    audience: _config["JwtToken:Audience"],
                    expires: DateTime.Now.AddMinutes(1),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            return Ok(new { Token = GetToken() });
        }

        [HttpPost("Test")]
        public IActionResult Test([FromBody] string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["JwtToken:SecretKey"]));

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["JwtToken:Issuer"],
                    ValidAudience = _config["JwtToken:Audience"],
                    IssuerSigningKey = key
                }, out _);
            }
            catch(Exception e)
            {
                return BadRequest("Token KC " + e.GetType().ToString() + "  " + e.Message);
            }

            return Ok();
        }
    }
}
