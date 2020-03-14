﻿using InTechNet.Common.Utils.Authentication;
using InTechNet.Service.User.Interfaces;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.User
{
    /// <summary>
    /// Service for various user operation handling wrapping both pupil and moderator service 
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Service for moderator's operations 
        /// </summary>
        private readonly IModeratorService _moderatorService;

        /// <summary>
        /// Service for pupil's operations 
        /// </summary>
        private readonly IPupilService _pupilService;

        /// <summary>
        /// Service for various user operations handling wrapping both pupil and moderator service
        /// </summary>
        /// <param name="moderatorService">Service for moderator's operations</param>
        /// <param name="pupilService">Service for pupil's operations</param>
        public UserService(IModeratorService moderatorService, IPupilService pupilService)
        {
            _moderatorService = moderatorService;
            _pupilService = pupilService;
        }

        /// <summary>
        /// Authenticate the moderator from its associated information
        /// </summary>
        /// <param name="authenticationData">The <see cref="AuthenticationDto"/> containing its authentication data</param>
        /// <returns>A <see cref="ModeratorDto"/> of the associated moderator</returns>
        public ModeratorDto AuthenticateModerator(AuthenticationDto authenticationData)
        {
            return _moderatorService.AuthenticateModerator(authenticationData);
        }

        /// <summary>
        /// Authenticate the pupil from its associated information
        /// </summary>
        /// <param name="authenticationData">The <see cref="AuthenticationDto"/> containing its authentication data</param>
        /// <returns>A <see cref="PupilDto"/> of the associated pupil</returns>
        public PupilDto AuthenticatePupil(AuthenticationDto authenticationData)
        {
            return _pupilService.AuthenticatePupil(authenticationData);
        }
    }
}
