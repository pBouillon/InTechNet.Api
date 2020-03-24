﻿using System.Collections.Generic;
using System.Net;
using InTechNet.Api.Attributes;
using InTechNet.Api.Errors.Classes;
using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Utils.Api;
using InTechNet.Exception;
using InTechNet.Exception.Registration;
using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.Hub.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// Creation endpoint to add a new hub to the currently logged moderator
        /// </summary>
        /// <param name="hubCreation"><see cref="HubCreationDto" /> holding information on the hub to be created</param>
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

        /// <summary>
        /// Deletion endpoint to remove an existing hub
        /// </summary>
        /// <param name="hubId">Id the hub to be deleted</param>
        [HttpDelete("{hubId}")]
        [ModeratorClaimRequired]
        [SwaggerResponse((int) HttpStatusCode.OK, "Hub successfully deleted")]
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

                return Ok();
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }

        /// <summary>
        /// Get the details of a requested hub
        /// </summary>
        [HttpGet("{hubId}")]
        [ModeratorClaimRequired]
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
            try
            {
                var currentModerator = _authenticationService.GetCurrentModerator();

                var hub =_hubService.GetModeratorHub(currentModerator, hubId);

                return Ok(hub);
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }

        /// <summary>
        /// Get a list of all hubs owned by the current moderator
        /// </summary>
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

        /// <summary>
        /// Update a specific hub
        /// </summary>
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