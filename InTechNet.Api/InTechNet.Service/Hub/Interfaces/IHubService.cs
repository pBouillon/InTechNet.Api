using System.Collections.Generic;
using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Dto.User.Pupil;

namespace InTechNet.Services.Hub.Interfaces
{
    /// <summary>
    /// Service for hub's operations
    /// </summary>
    public interface IHubService
    {
        /// <summary>
        /// Register a new hub for the provided moderator
        /// </summary>
        /// <param name="moderatorDto">Current <see cref="ModeratorDto" /> data</param>
        /// <param name="newHubDto"><see cref="HubCreationDto" /> containing the minimal information for the hub to create</param>
        void CreateHub(ModeratorDto moderatorDto, HubCreationDto newHubDto);

        /// <summary>
        /// Delete a hub for a moderator
        /// </summary>
        /// <param name="moderatorDto">Current <see cref="ModeratorDto" /> data</param>
        /// <param name="hubId">Id of the hub to be deleted</param>
        void DeleteHub(ModeratorDto moderatorDto, int hubId);

        /// <summary>
        /// Retrieve the information for a specific hub
        /// </summary>
        /// <param name="moderatorDto">Current <see cref="ModeratorDto" /> data</param>
        /// <param name="hubId">The id of the hub to retriever</param>
        /// <returns>The <see cref="HubDto"/> containing the hub details</returns>
        HubDto GetModeratorHub(ModeratorDto moderatorDto, int hubId);

        /// <summary>
        /// Retrieve all hubs owned by the moderator matching the provided moderator's id
        /// </summary>
        /// <param name="moderatorDto">Current <see cref="ModeratorDto" /> data</param>
        /// <returns>An <see cref="IEnumerable&lt;LightweightHubDto&gt;" /> of its owned hubs</returns>
        IEnumerable<LightweightHubDto> GetModeratorHubs(ModeratorDto moderatorDto);

        /// <summary>
        /// Retrieve all hubs owned by the pupil matching the provided pupil's id
        /// </summary>
        /// <param name="currentPupil">Current <see cref="Pupil" /> data</param>
        /// <returns>An <see cref="IEnumerable&lt;PupilHubDto&gt;" /> of its owned hubs</returns>
        IEnumerable<PupilHubDto> GetPupilHubs(PupilDto currentPupil);

        /// <summary>
        /// Update the hub general information
        /// </summary>
        /// <param name="moderatorDto">Current <see cref="ModeratorDto" /> data</param>
        /// <param name="hubId">Id of the hub to be updated</param>
        /// <param name="hubUpdateDto">The <see cref="HubUpdateDto" /> data for the hub to be updated</param>
        void UpdateHub(ModeratorDto moderatorDto, int hubId, HubUpdateDto hubUpdateDto);
    }
}
