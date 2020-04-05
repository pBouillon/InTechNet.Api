using InTechNet.Api.Attributes;
using InTechNet.Api.Errors.Classes;
using InTechNet.Common.Dto.Modules;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Attendee;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Utils.Api;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Exception;
using InTechNet.Exception.Registration;
using InTechNet.Services.Authentication.Interfaces;
using InTechNet.Services.Hub.Interfaces;
using InTechNet.Services.Module.Interfaces;
using InTechNet.Services.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using InTechNet.Exception.Hub;
using InTechNet.Exception.Module;

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
        /// Module service for module related operations
        /// </summary>
        private readonly IModuleService _moduleService;

        /// <summary>
        /// Controller for hub endpoints relative to moderators management
        /// </summary>
        /// <param name="authenticationService">Authentication service</param>
        /// <param name="hubService">Hub service</param>
        /// <param name="moderatorService">Moderator service</param>
        /// <param name="moduleService">Module service</param>
        public ModeratorsController(IAuthenticationService authenticationService, IModeratorService moderatorService,
            IModuleService moduleService, IHubService hubService)
        {
            _authenticationService = authenticationService;
            _hubService = hubService;
            _moderatorService = moderatorService;
            _moduleService = moduleService;
        }

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
            Summary = "Login endpoint for a moderator",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Moderators
            }
        )]
        public ActionResult<ModeratorDto> Authenticate(
            [FromBody, SwaggerParameter("Moderator login details")]
            AuthenticationDto authenticationDto)
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

        [HttpDelete]
        [ModeratorClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.NoContent, "Moderator successfully deleted")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Moderator deletion failed")]
        [SwaggerOperation(
            Summary = "Deletion endpoint to remove an existing moderator",
            Tags = new[]
            {
                SwaggerTag.Moderators
            }
        )]
        public IActionResult DeleteHub()
        {
            try
            {
                var currentModerator = _authenticationService.GetCurrentModerator();

                _moderatorService.DeleteModerator(currentModerator);

                return NoContent();
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }

        [ModeratorClaimRequired]
        [HttpGet("me/Hubs/{idHub}/Modules")]
        [SwaggerResponse((int) HttpStatusCode.OK, "Hubs modules successfully fetched")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "The current user can't perform this action")]
        [SwaggerOperation(
            Summary = "Fetch the modules of the specified hub for the current moderator",
            Tags = new[]
            {
                SwaggerTag.Hubs,
                SwaggerTag.Moderators,
                SwaggerTag.Modules,
            }
        )]
        public ActionResult<IEnumerable<ModuleDto>> GetHubsModules(
            [FromRoute, SwaggerParameter("Id of the hub from which fetch the modules")] 
            int idHub)
        {
            ModeratorDto currentModerator;
            
            try
            {
                currentModerator = _authenticationService.GetCurrentModerator();
            }
            catch (BaseException ex)
            {
                return Unauthorized (ex);
            }

            var modules = _moduleService.GetModulesForHub(currentModerator.Id, idHub);

            return Ok(modules);
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
            [FromBody, SwaggerParameter("Moderator's creation payload")] 
            ModeratorRegistrationDto newModeratorData)
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
        [HttpDelete("me/Hubs/{idHub}/Pupils/{idPupil}")]
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
            [FromRoute, SwaggerParameter("Id of the hub from which the attendance is removed")]
            int idHub,
            [FromRoute, SwaggerParameter("Id of the attending pupil to be removed")]
            int idPupil,
            [FromBody, SwaggerParameter("Attendee to be removed")]
            AttendeeDto attendeeDto)
        {
            var currentModerator = _authenticationService.GetCurrentModerator();

            if (attendeeDto.IdHub != idHub
                || attendeeDto.IdPupil != idPupil)
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

        [ModeratorClaimRequired]
        [HttpPut("me/Hubs/{idHub}/Modules/{idModule}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "State of the module toggled")]
        [SwaggerOperation(
            Summary = "Toggle the activation of a module in a given hub",
            Tags = new[]
            {
                SwaggerTag.Hubs,
                SwaggerTag.Moderators,
                SwaggerTag.Modules,
            }
        )]
        public IActionResult ToggleModuleState(
            [FromRoute, SwaggerParameter("Id of the concerned hub")]
            int idHub,
            [FromRoute, SwaggerParameter("Id of the module to be toggled")]
            int idModule)
        {
            try
            {
                var currentModerator = _authenticationService.GetCurrentModerator();
                _moduleService.ToggleModuleState(currentModerator.Id, idHub, idModule);
                return Ok();
            }
            catch (BaseException ex)
            {
                if (ex is UnknownHubException 
                    || ex is UnknownModuleException)
                {
                    return BadRequest(ex);
                }

                return Unauthorized(ex);
            }
        }
    }
}
