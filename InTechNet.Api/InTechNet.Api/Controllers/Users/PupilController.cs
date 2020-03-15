using InTechNet.Common.Utils.Api;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Exception;
using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.User.Interfaces;
using InTechNet.Service.User.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InTechNet.Api.Controllers.Users
{
    /// <summary>
    /// Authentication controller for InTechNet API for pupils
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PupilController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        private readonly IUserService _userService;

        public PupilController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        /// <summary>
        /// Login end point for a pupil
        /// </summary>
        /// <param name="authenticationDto">The login parameters as <see cref="AuthenticationDto" /></param>
        /// <returns>A valid JWT on success</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [SwaggerResponse(200, "Successful authentication")]
        [SwaggerResponse(401, "Invalid credentials")]
        [SwaggerOperation(
            Summary = "Login end point for a pupil",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Pupil
            }
        )]
        public ActionResult<string> Authenticate([FromBody] AuthenticationDto authenticationDto)
        {
            try
            {
                var token = _authenticationService.GetPupilToken(authenticationDto);
                return Ok(new {Token = token});
            }
            catch (BaseException)
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Registration endpoint to create a new pupil
        /// </summary>
        /// <param name="newPupilData">A <see cref="PupilDto" /> holding the new pupil's data</param>
        /// <returns>A JWT for the newly created user on success</returns>
        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse(200, "New pupil successfully added")]
        [SwaggerResponse(404, "Invalid payload")]
        [SwaggerOperation(
            Summary = "Registration endpoint to create a new pupil",
            Tags = new[]
            {
                SwaggerTag.Pupil,
                SwaggerTag.Registration
            }
        )]
        public IActionResult Register([FromBody] PupilDto newPupilData)
        {
            try
            {
                _userService.RegisterPupil(newPupilData);

                var associatedToken = _authenticationService.GetModeratorToken(new AuthenticationDto
                {
                    Login = newPupilData.Nickname,
                    Password = newPupilData.Password
                });

                return Ok(new { Token = associatedToken });
            }
            catch (BaseException)
            {
                return BadRequest();
            }
        }
    }
}
