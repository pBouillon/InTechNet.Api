using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;

namespace InTechNet.Services.Authentication.Interfaces
{
    /// <summary>
    /// Authentication service providing helpers and utils for the authentication process
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Check if any of those credentials are in use
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>
        /// The <see cref="CredentialsCheckDto" /> with the <see cref="CredentialsCheckDto.AreUnique"/>
        /// property true if any provided credential is already in use; false otherwise
        /// </returns>
        CredentialsCheckDto AreCredentialsAlreadyInUse(CredentialsCheckDto credentials);

        /// <summary>
        /// Authenticate a moderator
        /// </summary>
        /// <returns>The <see cref="ModeratorDto"/> of the authenticated moderator</returns>
        ModeratorDto GetAuthenticatedModerator(AuthenticationDto authenticationDto);

        /// <summary>
        /// Generate a valid JWT for the pupil
        /// </summary>
        /// <returns>The valid JWT for the pupil</returns>
        public PupilDto GetAuthenticatedPupil(AuthenticationDto authenticationDto);

        /// <summary>
        /// Get the current moderator's data based on the JWT information
        /// </summary>
        /// <returns>The <see cref="ModeratorDto" /> holding the data associated with the moderator</returns>
        ModeratorDto GetCurrentModerator();

        /// <summary>
        /// Get the current pupil's data based on the JWT information
        /// </summary>
        /// <returns>The <see cref="PupilDto" /> holding the data associated with the pupil</returns>
        PupilDto GetCurrentPupil();

        /// <summary>
        /// Try get the current moderator
        /// </summary>
        /// <param name="moderatorDto">The current <see cref="ModeratorDto"/> object to be filled</param>
        /// <returns>True if found; false otherwise</returns>
        /// <remarks>moderatorDto will be null if the operation fails</remarks>
        bool TryGetCurrentModerator(out ModeratorDto moderatorDto);

        /// <summary>
        /// Try get the current pupil
        /// </summary>
        /// <param name="pupilDto">The current <see cref="PupilDto"/> object to be filled</param>
        /// <returns>True if found; false otherwise</returns>
        /// <remarks>pupilDto will be null if the operation fails</remarks>
        bool TryGetCurrentPupil(out PupilDto pupilDto);
    }
}
