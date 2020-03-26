using InTechNet.Api.Attributes;
using InTechNet.Api.Errors.Classes;
using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Api;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Exception;
using InTechNet.Exception.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using InTechNet.Services.Authentication.Interfaces;
using InTechNet.Services.Hub.Interfaces;
using InTechNet.Services.User.Interfaces;
using InTechNet.Common.Dto.User.Attendee;

namespace InTechNet.Api.Controllers.Users
{
    /// <summary>
    /// Authentication controller for InTechNet API for pupils
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PupilsController : ControllerBase
    {
        /// <summary>
        /// Authentication service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// User service for user related operations
        /// </summary>
        private readonly IPupilService _pupilService;

        /// <summary>
        /// Hub service for hub related operations
        /// </summary>
        private readonly IHubService _hubService;

        /// <summary>
        /// Controller for hub endpoints relative to pupils management
        /// </summary>
        /// <param name="authenticationService">Authentication service</param>
        /// <param name="pupilService">Pupil service</param>
        /// <param name="hubService">Hub service</param>
        public PupilsController(IAuthenticationService authenticationService, IPupilService pupilService, IHubService hubService)
            => (_authenticationService, _pupilService, _hubService) = (authenticationService, pupilService, hubService);

        [AllowAnonymous]
        [HttpGet("identifiers-checks")]
        [SwaggerResponse((int) HttpStatusCode.OK, "Status of the provided credentials successfully fetched")]
        [SwaggerOperation(
            Summary = "Endpoint for the credential duplicates checks",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Pupils
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
            Summary = "Login endpoint for a pupil",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Pupils
            }
        )]
        public ActionResult<string> Authenticate(
            [FromBody, SwaggerParameter("Pupil login details")] AuthenticationDto authenticationDto)
        {
            try
            {
                return Ok(
                    _authenticationService.GetAuthenticatedPupil(authenticationDto));
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }

        [HttpGet("me/Hubs")]
        [PupilClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.OK, "Hubs successfully fetched")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Hubs fetching failed")]
        [SwaggerOperation(
            Summary = "Get a list of all hubs owned by the current pupil",
            Tags = new[]
            {
                SwaggerTag.Pupils
            }
        )]
        public ActionResult<IEnumerable<PupilHubDto>> GetHubs()
        {
            try
            {
                var currentPupil = _authenticationService.GetCurrentPupil();

                var hubs = _hubService.GetPupilHubs(currentPupil);

                return Ok(hubs);
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse((int) HttpStatusCode.OK, "New pupil successfully added")]
        [SwaggerResponse((int)HttpStatusCode.Conflict, "The pupil has a duplicated credential (login / email)")]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "Invalid payload")]
        [SwaggerOperation(
            Summary = "Registration endpoint to create a new pupil",
            Tags = new[]
            {
                SwaggerTag.Pupils,
                SwaggerTag.Registrations
            }
        )]
        public IActionResult Register(
            [FromBody, SwaggerParameter("Pupil's creation payload")] PupilRegistrationDto newPupilData)
        {
            try
            {
                _pupilService.RegisterPupil(newPupilData);

                var authenticatedPupil = _authenticationService.GetAuthenticatedPupil(new AuthenticationDto
                {
                    Login = newPupilData.Nickname,
                    Password = newPupilData.Password
                });

                return Ok(authenticatedPupil);
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


        [PupilClaimRequired]
        [HttpDelete("me/Hubs/{hubId}")]
        [SwaggerResponse((int) HttpStatusCode.OK, "Attendee successfully removed")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Invalid payload")]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "The provided data does not correspond")]
        [SwaggerOperation(
            Summary = "Remove the logged in pupil from the specified hub",
            Tags = new[]
            {
                SwaggerTag.Hubs,
                SwaggerTag.Pupils,
            }
        )]
        public IActionResult RemoveAttendee(
            [FromRoute, SwaggerParameter("Id of the hub from which the attendance is removed")] int hubId,
            [FromBody, SwaggerParameter("Attendee to be removed")] AttendeeDto attendeeDto)
        {
            var currentPupil = _authenticationService.GetCurrentPupil();

            if (attendeeDto.IdHub != hubId)
            {
                return BadRequest();
            }

            try
            {
                _hubService.RemoveAttendance(currentPupil, attendeeDto);
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
