using InTechNet.Common.Utils.Api;
using InTechNet.Service.Authentication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Security.Claims;
using InTechNet.Common.Utils.Authentication;

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
        /// <param name="authenticationDto">The login parameters as <see cref="AuthenticationDto"/></param>
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
        public ActionResult<string> Login([FromBody] AuthenticationDto authenticationDto)
        {
            var token = _authenticationService.GetModeratorToken(authenticationDto);

            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpPost("Test")]
        public IActionResult Test() {
            var current = HttpContext.User;

            return Ok(current.Claims.Count());
        }
    }
}
