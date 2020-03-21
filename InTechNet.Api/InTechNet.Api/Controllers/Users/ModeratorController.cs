using InTechNet.Api.Errors.Classes;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Utils.Api;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Exception;
using InTechNet.Exception.Registration;
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
        /// Authentication service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// User service for user related operations
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Controller for hub endpoints relative to moderators management
        /// </summary>
        /// <param name="authenticationService">Authentication service</param>
        /// <param name="userService">User service for user related operations</param>
        public ModeratorController(IAuthenticationService authenticationService, IUserService userService)
            => (_authenticationService, _userService) = (authenticationService, userService);

        /// <summary>
        /// Login end point for a moderator
        /// </summary>
        /// <param name="authenticationDto">The login parameters as <see cref="AuthenticationDto" /></param>
        /// <returns>A <see cref="ModeratorDto" /> holding the authenticated moderator's data</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [SwaggerResponse(200, "Successful authentication")]
        [SwaggerResponse(401, "Invalid credentials")]
        [SwaggerOperation(
            Summary = "Login endpoint for a moderator",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Moderator
            }
        )]
        public ActionResult<ModeratorDto> Authenticate(
            [FromBody, SwaggerParameter("Moderator login details")] AuthenticationDto authenticationDto)
        {
            try
            {
                return Ok(
                    _authenticationService.GetAuthenticatedModerator(authenticationDto));
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }


        /// <summary>
        /// Endpoint for the email duplication check
        /// </summary>
        /// <param name="emailDto">The login parameters as <see cref="EmailDuplicationCheckDto" /></param>
        /// <returns>A bool with value true if email is OK, false otherwise</returns>
        [AllowAnonymous]
        [HttpPost("emailCheck")]
        [SwaggerResponse(200, "Email not already in use")]
        [SwaggerResponse(401, "Email already used")]
        [SwaggerOperation(
            Summary = "Endpoint for the email check",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Moderator
            }
        )]
        public ActionResult<bool> IsEmailAlreadyInUse(
            [FromBody, SwaggerParameter("Email to check")] EmailDuplicationCheckDto emailDto)
        {
            try
            {
                return Ok(
                    _authenticationService.IsEmailAlreadyInUse(emailDto));
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }


        /// <summary>
        /// Endpoint for the nickname duplication check
        /// </summary>
        /// <param name="nicknameDto">The login parameters as <see cref="NicknameDuplicationCheckDto" /></param>
        /// <returns>A bool with value true if email is OK, false otherwise</returns>
        [AllowAnonymous]
        [HttpPost("nicknameCheck")]
        [SwaggerResponse(200, "Nickname not already in use")]
        [SwaggerResponse(401, "Nickname already used")]
        [SwaggerOperation(
            Summary = "Endpoint for the nickname check",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Moderator
            }
        )]
        public ActionResult<bool> IsNicknameAlreadyInUse(
            [FromBody, SwaggerParameter("Email to check")] NicknameDuplicationCheckDto nicknameDto)
        {
            try
            {
                return Ok(
                    _authenticationService.IsNicknameAlreadyInUse(nicknameDto));
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }



        /// <summary>
        /// Registration endpoint to create a new moderator
        /// </summary>
        /// <param name="newModeratorData">A <see cref="ModeratorDto" /> holding the new moderator's data</param>
        /// <returns>A <see cref="ModeratorDto" /> holding the authenticated moderator's data</returns>
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
        public IActionResult Register(
            [FromBody, SwaggerParameter("Moderator's creation payload")] ModeratorRegistrationDto newModeratorData)
        {
            try
            {
                _userService.RegisterModerator(newModeratorData);

                var authenticatedModerator = _authenticationService.GetAuthenticatedModerator(new AuthenticationDto
                {
                    Login = newModeratorData.Nickname,
                    Password = newModeratorData.Password
                });

                return Ok(authenticatedModerator);
            }
            catch (BaseException ex)
            {
                if (ex is DuplicatedEmailException
                    || ex is DuplicatedIdentifierException)
                {
                    return Conflict(
                        new ConflictError(ex));
                }

                return BadRequest(
                    new BadRequestError(ex));
            }
        }
    }
}
