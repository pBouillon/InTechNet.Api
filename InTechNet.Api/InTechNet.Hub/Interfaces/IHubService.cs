using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User;
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
        /// <param name="hubData"></param>
        void CreateHub(HubDto newHubData, ModeratorDto moderatorDto);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="moderatorId"></param>
        /// <returns></returns>
        IEnumerable<HubDto> GetModeratorHubs(int moderatorId);
    }
}
