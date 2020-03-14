using System.Linq;
using InTechNet.Api.Attributes;
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
    /// Authentication controller for InTechNet API for pupils
    /// </summary>
    [Route("api/v1/pupil/[controller]")]
    [ApiController]
    public class PupilController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public PupilController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Login end point for a pupil
        /// </summary>
        /// <param name="authenticationDto">The login parameters as <see cref="AuthenticationDto"/></param>
        /// <returns>A valid JWT on success</returns>
        [HttpPost]
        [SwaggerResponse(200, "Successful authentication")]
        [SwaggerResponse(400, "Invalid credentials")]
        [SwaggerOperation(
            Summary = "Login end point for a pupil",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Pupil
            }
        )]
        public ActionResult<string> Login([FromBody] AuthenticationDto authenticationDto)
        {
            try
            {
                var token = _authenticationService.GetPupilToken(authenticationDto);
                return Ok(new { Token = token });
            }
            catch (BaseException)
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [PupilClaimRequired]
        [HttpPost("Test")]
        public IActionResult Test()
        {
            var current = HttpContext.User;

            return Ok(current.Claims.Count());
        }
    }
}
