using AutoFixture;
using InTechNet.DataAccessLayer.Context;
using InTechNet.Services.SubscriptionPlan;
using InTechNet.UnitTests.Extensions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using InTechNet.Common.Dto.Subscription;
using Xbehave;
using InTechNetUsers = InTechNet.DataAccessLayer.Entities.Users;

namespace InTechNet.UnitTests.Services.SubscriptionPlan
{
    /// <summary>
    /// SubscriptionPlanService testing methods
    /// </summary>
    public class SubscriptionPlanServiceTest
    {
        /// <summary>
        /// Fixture object for dummy test data
        /// </summary>
        private readonly Fixture _fixture = new Fixture();

        /// <summary>
        /// InTechNet context mock
        /// </summary>
        private Mock<IInTechNetContext> _context;

        /// <summary>
        /// Collection of SubscriptionPlan objects representing the database
        /// </summary>
        private ICollection<InTechNetUsers.SubscriptionPlan> _subscriptionPlans;

        /// <summary>
        /// SubscriptionPlan service
        /// </summary>
        private SubscriptionPlanService _subscriptionPlanService;

        /// <summary>
        /// Default constructor to setup AutoFixture behavior
        /// </summary>
        public SubscriptionPlanServiceTest()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(_
                    => _fixture.Behaviors.Remove(_));

            _fixture.Behaviors
                .Add(new OmitOnRecursionBehavior());
        }

        /// <summary>
        /// The background method is executed exactly once before each scenario
        /// </summary>
        [Background]
        public void Background()
        {
            "Given various subscription plans"
                .x(()
                    => _subscriptionPlans = _fixture.CreateMany<InTechNetUsers.SubscriptionPlan>()
                        .ToList());

            "And a database using them as records"
                .x(() =>
                {
                    _context = new Mock<IInTechNetContext>();

                    var subscriptionPlansDbSet = _subscriptionPlans.AsMockedDbSet();

                    _context.SetupGet(_ => _.SubscriptionPlans)
                        .Returns(subscriptionPlansDbSet.Object);
                });

            "And a subscription plans service"
                .x(()
                    => _subscriptionPlanService = new SubscriptionPlanService(_context.Object));
        }

        /// <summary>
        /// Check the behavior of the subscription plan service when fetching all subscription
        /// plans
        /// </summary>
        [Scenario]
        public void GetAllSubscriptionPlans(IEnumerable<SubscriptionPlanDto> subscriptionPlans)
        {
            "When fetching all subscription plans"
                .x(() 
                    => subscriptionPlans = _subscriptionPlanService.GetAllSubscriptionPlans());

            "Then we should have fetched all available subscription plans"
                .x(() =>
                {
                    subscriptionPlans.Count()
                        .Should()
                        .Be(_subscriptionPlans.Count);

                    foreach (var subscriptionPlan in _subscriptionPlans)
                    {
                        subscriptionPlans.Should()
                            .ContainSingle(_
                                => _.IdSubscriptionPlan == subscriptionPlan.Id
                                    && _.SubscriptionPlanName == subscriptionPlan.SubscriptionPlanName);
                    }
                });
        }
    }
}
