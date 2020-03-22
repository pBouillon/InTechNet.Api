using InTechNet.Common.Dto.Subscription;
using System.Collections.Generic;

namespace InTechNet.Service.Subscription.Interfaces
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
