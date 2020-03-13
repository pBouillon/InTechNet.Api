using InTechNet.Common.Utils.Api;
using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.Authentication.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

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

        public ModeratorController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
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
            return Ok(new { Token = _authenticationService.GetModeratorToken() });
        }

        [HttpPost("Test")]
        public IActionResult Test([FromBody] string token)
        {
            try
            {
                _authenticationService.EnsureTokenValidity(token);
            }
            catch(Exception e)
            {
                return Unauthorized("Token KO " + e.GetType() + "  " + e.Message);
            }

            return Ok("Token OK");
        }
    }
}
