using InTechNet.Common.Dto.Hub;
using System.Collections.Generic;

namespace InTechNet.Service.Hub.Interfaces
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IHubService
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="moderatorId"></param>
        /// <returns></returns>
        IEnumerable<HubDto> GetModeratorHubs(int moderatorId);
    }
}
