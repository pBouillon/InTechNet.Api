using InTechNet.Common.Utils.Authentication;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.User.Interfaces
{
    public interface IUserService
    {
        ModeratorDto AuthenticateModerator(AuthenticationDto authenticationData);

        PupilDto AuthenticatePupil(AuthenticationDto authenticationData);
    }
}
