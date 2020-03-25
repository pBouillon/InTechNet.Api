using InTechNet.Api.Attributes;
using InTechNet.Api.Errors.Classes;
using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Api;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Exception;
using InTechNet.Exception.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using InTechNet.Services.Authentication.Interfaces;
using InTechNet.Services.Hub.Interfaces;
using InTechNet.Services.User.Interfaces;

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
        /// Authentication service
        /// </summary>
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// User service for user related operations
        /// </summary>
        private readonly IPupilService _pupilService;

        /// <summary>
        /// Hub service for hub related operations
        /// </summary>
        private readonly IHubService _hubService;

        /// <summary>
        /// Controller for hub endpoints relative to pupils management
        /// </summary>
        /// <param name="authenticationService">Authentication service</param>
        /// <param name="pupilService">Pupil service</param>
        /// <param name="hubService">Hub service</param>
        public PupilsController(IAuthenticationService authenticationService, IPupilService pupilService, IHubService hubService)
            => (_authenticationService, _pupilService, _hubService) = (authenticationService, pupilService, hubService);

        [AllowAnonymous]
        [HttpGet("identifiers-checks")]
        [SwaggerResponse(200, "Email not already in use")]
        [SwaggerResponse(401, "Email already used")]
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
        [SwaggerResponse(200, "Successful authentication")]
        [SwaggerResponse(401, "Invalid credentials")]
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Hubs successfully fetched")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Hubs fetching failed")]
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

        [AllowAnonymous]
        [HttpPost]
        [SwaggerResponse(200, "New pupil successfully added")]
        [SwaggerResponse(404, "Invalid payload")]
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
    }
}
