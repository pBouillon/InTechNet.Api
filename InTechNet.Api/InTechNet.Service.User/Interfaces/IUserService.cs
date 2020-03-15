using InTechNet.Common.Utils.Authentication;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.User.Interfaces
{
    /// <summary>
    /// Service for various user operation handling wrapping both pupil and moderator service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Authenticate the moderator from its associated information
        /// </summary>
        /// <param name="authenticationData">The <see cref="AuthenticationDto" /> containing its authentication data</param>
        /// <returns>A <see cref="ModeratorDto" /> of the associated moderator</returns>
        ModeratorDto AuthenticateModerator(AuthenticationDto authenticationData);

        /// <summary>
        /// Authenticate the pupil from its associated information
        /// </summary>
        /// <param name="authenticationData">The <see cref="AuthenticationDto" /> containing its authentication data</param>
        /// <returns>A <see cref="PupilDto" /> of the associated pupil</returns>
        PupilDto AuthenticatePupil(AuthenticationDto authenticationData);

        /// <summary>
        /// Create a new moderator in the database
        /// </summary>
        /// <param name="newModeratorData">A <see cref="ModeratorDto" /> holding the new moderator's data</param>
        void RegisterModerator(ModeratorDto newModeratorData);
    }
}