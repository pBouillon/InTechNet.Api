using System.Collections.Generic;
using InTechNet.Common.Dto.Subscription;

namespace InTechNet.Services.SubscriptionPlan.Interfaces
{
    /// <summary>
    /// Service for subscription plan's operations
    /// </summary>
    public interface ISubscriptionPlanService
    {
        /// <summary>
        /// Get all the subscription existing
        /// </summary>
        /// <returns>A list of <see cref="SubscriptionPlanDto" /> of all the subscriptions</returns>
        IEnumerable<SubscriptionPlanDto> GetAllSubscriptions(); 
    }
}
