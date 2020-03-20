using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;

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
        /// Get the moderator's data based on its identifier
        /// </summary>
        /// <param name="moderatorId">The moderator's identifier</param>
        /// <returns>A <see cref="ModeratorDto" /> containing the moderator's data</returns>
        ModeratorDto GetModerator(int moderatorId);

        /// <summary>
        /// Get the pupil's data based on its identifier
        /// </summary>
        /// <param name="pupilId">The pupil's identifier</param>
        /// <returns>A <see cref="PupilDto" /> containing the pupil's data</returns>
        PupilDto GetPupil(int pupilId);

        /// <summary>
        /// Create a new moderator in the database
        /// </summary>
        /// <param name="newModeratorData">A <see cref="ModeratorRegistrationDto" /> holding the new moderator's data</param>
        void RegisterModerator(ModeratorRegistrationDto newModeratorData);

        /// <summary>
        /// Create a new pupil in the database
        /// </summary>
        /// <param name="newPupilData">A <see cref="PupilRegistrationDto" /> holding the new pupil's data</param>
        void RegisterPupil(PupilRegistrationDto newPupilData);

        /// <summary>
        /// Check the email from the EmailDuplicationCheckDto
        /// </summary>
        /// <param name="emailDto">A <see cref="EmailDuplicationCheckDto" /> holding the email</param>
        /// /// <returns>A bool with value true if email is OK, false otherwise</returns>
        bool CheckEmailDuplicates(EmailDuplicationCheckDto emailDto);

        /// <summary>
        /// Check the nickname from the NicknameDuplicationCheckDto
        /// </summary>
        /// <param name="nicknameDto">A <see cref="NicknameDuplicationCheckDto" /> holding the nickname</param>
        /// /// <returns>A bool with value true if email is OK, false otherwise</returns>
        bool CheckNickNameDuplicates(NicknameDuplicationCheckDto nicknameDto);
    }
}