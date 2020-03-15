﻿using InTechNet.Common.Utils.Authentication;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.User.Interfaces
{
    /// <summary>
    /// Service for moderator's operations
    /// </summary>
    public interface IModeratorService
    {
        /// <summary>
        /// Authenticate the moderator from its associated information
        /// </summary>
        /// <param name="authenticationData">The <see cref="AuthenticationDto" /> containing its authentication data</param>
        /// <returns>A <see cref="ModeratorDto" /> of the associated moderator</returns>
        public ModeratorDto AuthenticateModerator(AuthenticationDto authenticationData);

        /// <summary>
        /// Get the moderator's data based on its identifier
        /// </summary>
        /// <param name="moderatorId">The moderator's identifier</param>
        /// <returns>A <see cref="ModeratorDto" /> containing the moderator's data</returns>
        ModeratorDto GetModerator(int moderatorId);

        /// <summary>
        /// Create a new moderator in the database
        /// </summary>
        /// <param name="newModeratorData">A <see cref="ModeratorDto" /> holding the new moderator's data</param>
        public void RegisterModerator(ModeratorDto newModeratorData);
    }
}