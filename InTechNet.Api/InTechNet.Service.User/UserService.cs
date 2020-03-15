using InTechNet.Common.Utils.Authentication;
using InTechNet.Service.User.Interfaces;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.User
{
    /// <inheritdoc cref="IUserService" />
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

        /// <inheritdoc cref="IUserService.AuthenticateModerator" />
        public ModeratorDto AuthenticateModerator(AuthenticationDto authenticationData)
        {
            return _moderatorService.AuthenticateModerator(authenticationData);
        }

        /// <inheritdoc cref="IUserService.AuthenticatePupil" />
        public PupilDto AuthenticatePupil(AuthenticationDto authenticationData)
        {
            return _pupilService.AuthenticatePupil(authenticationData);
        }

        /// <inheritdoc cref="IUserService.RegisterModerator" />
        public void RegisterModerator(ModeratorDto newModeratorData)
        {
            _moderatorService.RegisterModerator(newModeratorData);
        }
    }
}
