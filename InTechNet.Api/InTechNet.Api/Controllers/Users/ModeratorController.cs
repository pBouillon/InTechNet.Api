using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Utils.Api;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Exception;
using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.User.Interfaces;
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
        /// <summary>
        /// TODO
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// TODO
        /// </summary>
        private readonly IUserService _userService;

        public ModeratorController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<ModeratorDto> Get()
        {
            return Ok(_authenticationService.GetCurrentModerator());
        }

        /// <summary>
        /// Login end point for a moderator
        /// </summary>
        /// <param name="authenticationDto">The login parameters as <see cref="AuthenticationDto" /></param>
        /// <returns>A valid JWT on success</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [SwaggerResponse(200, "Successful authentication")]
        [SwaggerResponse(401, "Invalid credentials")]
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

        /// <summary>
        /// Registration endpoint to create a new moderator
        /// </summary>
        /// <param name="newModeratorData">A <see cref="ModeratorDto" /> holding the new moderator's data</param>
        /// <returns>A JWT for the newly created user on success</returns>
        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse(200, "New moderator successfully added")]
        [SwaggerResponse(404, "Invalid payload")]
        [SwaggerOperation(
            Summary = "Registration endpoint to create a new moderator",
            Tags = new[]
            {
                SwaggerTag.Moderator,
                SwaggerTag.Registration
            }
        )]
        public IActionResult Register([FromBody] ModeratorRegistrationDto newModeratorData)
        {
            try
            {
                _userService.RegisterModerator(newModeratorData);

                var associatedToken = _authenticationService.GetModeratorToken(new AuthenticationDto
                {
                    Login = newModeratorData.Nickname,
                    Password = newModeratorData.Password
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
