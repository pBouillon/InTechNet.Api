using InTechNet.Common.Dto.Modules;
using InTechNet.Common.Dto.Resource;
using System.Collections.Generic;

namespace InTechNet.Services.Module.Interfaces
{
    /// <summary>
    /// Service for module's operations
    /// </summary>
    public interface IModuleService
    {
        /// <summary>
        /// Terminate the module finished by the pupil
        /// </summary>
        /// <param name="idPupil">Id of the pupil that will finish the module</param>
        /// <param name="idHub">id of the hub in which the pupil is</param>
        /// <param name="idModule">id of the module to finish</param>
        void FinishModule(int idPupil, int idHub, int idModule);

        /// <summary>
        /// Get the current resource of a pupil for a module in a given hub
        /// </summary>
        /// <param name="idPupil">Id of the pupil completing the module</param>
        /// <param name="idHub">Id of the hub in which this module is provided</param>
        /// <param name="idModule">Id of the module in progress</param>
        /// <returns>The content of the resource as a <see cref="ResourceDto"/></returns>
        ResourceDto GetCurrentResource(int idPupil, int idHub, int idModule);

        /// <summary>
        /// Get all modules available for a given hub
        /// </summary>
        /// <param name="idModerator">Id of the querying moderator</param>
        /// <param name="idHub">Id of the hub to query</param>
        /// <returns>A collection of <see cref="ModuleDto"/></returns>
        IEnumerable<ModuleDto> GetModulesForHub(int idModerator, int idHub);

        /// <summary>
        /// Get all modules for the current pupil in a given hub
        /// </summary>
        /// <param name="idPupil">Id of the querying pupil</param>
        /// <param name="idHub">Id of the hub queried</param>
        /// <returns>A collection of <see cref="PupilModuleDto"/></returns>
        IEnumerable<PupilModuleDto> GetPupilModules(int idPupil, int idHub);

        /// <summary>
        /// Begin a module for a pupil
        /// </summary>
        /// <param name="idPupil">Id of the pupil that is about to start the module</param>
        /// <param name="idHub">id of the hub in which the pupil is</param>
        /// <param name="idModule">id of the module to start</param>
        ResourceDto StartModule(int idPupil, int idHub, int idModule);

        /// <summary>
        /// Add or remove the current module to the selected ones
        /// </summary>
        /// <param name="idModerator">Id of the current moderator</param>
        /// <param name="idHub">Id of the concerned hub</param>
        /// <param name="idModule">Id of the related module</param>
        void ToggleModuleState(int idModerator, int idHub, int idModule);

        /// <summary>
        /// Validate the current resource of the user and set its current resource as the next one
        /// </summary>
        /// <param name="idPupil">Id of the pupil</param>
        /// <param name="idHub">id of the hub in which the pupil is</param>
        /// <param name="idModule">id of the module in which the pupil is progressing</param>
        void ValidateCurrentResource(int idPupil, int idHub, int idModule);
    }
}
