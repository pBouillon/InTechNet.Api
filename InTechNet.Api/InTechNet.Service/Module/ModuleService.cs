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
                .Include(_ => _.SubscriptionPlan)
                .Include(_ => _.SelectedModules)
                .Where(_ => _.SelectedModules.Any(_ => _.IdHub == idHub))
                .Where(_ =>
                    _.SubscriptionPlan.IdSubscriptionPlan
                        == hubModerator.ModeratorSubscriptionPlan.IdSubscriptionPlan)
                .Select(_ => new ModuleDto
                {
                    Id = _.IdModule,
                    Tags = GetTagList(_.IdModule, _context),
                    IsActive = _.SelectedModules.Any(selected => selected.IdModule == _.IdModule),
                    ModuleName = _.ModuleName,
                    ModuleSubscriptionPlanDto = new Common.Dto.Subscription.LightweightSubscriptionPlanDto
                    {
                        IdSubscriptionPlan = _.SubscriptionPlan.IdSubscriptionPlan,
                        SubscriptionPlanName = _.SubscriptionPlan.SubscriptionPlanName,
                    }

                }).ToList();

            // Retrieve the associated modules
            return modules;
        }

        // TODO: check static / dbcontext in parameter thing, too late for this
        // If the method is not static -> exception at run time: possible memory leak
        // If the method is static -> can not access _context field directly
        // Context passed in method parameters.. don't if this is OK, but it works, so yay?
        private static IEnumerable<TagDto> GetTagList(int idModule, InTechNetContext context)
        {
           return context.Topics
                .Include(_ => _.Tag)
                .Where(_ =>
                    _.IdModule == idModule)
                .Select(_ => new TagDto
                {
                    Id = _.Tag.IdTag,
                    Name = _.Tag.Name,
                });

            
        }
    }
}
