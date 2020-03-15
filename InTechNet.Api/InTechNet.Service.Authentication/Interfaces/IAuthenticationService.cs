using InTechNet.Common.Utils.Authentication;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.Authentication.Interfaces
{
    /// <summary>
    /// Authentication service providing helpers and utils for the authentication process
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Get the current moderator's data based on the JWT information
        /// </summary>
        /// <returns>The <see cref="ModeratorDto" /> holding the data associated with the moderator</returns>
        ModeratorDto GetCurrentModerator();

        /// <summary>
        /// Generate a valid JWT for the moderator
        /// </summary>
        /// <returns>The valid JWT for the moderator</returns>
        string GetModeratorToken(AuthenticationDto authenticationDto);

        /// <summary>
        /// Generate a valid JWT for the pupil
        /// </summary>
        /// <returns>The valid JWT for the pupil</returns>
        public string GetPupilToken(AuthenticationDto authenticationDto);
    }
}
