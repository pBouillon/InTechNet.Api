﻿using InTechNet.Api.Attributes;
using InTechNet.Api.Errors.Classes;
using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Attendee;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Api;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Exception;
using InTechNet.Exception.Attendee;
using InTechNet.Exception.Hub;
using InTechNet.Exception.Registration;
using InTechNet.Services.Attendee.Interfaces;
using InTechNet.Services.Authentication.Interfaces;
using InTechNet.Services.Hub.Interfaces;
using InTechNet.Services.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;

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
        /// Attendee service for attendee related operations
        /// </summary>
        private readonly IAttendeeService _attendeeService;

        /// <summary>
        /// Authentication service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Hub service for hub related operations
        /// </summary>
        private readonly IHubService _hubService;

        /// <summary>
        /// User service for user related operations
        /// </summary>
        private readonly IPupilService _pupilService;

        /// <summary>
        /// Controller for hub endpoints relative to pupils management
        /// </summary>
        /// <param name="authenticationService">Authentication service</param>
        /// <param name="pupilService">Pupil service</param>
        /// <param name="hubService">Hub service</param>
        /// <param name="attendeeService">Attendee service</param>
        public PupilsController(IAttendeeService attendeeService, IAuthenticationService authenticationService, IHubService hubService, IPupilService pupilService)
        {
            _attendeeService = attendeeService;
            _authenticationService = authenticationService;
            _hubService = hubService;
            _pupilService = pupilService;
        }

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

        [HttpPost("me/Hubs/join")]
        [PupilClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.Created, "Hub successfully joined")]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "Invalid payload")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Pupil already joined the hub")]
        [SwaggerResponse((int) HttpStatusCode.Conflict, "Hub already at maximum capacity")]
        [SwaggerOperation(
            Summary = "Register the current pupil as an attendee of the hub associated to the provided link",
            Tags = new[]
            {
                SwaggerTag.Hubs,
                SwaggerTag.Pupils,
            }
        )]
        public ActionResult JoinHub(
            [FromQuery, SwaggerParameter("Link of the hub the pupil is joining")] string link)
        {
            try
            {
                var currentPupil = _authenticationService.GetCurrentPupil();

                _attendeeService.AddAttendee(currentPupil, link);

                return Created("Pupil added to hub", true);
            }
            catch (AttendeeAlreadyRegisteredException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
            catch (HubMaxAttendeeCountReachedException ex)
            {
                return Conflict(
                    new ConflictError(ex));
            }
            catch (UnknownHubException ex)
            {
                return BadRequest(
                    new BadRequestError(ex));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse((int) HttpStatusCode.OK, "New pupil successfully added")]
        [SwaggerResponse((int) HttpStatusCode.Conflict, "The pupil has a duplicated credential (login / email)")]
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
