using InTechNet.Common.Dto.Subscription;
using InTechNet.DataAccessLayer;
using InTechNet.Service.Subscription.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace InTechNet.Service.Subscription
{
    public class SubscriptionService : ISubscriptionService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly InTechNetContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public SubscriptionService(InTechNetContext context)
            => _context = context;

        /// <inheritdoc cref="ISubscriptionService.getAllSubscriptions" />
        public IEnumerable<SubscriptionDto> GetAllSubscriptions()
        {
            

            var AllSubscription =  _context.Subscriptions.ToList();

            return AllSubscription.AsQueryable().Select( _ => new SubscriptionDto
            {
                IdSubscription = _.IdSubscription,
                HubMaxNumber = _.HubMaxNumber,
                SubscriptionName = _.SubscriptionName,
                SubscriptionPrice = _.SubscriptionPrice
            });
        }
    }
}
