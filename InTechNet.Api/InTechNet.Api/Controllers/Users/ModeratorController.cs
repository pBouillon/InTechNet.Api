using InTechNet.Common.Utils.Api;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Exception;
using InTechNet.Service.Authentication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InTechNet.Api.Controllers.Users
{
    /// <summary>
    /// Authentication controller for InTechNet API for moderators
    /// </summary>
    [Route("api/v1/[controller]")]
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
        /// <param name="authenticationDto">The login parameters as <see cref="AuthenticationDto" /></param>
        /// <returns>A valid JWT on success</returns>
        [HttpPost("authenticate")]
        [AllowAnonymous]
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
        public ActionResult<string> Authenticate([FromBody] AuthenticationDto authenticationDto)
        {
            try
            {
                var token = _authenticationService.GetModeratorToken(authenticationDto);
                return Ok(new {Token = token});
            }
            catch (BaseException)
            {
                return Unauthorized();
            }
        }
    }
}
