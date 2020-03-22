using System;
using System.Collections.Generic;
using System.Text;

namespace InTechNet.Common.Utils.SubscriptionPlan
{
    /// <summary>
    /// Class for the free subscription plan
    /// </summary>
    public class FreeSubscriptionPlan : BaseSubscriptionPlan
    {
        /// <summary>
        /// Name of the free subscription plan
        /// </summary>
        public override string SubscriptionPlanName
            => "standard";
    }
}
