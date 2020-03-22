using System;
using System.Collections.Generic;
using System.Text;

namespace InTechNet.Common.Utils.SubscriptionPlan
{
    /// <summary>
    /// Abstract class for all subscription plans
    /// </summary>
    public abstract class BaseSubscriptionPlan
    {
        /// <summary>
        /// Name of the free subscription plan
        /// </summary>
        public abstract string SubscriptionPlanName { get; }
    }
}
