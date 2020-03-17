﻿using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User;
using System.Collections.Generic;
using InTechNet.Common.Dto.User.Moderator;

namespace InTechNet.Service.Hub.Interfaces
{
    /// <summary>
    /// Service for hub's operations
    /// </summary>
    public interface IHubService
    {
        /// <summary>
        /// Register a new hub for the provided moderator
        /// </summary>
        /// <param name="moderatorDto">Current <see cref="ModeratorDto" /> data</param>
        /// <param name="newHubDto"><see cref="HubCreationDto" /> containing the minimal information for the hub to create</param>
        void CreateHub(ModeratorDto moderatorDto, HubCreationDto newHubDto);

        /// <summary>
        /// Delete a hub for a moderator
        /// </summary>
        /// <param name="moderatorDto">Current <see cref="ModeratorDto" /> data</param>
        /// <param name="hubDeletionData"><see cref="HubDeletionDto" /> containing the minimal information for the hub to be deleted</param>
        void DeleteHub(ModeratorDto moderatorDto, HubDeletionDto hubDeletionData);

        /// <summary>
        /// Retrieve all hubs owned by the moderator matching the provided moderator's id
        /// </summary>
        /// <param name="moderatorId">The id of the moderator to look for</param>
        /// <returns>An <see cref="IEnumerable&lt;HubDto&gt;" /> of its owned hubs</returns>
        IEnumerable<HubDto> GetModeratorHubs(int moderatorId);
    }
}
