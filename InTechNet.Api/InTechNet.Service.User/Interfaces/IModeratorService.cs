using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Utils.Authentication;

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
        ModeratorDto AuthenticateModerator(AuthenticationDto authenticationData);

        /// <summary>
        /// Create the hub from the HubDto
        /// </summary>
        /// <param name="hub"><see cref="HubDto" /> obtained from the current moderator</param>
        /// <param name="moderator"><see cref="ModeratorDto" /> containing the moderator's data</param>
        void CreateHub(HubDto hub, ModeratorDto moderator);

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
        void RegisterModerator(ModeratorDto newModeratorData);
    }
}