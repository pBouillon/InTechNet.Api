﻿using InTechNet.Common.Dto.User;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;

namespace InTechNet.Service.Authentication.Interfaces
{
    /// <summary>
    /// Authentication service providing helpers and utils for the authentication process
    /// </summary>
    public interface IAuthenticationService
    {
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
        /// Check the email written by the user
        /// </summary>
        /// <returns>A bool with value true if email is OK, false otherwise</returns>
        bool IsEmailAlreadyInUse(EmailDuplicationCheckDto emailDto);

        /// <summary>
        /// Check the nickname written by the user
        /// </summary>
        /// <returns>A bool with value true if email is OK, false otherwise</returns>
        bool IsNicknameAlreadyInUse(NicknameDuplicationCheckDto nicknameDto);
    }
}
