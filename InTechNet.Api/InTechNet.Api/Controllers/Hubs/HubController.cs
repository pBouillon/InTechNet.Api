using InTechNet.Api.Attributes;
using InTechNet.Api.Errors.Classes;
using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Utils.Api;
using InTechNet.Exception;
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
    public class HubController : ControllerBase
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
        public HubController(IAuthenticationService authenticationService, IHubService hubService)
            => (_authenticationService, _hubService) = (authenticationService, hubService);

        /// <summary>
        /// Creation endpoint to add a new hub to the currently logged moderator
        /// </summary>
        /// <param name="hubCreation"><see cref="HubCreationDto" /> holding information on the hub to be created</param>
        [HttpPost]
        [ModeratorClaimRequired]
        [SwaggerResponse(200, "Hub successfully created")]
        [SwaggerResponse(400, "Hub creation failed")]
        [SwaggerOperation(
            Summary = "Creation endpoint to add a new hub to the currently logged moderator",
            Tags = new[]
            {
                SwaggerTag.Hub
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
                return BadRequest(
                    new BadRequestError(ex));
            }
        }

        /// <summary>
        /// Deletion endpoint to remove an existing hub
        /// </summary>
        /// <param name="hubDeletion"><see cref="HubDeletionDto" /> holding information on the hub to be deleted</param>
        [HttpDelete]
        [ModeratorClaimRequired]
        [SwaggerResponse(200, "Hub successfully deleted")]
        [SwaggerResponse(401, "Hub deletion failed")]
        [SwaggerOperation(
            Summary = "Deletion endpoint to remove an existing hub",
            Tags = new[]
            {
                SwaggerTag.Hub
            }
        )]
        public IActionResult DeleteHub(
            [FromBody, SwaggerParameter("Basic data for hub deletion")] HubDeletionDto hubDeletion)
        {
            try
            {
                var currentModerator = _authenticationService.GetCurrentModerator();

                _hubService.DeleteHub(currentModerator, hubDeletion);

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