using System;
using InTechNet.Api.Attributes;
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
using InTechNet.Exception.Module;
using InTechNet.Exception.Registration;
using InTechNet.Services.Attendee.Interfaces;
using InTechNet.Services.Authentication.Interfaces;
using InTechNet.Services.Hub.Interfaces;
using InTechNet.Services.Module.Interfaces;
using InTechNet.Services.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using InTechNet.Common.Dto.Resource;

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
        /// Module service for module related operations
        /// </summary>
        private readonly IModuleService _moduleService;

        /// <summary>
        /// User service for user related operations
        /// </summary>
        private readonly IPupilService _pupilService;

        /// <summary>
        /// Controller for hub endpoints relative to pupils management
        /// </summary>
        /// <param name="authenticationService">Authentication service</param>
        /// <param name="attendeeService">Attendee service</param>
        /// <param name="hubService">Hub service</param>
        /// <param name="moduleService">Module service</param>
        /// <param name="pupilService">Pupil service</param>
        public PupilsController(IAttendeeService attendeeService, IAuthenticationService authenticationService,
            IHubService hubService, IModuleService moduleService, IPupilService pupilService)
        {
            _attendeeService = attendeeService;
            _authenticationService = authenticationService;
            _hubService = hubService;
            _moduleService = moduleService;
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
            [FromQuery, SwaggerParameter("Credentials to check")]
            CredentialsCheckDto credentials)
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
            [FromBody, SwaggerParameter("Pupil login details")]
            AuthenticationDto authenticationDto)
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

        [HttpDelete]
        [PupilClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.NoContent, "Pupil successfully deleted")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Pupil deletion failed")]
        [SwaggerOperation(
            Summary = "Deletion endpoint to remove an existing pupil",
            Tags = new[]
            {
                SwaggerTag.Pupils
            }
        )]
        public IActionResult DeleteHub()
        {
            try
            {
                var currentPupil = _authenticationService.GetCurrentPupil();

                _pupilService.DeletePupil(currentPupil);

                return NoContent();
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }

        [PupilClaimRequired]
        [SwaggerResponse((int)HttpStatusCode.OK, "Module successfully finished")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "The attendee does not exists in the current hub")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Unable to finish this module")]
        [HttpDelete("me/Hubs/{idHub}/Modules/{idModule}/States/current")]
        [SwaggerOperation(
            Summary = "Finish the resource currently in progress for the given pupil in a given hub",
            Tags = new[]
            {
                SwaggerTag.Modules,
                SwaggerTag.Pupils,
            }
        )]
        public IActionResult FinishModule(
            [FromRoute, SwaggerParameter("Id of the hub in which the module is")]
            int idHub,
            [FromRoute, SwaggerParameter("Id of the module to finish")]
            int idModule)
        {
            try
            {
                var currentPupil = _authenticationService.GetCurrentPupil();

                _moduleService.FinishModule(currentPupil.Id, idHub, idModule);

                return Ok();
            }
            catch (BaseException ex)
            {
                if (ex is UnknownAttendeeException)
                {
                    return Unauthorized(ex);
                }

                return BadRequest(ex);
            }
        }

        [PupilClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.OK, "Resource successfully fetched")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "The attendee does not exists in the current hub")]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "Unable to fetch the resource")]
        [HttpGet("me/Hubs/{idHub}/Modules/{idModule}/Resources/current")]
        [SwaggerOperation(
            Summary = "Retrieve the current resource of a module for a given pupil in a given hub",
            Tags = new[]
            {
                SwaggerTag.Modules,
                SwaggerTag.Pupils,
            }
        )]
        public ActionResult<ResourceDto> GetCurrentResource(
            [FromRoute, SwaggerParameter("Id of the hub in which the module is")]
            int idHub,
            [FromRoute, SwaggerParameter("Id of the module from which fetching the current resource")]
            int idModule)
        {
            try
            {
                var currentPupil = _authenticationService.GetCurrentPupil();

                var currentResource = _moduleService.GetCurrentResource(currentPupil.Id, idHub, idModule);

                return Ok(currentResource);
            }
            catch (BaseException ex)
            {
                if (ex is UnknownAttendeeException)
                {
                    return Unauthorized(ex);
                }

                return BadRequest(ex);
            }
        }

        [HttpGet("me/Hubs/{hubLink}")]
        [SwaggerResponse((int) HttpStatusCode.OK, "Hub successfully fetched")]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "Invalid payload")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Hub fetching failed")]
        [SwaggerOperation(
            Summary = "Get the details of a requested hub by its link",
            Tags = new[]
            {
                SwaggerTag.Hubs,
            }
        )]
        public ActionResult<HubDto> GetHubByLink(
            [FromRoute, SwaggerParameter("Link of the hub to be checked")] 
            string hubLink)
        {
            try
            {
                var currentPupil = _authenticationService.GetCurrentPupil();

                var hub = _pupilService.GetHubByLink(currentPupil, hubLink);

                return Ok(hub);
            }
            catch (UnknownHubException ex)
            {
                return BadRequest(
                    new BadRequestError(ex));
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

        [HttpGet("me/Hubs/{idHub}/Modules")]
        [PupilClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.OK, "Modules successfully fetched")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Modules fetching failed")]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "The provided data does not correspond to any resource")]
        [SwaggerOperation(
            Summary = "Get a list of all modules the current pupil can see in the specified hub",
            Tags = new[]
            {
                SwaggerTag.Modules,
                SwaggerTag.Pupils
            }
        )]
        public ActionResult<IEnumerable<PupilHubDto>> GetHubSelectedModules(
            [FromRoute, SwaggerParameter("Id of the hub from which the modules are fetched")]
            int idHub)
        {
            try
            {
                var currentPupil = _authenticationService.GetCurrentPupil();

                var hubs = _moduleService.GetPupilModules(currentPupil.Id, idHub);

                return Ok(hubs);
            }
            catch (BaseException ex)
            {
                if (ex is UnknownHubException
                    || ex is UnknownModuleException)
                {
                    return BadRequest(ex);
                }

                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }

        [HttpPost("me/Hubs/join")]
        [PupilClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.Created, "Hub successfully joined")]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "Invalid payload")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Pupil already joined the hub")]
        [SwaggerResponse((int) HttpStatusCode.Conflict, "Hub already at its maximum capacity")]
        [SwaggerOperation(
            Summary = "Register the current pupil as an attendee of the hub associated to the provided link",
            Tags = new[]
            {
                SwaggerTag.Hubs,
                SwaggerTag.Pupils,
            }
        )]
        public ActionResult JoinHub(
            [FromQuery, SwaggerParameter("Link of the hub the pupil is joining")] 
            string link)
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
            [FromBody, SwaggerParameter("Pupil's creation payload")] 
            PupilRegistrationDto newPupilData)
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
            [FromRoute, SwaggerParameter("Id of the hub from which the attendance is removed")] 
            int hubId,
            [FromBody, SwaggerParameter("Attendee to be removed")] 
            AttendeeDto attendeeDto)
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

        [PupilClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.OK, "Module successfully started")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "The attendee does not exists in the current hub")]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "Unable to start this module")]
        [HttpPost("me/Hubs/{idHub}/Modules/{idModule}/States/current")]
        [SwaggerOperation(
            Summary = "Begin the specified module",
            Tags = new[]
            {
                SwaggerTag.Modules,
                SwaggerTag.Pupils,
            }
        )]
        public IActionResult StartModule(
            [FromRoute, SwaggerParameter("Id of the hub in which the module is")]
            int idHub,
            [FromRoute, SwaggerParameter("Id of the module to start")]
            int idModule)
        {
            try
            {
                var currentPupil = _authenticationService.GetCurrentPupil();

                _moduleService.StartModule(currentPupil.Id, idHub, idModule);

                return Ok();
            }
            catch (BaseException ex)
            {
                if (ex is UnknownAttendeeException)
                {
                    return Unauthorized(ex);
                }

                return BadRequest(ex);
            }
        }

        [PupilClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.OK, "Current resource successfully updated")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "The attendee does not exists in the current hub")]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "Unable to go to the next resource of this module")]
        [HttpPut("me/Hubs/{idHub}/Modules/{idModule}/States/current")]
        [SwaggerOperation(
                    Summary = "Validate the current resource and go to the next one for the current module",
                    Tags = new[]
                    {
                SwaggerTag.Modules,
                SwaggerTag.Pupils,
                    }
                )]
        public IActionResult ValidateCurrentResource(
            [FromRoute, SwaggerParameter("Id of the hub in which the module is")]
            int idHub,
            [FromRoute, SwaggerParameter("Id of the module to start")]
            int idModule)
        {
            throw new NotImplementedException();
        }
    }
}
