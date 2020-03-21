﻿using InTechNet.Common.Dto.Subscription;
using System.Collections.Generic;

namespace InTechNet.Service.Subscription.Interfaces
{
    public interface ISubscriptionService
    {
        /// <summary>
        /// Get all the subscription existing
        /// </summary>
        /// <returns>A list of <see cref="SubscriptionDto" /> of all the subscription</returns>
        IEnumerable<SubscriptionDto> GetAllSubscriptions(); 
    }
}