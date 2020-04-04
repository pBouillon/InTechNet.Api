using InTechNet.Common.Dto.Modules;
using InTechNet.DataAccessLayer;
using InTechNet.Exception.Hub;
using InTechNet.Services.Module.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using InTechNet.Common.Dto.Topic;

namespace InTechNet.Services.Module
{
    /// <inheritdoc cref="IModuleService"/>
    public class ModuleService : IModuleService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly InTechNetContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public ModuleService(InTechNetContext context)
            => _context = context;

        /// <inheritdoc cref="IModuleService.GetModulesForHub"/>
        public IEnumerable<ModuleDto> GetModulesForHub(int idModerator, int idHub)
        {
            // Fetch the hub queried
            var hub = _context.Hubs
                          .Include(_ => _.Moderator)
                              .ThenInclude(_ => _.ModeratorSubscriptionPlan)
                          .SingleOrDefault(_ =>
                            _.IdHub == idHub)
                      ?? throw new UnknownHubException();

            // Assert the current moderator can query this hub
            var hubModerator = hub.Moderator;

            if (hubModerator.IdModerator != idModerator)
            {
                throw new UnauthorizedAccessException();
            }

            // Filter all hub to be displayed
            var modules = _context.Modules
                .Include(_ => _.SelectedModules)
                .Where(_ => _.SelectedModules.Any(_ => _.IdHub == idHub))
                .Where(_ =>
                    _.SubscriptionPlan.IdSubscriptionPlan
                        == hubModerator.ModeratorSubscriptionPlan.IdSubscriptionPlan)
                .Select(_ => new ModuleDto
                {
                    Id = _.IdModule,
                    Tags = new List<TagDto>(),  // TODO: GetTagList(module)
                    IsActive = false,  // _.SelectedModules.FirstOrDefault(selected => selected.IdModule == _.IdModule),  // TODO: IsActive(module, selectedModules)

                }).ToList();

            // Retrieve the associated modules
            return modules;
        }
    }
}
