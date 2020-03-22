using System;
using System.Collections.Generic;
using System.Text;

namespace InTechNet.Exception.SubscriptionPlan
{
    /// <summary>
    /// Exception to be thrown when attempting to query an unknown subscription plan
    /// </summary>
    public class UnknownSubscriptionPlanException : BaseException
    {
        /// <summary>
        /// Custom exception message for this exception
        /// </summary>
        protected const string ExceptionMessage = "Unable to find this subscription plan in the given context";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="innerException">Nullable inner-exception</param>
        public UnknownSubscriptionPlanException(System.Exception innerException = null)
            : base(ExceptionMessage, innerException) { }
    }
}
