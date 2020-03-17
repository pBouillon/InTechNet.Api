using InTechNet.Api.Attributes;
using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Utils.Api;
using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.Hub.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InTechNet.Api.Controllers.Hubs
{
    [Authorize]
    [Route("api/[controller]")]
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
        {
            _authenticationService = authenticationService;
            _hubService = hubService;
        }

        /// <summary>
        /// Creation endpoint to add a new hub to the currently logged moderator
        /// </summary>
        /// <param name="hub"><see cref="HubCreationDto" /> holding information on the hub to be created</param>
        /// <returns></returns>
        [HttpPost]
        [ModeratorClaimRequired]
        [SwaggerResponse(200, "Hub successfully created")]
        [SwaggerResponse(400, "Hub creation failed")]
        [SwaggerOperation(
            Summary = "Creation endpoint to add a new hub to the currently logged moderator",
            Tags = new[]
            {
                SwaggerTag.Hub,
                SwaggerTag.Moderator
            }
        )]
        public IActionResult CreateHub(
            [FromBody, SwaggerParameter("Basic data for hub creation")] HubCreationDto hub)
        {
            try
            {
                var currentModerator = _authenticationService.GetCurrentModerator();

                _hubService.CreateHub(currentModerator, hub);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }

    }
}