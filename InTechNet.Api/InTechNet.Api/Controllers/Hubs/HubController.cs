using InTechNet.Api.Attributes;
using InTechNet.Common.Dto.Hub;
using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.Hub.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InTechNet.Api.Controllers.Hubs
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HubController : ControllerBase
    {
        /// <summary>
        /// TODO
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// TODO
        /// </summary>
        private readonly IHubService _hubService;

        public HubController(IAuthenticationService authenticationService, IHubService hubService)
        {
            _authenticationService = authenticationService;
            _hubService = hubService;
        }

        [HttpPost]
        [ModeratorClaimRequired]
        public IActionResult CreateHub([FromBody] HubDto hub)
        {
            try
            {

                var currentModerator = _authenticationService.GetCurrentModerator();

                _hubService.CreateHub(hub, currentModerator);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }

        }

    }
}