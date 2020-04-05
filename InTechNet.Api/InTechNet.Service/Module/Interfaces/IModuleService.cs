﻿using System.Collections.Generic;
using InTechNet.Common.Dto.Modules;
using InTechNet.Common.Dto.User.Pupil;

namespace InTechNet.Services.Module.Interfaces
{
    /// <summary>
    /// Service for module's operations
    /// </summary>
    public interface IModuleService
    {
        /// <summary>
        /// Get all modules available for a given hub
        /// </summary>
        /// <param name="idModerator">Id of the querying moderator</param>
        /// <param name="idHub">Id of the hub to query</param>
        /// <returns>A collection of <see cref="ModuleDto"/></returns>
        IEnumerable<ModuleDto> GetModulesForHub(int idModerator, int idHub);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPupil">Id of the querying pupil</param>
        /// <param name="idHub"></param>
        /// <returns>A collection of <see cref="PupilModuleDto"/></returns>
        IEnumerable<PupilModuleDto> GetPupilModules(int idPupil, int idHub);

        /// <summary>
        /// Add or remove the current module to the selected ones
        /// </summary>
        /// <param name="idModerator">Id of the current moderator</param>
        /// <param name="idHub">Id of the concerned hub</param>
        /// <param name="idModule">Id of the related module</param>
        void ToggleModuleState(int idModerator, int idHub, int idModule);
    }
}
