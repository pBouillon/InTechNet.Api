using InTechNet.Api.Attributes;
using InTechNet.Api.Errors.Classes;
using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Utils.Api;
using InTechNet.Exception;
using InTechNet.Exception.Registration;
using InTechNet.Services.Authentication.Interfaces;
using InTechNet.Services.Hub.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using InTechNet.Common.Dto.User.Moderator;

namespace InTechNet.Api.Controllers.Hubs
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HubsController : ControllerBase
    {
        /// <summary>
        /// Authentication service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Hub service for hub related operations
        /// </summary>
        private readonly IHubService _hubService;

        /// <summary>
        /// Controller for hub related endpoints
        /// </summary>
        /// <param name="authenticationService">Authentication service</param>
        /// <param name="hubService">Hub service for hub related operations</param>
        public HubsController(IAuthenticationService authenticationService, IHubService hubService)
            => (_authenticationService, _hubService) = (authenticationService, hubService);

        [HttpPost]
        [ModeratorClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.OK, "Hub successfully created")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Hub creation failed")]
        [SwaggerOperation(
            Summary = "Creation endpoint to add a new hub to the currently logged moderator",
            Tags = new[]
            {
                SwaggerTag.Hubs
            }
        )]
        public IActionResult CreateHub(
            [FromBody, SwaggerParameter("Basic data for hub creation")] HubCreationDto hubCreation)
        {
            try
            {
                var currentModerator = _authenticationService.GetCurrentModerator();

                _hubService.CreateHub(currentModerator, hubCreation);

                return Ok();
            }
            catch (BaseException ex)
            {
                if (ex is DuplicatedIdentifierException)
                {
                    return Conflict(
                        new ConflictError(ex));
                }

                return BadRequest(
                    new BadRequestError(ex));
            }
        }

        [HttpDelete("{hubId}")]
        [ModeratorClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.NoContent, "Hub successfully deleted")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Hub deletion failed")]
        [SwaggerOperation(
            Summary = "Deletion endpoint to remove an existing hub",
            Tags = new[]
            {
                SwaggerTag.Hubs
            }
        )]
        public IActionResult DeleteHub(
            [FromRoute, SwaggerParameter("Id of the hub to be deleted")] int hubId)
        {
            try
            {
                var currentModerator = _authenticationService.GetCurrentModerator();

                _hubService.DeleteHub(currentModerator, hubId);

                return NoContent();
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }

        [HttpGet("{hubId}")]
        [SwaggerResponse((int) HttpStatusCode.OK, "Hub successfully fetched")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Hub fetching failed")]
        [SwaggerOperation(
            Summary = "Get the details of a requested hub",
            Tags = new[]
            {
                SwaggerTag.Hubs,
            }
        )]
        public ActionResult<HubDto> GetHub(int hubId)
        {
            HubDto hub;

            try
            {
                // Fetch the hub from the moderator
                if (_authenticationService.TryGetCurrentModerator(out var currentModerator))
                {
                    hub = _hubService.GetModeratorHub(currentModerator, hubId);

                    return Ok(hub);
                }

                // If the request came from the pupil, fetch the request from the pupil
                if (_authenticationService.TryGetCurrentPupil(out var currentPupil))
                {
                    hub = _hubService.GetPupilHub(currentPupil, hubId);

                    return Ok(hub);
                }

                // If the request does not come from any of the previous roles, return Unauthorized
                return Unauthorized();
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }

        [HttpGet]
        [ModeratorClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.OK, "Hubs successfully fetched")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Hubs fetching failed")]
        [SwaggerOperation(
            Summary = "Get a list of all hubs owned by the current moderator",
            Tags = new[]
            {
                SwaggerTag.Hubs,
            }
        )]
        public ActionResult<IEnumerable<LightweightHubDto>> GetHubs()
        {
            try
            {
                var currentModerator = _authenticationService.GetCurrentModerator();

                var hubs = _hubService.GetModeratorHubs(currentModerator);

                return Ok(hubs);
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }

        [HttpPut("{hubId}")]
        [ModeratorClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.OK, "Hub successfully updated")]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Hub update failed")]
        [SwaggerOperation(
            Summary = "Update hub's data",
            Tags = new[]
            {
                SwaggerTag.Hubs,
            }
        )]
        public IActionResult UpdateHub(
            [FromRoute, SwaggerParameter("Id of the hub to update")] int hubId,
            [FromBody, SwaggerParameter("Data for hub update")] HubUpdateDto hubUpdate)
        {
            try
            {
                var currentModerator = _authenticationService.GetCurrentModerator();

                _hubService.UpdateHub(currentModerator, hubId, hubUpdate);

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