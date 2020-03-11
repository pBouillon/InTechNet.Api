using InTechNet.Common.Utils.Api;
using InTechNet.Service.Authentication.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InTechNet.Api.Controllers.Users
{
    /// <summary>
    /// Authentication controller for InTechNet API for pupils
    /// </summary>
    [Route("api/v1/pupil/[controller]")]
    [ApiController]
    public class PupilController : ControllerBase
    {
        /// <summary>
        /// Login end point for a pupil
        /// </summary>
        /// <param name="loginDto">The login parameters as <see cref="LoginDto"/></param>
        /// <returns>A valid JWT on success</returns>
        [HttpPost("login")]
        [SwaggerResponse(200, "Successful authentication")]
        [SwaggerResponse(400, "Invalid credentials")]
        [SwaggerOperation(
            Summary = "Login end point for a pupil",
            Tags = new[]
            {
                SwaggerTag.Authentication,
                SwaggerTag.Pupil
            }
        )]
        public ActionResult<string> Login([FromBody] LoginDto loginDto)
        {
            // Todo
            return Ok();
        }
    }
}
