using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Service.User.Interfaces;

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
            => (_moderatorService, _pupilService) = (moderatorService, pupilService);

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

        /// <inheritdoc cref="IUserService.GetModerator" />
        public ModeratorDto GetModerator(int moderatorId)
        {
            return _moderatorService.GetModerator(moderatorId);
        }

        /// <inheritdoc cref="IUserService.GetPupil" />
        public PupilDto GetPupil(int pupilId)
        {
            return _pupilService.GetPupil(pupilId);
        }

        /// <inheritdoc cref="IUserService.RegisterModerator" />
        public void RegisterModerator(ModeratorRegistrationDto newModeratorData)
        {
            _moderatorService.RegisterModerator(newModeratorData);
        }

        /// <inheritdoc cref="IUserService.RegisterPupil" />
        public void RegisterPupil(PupilRegistrationDto newPupilData)
        {
            _pupilService.RegisterPupil(newPupilData);
        }

        /// <inheritdoc cref="IUserService.IsEmailAlreadyInUse" />
        public bool IsEmailAlreadyInUse(string email)
        {
            return _moderatorService.IsEmailAlreadyInUse(email)
                || _pupilService.IsEmailAlreadyInUse(email);
        }

        /// <inheritdoc cref="IUserService.IsNicknameAlreadyInUse" />
        public bool IsNicknameAlreadyInUse(string nickname)
        {
            return _moderatorService.IsNicknameAlreadyInUse(nickname)
                || _pupilService.IsNicknameAlreadyInUse(nickname);
        }
    }
}
