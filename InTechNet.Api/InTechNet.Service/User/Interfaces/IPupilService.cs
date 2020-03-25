﻿using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;

namespace InTechNet.Services.User.Interfaces
{
    /// <summary>
    /// Service for pupil's operations
    /// </summary>
    public interface IPupilService
    {
        /// <summary>
        /// Authenticate the pupil from its associated information
        /// </summary>
        /// <param name="authenticationData">The <see cref="AuthenticationDto" /> containing its authentication data</param>
        /// <returns>A <see cref="PupilDto" /> of the associated pupil</returns>
        PupilDto AuthenticatePupil(AuthenticationDto authenticationData);

        /// <summary>
        /// Get the pupil's data based on its identifier
        /// </summary>
        /// <param name="pupilId">The pupil's identifier</param>
        /// <returns>A <see cref="PupilDto" /> containing the pupil's data</returns>
        PupilDto GetPupil(int pupilId);

        /// <summary>
        /// Create a new pupil in the database
        /// </summary>
        /// <param name="newPupilData">A <see cref="PupilRegistrationDto" /> holding the new pupil's data</param>
        void RegisterPupil(PupilRegistrationDto newPupilData);

        /// <summary>
        /// Check the nickname from the NicknameDuplicationCheckDto
        /// </summary>
        /// <param name="nickname">The nickname to be checked for duplicates</param>
        /// <returns>A bool with value true if email is OK, false otherwise</returns>
        bool IsNicknameAlreadyInUse(string nickname);

        /// <summary>
        /// Check the email from the EmailDuplicationCheckDto
        /// </summary>
        /// <param name="email">The email to be checked for duplicates</param>
        /// <returns>A bool with value true if email is OK, false otherwise</returns>
        bool IsEmailAlreadyInUse(string email);
    }
}