using InTechNet.Common.Dto.Modules;
using InTechNet.Common.Dto.Topic;
using InTechNet.DataAccessLayer;
using InTechNet.Exception.Hub;
using InTechNet.Exception.Module;
using InTechNet.Services.Module.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using InTechNet.DataAccessLayer.Entities.Modules;

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
                    _.Id == idHub)
                ?? throw new UnknownHubException();

            // Assert the current moderator can query this hub
            var hubModerator = hub.Moderator;

            if (hubModerator.Id != idModerator)
            {
                throw new UnauthorizedAccessException();
            }

            // Filter all hub to be displayed
            return _context.Modules
                .Include(_ => _.SubscriptionPlan)
                .Include(_ => _.AvailableModules)
                // Only select the modules with the same subscription plan as the moderator
                // FIXME: "lower" subscription plans should be visible for higher subscription plans
                .Where(_ =>
                    _.SubscriptionPlan.Id
                        == hubModerator.ModeratorSubscriptionPlan.Id)
                // Mapping the result to List<ModuleDto>
                .Select(_ => new ModuleDto
                {
                    Id = _.Id,
                    // Retrieving all tags of the current module's topic to IEnumerable<TagDto>
                    Tags = _.Topics.Select(topic => new TagDto
                    {
                        Id = topic.Tag.Id,
                        Name = topic.Tag.Name
                    }),
                    // A module is active if its ID also belong to the SelectedModule table
                    IsActive = _.AvailableModules.Any(availableModules
                        => availableModules.Module.Id == _.Id
                           && availableModules.Hub.Id == hub.Id),
                    Name = _.ModuleName,
                    Description = _.ModuleDescription,
                    ModuleSubscriptionPlanDto = new Common.Dto.Subscription.LightweightSubscriptionPlanDto
                    {
                        IdSubscriptionPlan = _.SubscriptionPlan.Id,
                        SubscriptionPlanName = _.SubscriptionPlan.SubscriptionPlanName,
                    }
                });
        }

        /// <inheritdoc cref="IModuleService.GetPupilModules"/>
        public IEnumerable<PupilModuleDto> GetPupilModules(int idPupil, int idHub)
        {
            // Get the hub
            var hub = _context.Hubs.Include(_ => _.Moderator)
                .Include(_ => _.Attendees)
                    .ThenInclude(_ => _.Pupil)
                .FirstOrDefault(_ =>
                    _.Id == idHub
                        && _.Attendees.Any(_ =>
                            _.Pupil.Id == idPupil))
                ?? throw new UnknownHubException();

            // Return all available modules
            return _context.AvailableModules
                .Where(_ =>
                    _.Hub.Id == hub.Id)
                .Select(_ => new PupilModuleDto
                {
                    Id = _.Id,
                    Name = _.Module.ModuleName,
                    Description = _.Module.ModuleDescription,
                    // An available module is the current module 
                    // if its id is found in the current_module table
                    IsOnGoing = _context.CurrentModules
                        .Any(current =>
                            current.Module.Id == _.Module.Id),
                });
        }

        /// <inheritdoc cref="IModuleService.StartModule"/>
        public void StartModule(int idPupil, int idHub, int idModule)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IModuleService.ToggleModuleState"/>
        public void ToggleModuleState(int idModerator, int idHub, int idModule)
        {
            // Get the hub
            var hub = _context.Hubs.Include(_ => _.Moderator)
                .Include(_ => _.AvailableModules)
                    .ThenInclude(_ => _.Module)
                .FirstOrDefault(_ =>
                    _.Id == idHub
                        && _.Moderator.Id == idModerator)
                ?? throw new UnknownHubException();

            // Get the related module
            var module = _context.Modules.SingleOrDefault(_ =>
                    _.Id == idModule)
                ?? throw new UnknownModuleException();

            // Retrieve the available module record
            var selectedModule = hub.AvailableModules.SingleOrDefault(_ =>
                _.Module.Id == module.Id);

            // Add the current module to the selected ones if not tracked as available
            if (selectedModule == null)
            {
                _context.AvailableModules.Add(new AvailableModule
                {
                    Hub = hub,
                    Module = module,
                });
            }
            // Delete the selected module record if tracked
            else
            {
                _context.AvailableModules.Remove(selectedModule);
            }

            // Commit changes
            _context.SaveChanges();
        }
    }
}
