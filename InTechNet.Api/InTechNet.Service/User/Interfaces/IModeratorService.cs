using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Utils.Authentication;

namespace InTechNet.Services.User.Interfaces
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
        /// Delete the current pupil
        /// </summary>
        /// <param name="currentModerator">Current <see cref="ModeratorDto" /> data</param>
        void DeleteModerator(ModeratorDto moderatorDto);

        /// <summary>
        /// Get the moderator's data based on its identifier
        /// </summary>
        /// <param name="moderatorId">The moderator's identifier</param>
        /// <returns>A <see cref="ModeratorDto" /> containing the moderator's data</returns>
        ModeratorDto GetModerator(int moderatorId);

        /// <summary>
        /// Create a new moderator in the database
        /// </summary>
        /// <param name="newModeratorData">A <see cref="ModeratorRegistrationDto" /> holding the new moderator's data</param>
        void RegisterModerator(ModeratorRegistrationDto newModeratorData);

        /// <summary>
        /// Check the email from the EmailDuplicationCheckDto
        /// </summary>
        /// <param name="email">The email to be checked for duplicates</param>
        /// <returns>A bool with value true if email is OK, false otherwise</returns>
        bool IsEmailAlreadyInUse(string email);

        /// <summary>
        /// Check the nickname from the NicknameDuplicationCheckDto
        /// </summary>
        /// <param name="nickname">The nickname to be checked for duplicates</param>
        /// <returns>A bool with value true if email is OK, false otherwise</returns>
        bool IsNicknameAlreadyInUse(string nickname);
    }
}