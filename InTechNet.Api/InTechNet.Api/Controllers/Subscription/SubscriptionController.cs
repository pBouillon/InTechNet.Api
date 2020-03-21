﻿using InTechNet.Api.Attributes;
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
    public class SubscriptionController : ControllerBase
    {
        /// <summary>
        /// Subscription service for subscription related operations
        /// </summary>
        private readonly ISubscriptionService _subscriptionService;

        /// <summary>
        /// Controller for subscription related endpoints
        /// </summary>
        /// <param name="subscriptionService">Subscription service for subscription related operations</param>
        public SubscriptionController(ISubscriptionService subscriptionService)
            => (_subscriptionService) = (subscriptionService);

        /// <summary>
        /// Get a list of all hubs owned by the current moderator
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse(200, "Subscriptions successfully fetched")]
        [SwaggerResponse(401, "Subscriptions fetching failed")]
        [SwaggerOperation(
            Summary = "Get a list of all subcriptions available",
            Tags = new[]
            {
                SwaggerTag.Subscription,
            }
        )]
        public ActionResult<IEnumerable<SubscriptionDto>> GetHubs()
        {
            try
            {

                var subscriptions = _subscriptionService.GetAllSubscriptions();

                return Ok(subscriptions);
            }
            catch (BaseException ex)
            {
                return Unauthorized(
                    new UnauthorizedError(ex));
            }
        }
    }
}