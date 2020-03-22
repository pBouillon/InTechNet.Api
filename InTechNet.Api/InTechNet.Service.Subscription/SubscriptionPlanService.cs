using InTechNet.Common.Dto.Subscription;
using InTechNet.DataAccessLayer;
using InTechNet.Service.Subscription.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace InTechNet.Service.Subscription
{
    /// <inheritdoc cref="ISubscriptionPlanService" />
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly InTechNetContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public SubscriptionPlanService(InTechNetContext context)
            => _context = context;

        /// <inheritdoc cref="ISubscriptionPlanService.GetAllSubscriptions" />
        public IEnumerable<SubscriptionPlanDto> GetAllSubscriptions()
        {
            return _context.SubscriptionPlans.Select( _ => new SubscriptionPlanDto
            {
                IdSubscriptionPlan = _.IdSubscriptionPlan,
                MaxHubPerModeratorAccount = _.MaxHubPerModeratorAccount,
                SubscriptionPlanName = _.SubscriptionPlanName,
                SubscriptionPlanPrice = _.SubscriptionPlanPrice,
                MaxAttendeesPerHub = _.MaxAttendeesPerHub
            });
        }
    }
}
