using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Moderator;
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
        /// <param name="emailDto">A <see cref="EmailDuplicationCheckDto" /> holding the email</param>
        /// <returns>A bool with value true if email is OK, false otherwise</returns>
        bool CheckEmailDuplicates(EmailDuplicationCheckDto emailDto);

        /// <summary>
        /// Check the nickname from the NicknameDuplicationCheckDto
        /// </summary>
        /// <param name="nicknameDto">A <see cref="NicknameDuplicationCheckDto" /> holding the nickname</param>
        /// <returns>A bool with value true if email is OK, false otherwise</returns>
        bool CheckNickNameDuplicates(NicknameDuplicationCheckDto nicknameDto);
    }
}