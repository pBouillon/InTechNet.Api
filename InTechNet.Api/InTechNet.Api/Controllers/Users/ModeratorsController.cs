using InTechNet.Api.Attributes;
using InTechNet.Api.Errors.Classes;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Attendee;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Utils.Api;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Exception;
using InTechNet.Exception.Registration;
using InTechNet.Services.Authentication.Interfaces;
using InTechNet.Services.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using InTechNet.Services.Hub.Interfaces;

namespace InTechNet.Api.Controllers.Users
{
    /// <summary>
    /// Authentication controller for InTechNet API for moderators
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ModeratorsController : ControllerBase
    {
        /// <summary>
        /// Authentication service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Hub service
        /// </summary>
        private readonly IHubService _hubService;

        /// <summary>
        /// User service for user related operations
        /// </summary>
        private readonly IModeratorService _moderatorService;

        /// <summary>
        /// Controller for hub endpoints relative to moderators management
        /// </summary>
        /// <param name="authenticationService">Authentication service</param>
        /// <param name="moderatorService">Moderator service</param>
        /// <param name="hubService">Hub service</param>
        public ModeratorsController(IAuthenticationService authenticationService, IModeratorService moderatorService, IHubService hubService)
            => (_authenticationService, _moderatorService, _hubService) = (authenticationService, moderatorService, hubService);

        [AllowAnonymous]
        [HttpGet("identifiers-checks")]
        [SwaggerResponse((int) HttpStatusCode.OK, "Email not already in use")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Email already used")]
        [SwaggerOperation(
            Summary = "Endpoint for the credential duplicates checks",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Moderators
            }
        )]
        public ActionResult<CredentialsCheckDto> AreIdentifiersAlreadyInUse(
            [FromQuery, SwaggerParameter("Credentials to check")] CredentialsCheckDto credentials)
        {
            return Ok(
                _authenticationService.AreCredentialsAlreadyInUse(credentials));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [SwaggerResponse((int) HttpStatusCode.OK, "Successful authentication")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Invalid credentials")]
        [SwaggerOperation(
            Summary = "Login endpoint for a moderator",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Moderators
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

        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse((int) HttpStatusCode.OK, "New moderator successfully added")]
        [SwaggerResponse((int) HttpStatusCode.Conflict, "The moderator has a duplicated credential (login / email)")]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "Invalid payload")]
        [SwaggerOperation(
            Summary = "Registration endpoint to create a new moderator",
            Tags = new[]
            {
                SwaggerTag.Moderators,
                SwaggerTag.Registrations
            }
        )]
        public IActionResult Register(
            [FromBody, SwaggerParameter("Moderator's creation payload")] ModeratorRegistrationDto newModeratorData)
        {
            try
            {
                _moderatorService.RegisterModerator(newModeratorData);

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

        [ModeratorClaimRequired]
        [HttpDelete("me/Hubs/{hubId}/Pupils/{pupilId}")]
        [SwaggerResponse((int) HttpStatusCode.OK, "Attendee successfully removed")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Invalid payload")]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "The provided data does not correspond")]
        [SwaggerOperation(
            Summary = "Remove a pupil attending the hub",
            Tags = new[]
            {
                SwaggerTag.Hubs,
                SwaggerTag.Moderators,
            }
        )]
        public IActionResult RemoveAttendee(
            [FromRoute, SwaggerParameter("Id of the hub from which the attendance is removed")] int hubId,
            [FromRoute, SwaggerParameter("Id of the attending pupil to be removed")] int pupilId,
            [FromBody, SwaggerParameter("Attendee to be removed")] AttendeeDto attendeeDto)
        {
            var currentModerator = _authenticationService.GetCurrentModerator();

            if (attendeeDto.IdHub != hubId
                || attendeeDto.IdPupil != pupilId)
            {
                return BadRequest();
            }

            try
            {
                _hubService.RemoveAttendance(currentModerator, attendeeDto);
                return Ok();
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }
    }
}
