using InTechNet.Service.Authentication.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace InTechNet.Api.Controllers
{
    /// <summary>
    /// Authentication controller for InTechNet API
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginDto loginDto)
        {
            return Ok();
        }
    }
}