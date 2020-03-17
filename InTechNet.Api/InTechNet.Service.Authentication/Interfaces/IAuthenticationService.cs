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
