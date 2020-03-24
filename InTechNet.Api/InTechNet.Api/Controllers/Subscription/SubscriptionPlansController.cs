using InTechNet.Api.Attributes;
using InTechNet.Api.Errors.Classes;
using InTechNet.Common.Dto.Subscription;
using InTechNet.Common.Utils.Api;
using InTechNet.Exception;
using InTechNet.Service.Subscription.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace InTechNet.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SubscriptionPlansController : ControllerBase
    {
        /// <summary>
        /// Subscription service for subscription related operations
        /// </summary>
        private readonly ISubscriptionPlanService _subscriptionPlanService;

        /// <summary>
        /// Constructor for subscription related endpoints
        /// </summary>
        /// <param name="subscriptionPlanService">Subscription service for subscription related operations</param>
        public SubscriptionPlansController(ISubscriptionPlanService subscriptionPlanService)
            => _subscriptionPlanService = subscriptionPlanService;

        /// <summary>
        /// Get a list of all hubs owned by the current moderator
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse(200, "Subscriptions successfully fetched")]
        [SwaggerResponse(401, "Subscriptions fetching failed")]
        [SwaggerOperation(
            Summary = "Get a list of all subscriptions available",
            Tags = new[]
            {
                SwaggerTag.SubscriptionPlans,
            }
        )]
        public ActionResult<IEnumerable<SubscriptionPlanDto>> GetHubs()
        {
            try
            {
                var subscriptionPlans = _subscriptionPlanService.GetAllSubscriptions();

                return Ok(subscriptionPlans);
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }
    }
}
