using InTechNet.Common.Utils.Authentication;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.User.Interfaces
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
    }
}