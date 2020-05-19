using System.Collections.Generic;
using System.Linq;
using InTechNet.Common.Dto.Subscription;
using InTechNet.DataAccessLayer;
using InTechNet.Services.SubscriptionPlan.Interfaces;

namespace InTechNet.Services.SubscriptionPlan
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
                IdSubscriptionPlan = _.Id,
                MaxAttendeesPerHub = _.MaxAttendeesPerHub,
                MaxHubPerModeratorAccount = _.MaxHubPerModeratorAccount,
                MaxModulePerHub = _.MaxModulePerHub,
                SubscriptionPlanName = _.SubscriptionPlanName,
                SubscriptionPlanPrice = _.SubscriptionPlanPrice,
            });
        }
    }
}
