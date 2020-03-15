using InTechNet.Common.Utils.Authentication;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.Authentication.Interfaces
{
    /// <summary>
    /// Contract for JWT usage and generation
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Generate a valid JWT for the moderator
        /// </summary>
        /// <param name="moderator">The moderator for which generate the token</param>
        /// <returns>The valid JWT for the moderator</returns>
        string GetModeratorToken(ModeratorDto moderator);

        /// <summary>
        /// Generate a valid JWT for the pupil
        /// </summary>
        /// <param name="authenticationDto"></param>
        /// <returns>The valid JWT for the pupil</returns>
        string GetPupilToken(AuthenticationDto authenticationDto);
    }
}
