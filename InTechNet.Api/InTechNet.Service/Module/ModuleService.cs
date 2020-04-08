using InTechNet.Common.Dto.Modules;
using InTechNet.Common.Dto.Topic;
using InTechNet.DataAccessLayer;
using InTechNet.DataAccessLayer.Entities.Modules;
using InTechNet.DataAccessLayer.Entities.Resources;
using InTechNet.Exception.Attendee;
using InTechNet.Exception.Hub;
using InTechNet.Exception.Module;
using InTechNet.Services.Module.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using InTechNet.Common.Dto.Resource;
using InTechNet.Exception.Resource;

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

        /// <inheritdoc cref="IModuleService.FinishModule"/>
        public void FinishModule(int idPupil, int idHub, int idModule)
        {
            // Retrieve the attendee associated to the pupil in this hub
            if (!TryGetAttendingPupil(idPupil, idHub, out var attendee))
            {
                throw new UnknownAttendeeException();
            }

            // Check if the module is in progress for the pupil
            var isModuleInProgress = _context.CurrentModules.Include(_ => _.Attendee)
                .Include(_ => _.Module)
                .Any(_ => _.Attendee.Id == attendee.Id && _.Module.Id == idModule);

            if (!isModuleInProgress)
            {
                throw new IllegalModuleOperationException();
            }

            // Clear the associate current state
            var currentState = _context.States.Include(_ => _.Attendee)
                    .ThenInclude(_ => _.Hub)
                    .SingleOrDefault(_ =>
                        _.Attendee.Id == attendee.Id
                        && _.Attendee.Hub.Id == idHub)
                ?? throw new UnknownStateException();

            _context.States.Remove(currentState);

            // Clear the current module
            var currentModule = _context.CurrentModules
                    .SingleOrDefault(_ =>
                        _.Attendee.Id == attendee.Id
                        && _.Module.Id == idModule)
                ?? throw new UnknownModuleException();

            _context.CurrentModules.Remove(currentModule);

            // Commit changes
            _context.SaveChanges();
        }

        /// <inheritdoc cref="IModuleService.GetCurrentResource"/>
        public ResourceDto GetCurrentResource(int idPupil, int idHub, int idModule)
        {
            // Retrieve the attendee associated to the pupil in this hub
            if (!TryGetAttendingPupil(idPupil, idHub, out var attendee))
            {
                throw new UnknownAttendeeException();
            }

            // Retrieve the state of the user
            var state = _context.States.Include(_ => _.Attendee)
                    .Include(_ => _.Resource)
                     .SingleOrDefault(_ =>
                         _.Attendee.Id == attendee.Id)
                 ?? throw new UnknownStateException();

            // Retrieve the resource associated with this sate
            var resource = _context.Resources
                    .SingleOrDefault(_ => _.Id == state.Resource.Id)
                ?? throw new UnknownResourceException();

            return new ResourceDto
            {
                Content = resource.Content,
                Id = resource.Id
            };
        }

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

        /// <summary>
        /// Attempt to retrieve the pupil attending the hub
        /// </summary>
        /// <param name="idPupil">Id of the pupil</param>
        /// <param name="idHub">Id of the hub to be looked at</param>
        /// <param name="attendee">The retrieved attendee</param>
        /// <returns>True the attending pupil is successfully fetched; false otherwise</returns>
        private bool TryGetAttendingPupil(int idPupil, int idHub, out DataAccessLayer.Entities.Hubs.Attendee attendee)
        {
            attendee = _context.Attendees
                .Include(_ => _.Pupil)
                .Include(_ => _.Hub)
                .FirstOrDefault(_ =>
                    _.Pupil.Id == idPupil
                    && _.Hub.Id == idHub);

            return attendee != null;
        }

        /// <inheritdoc cref="IModuleService.StartModule"/>
        public void StartModule(int idPupil, int idHub, int idModule)
        {
            // Retrieve the attendee associated to the pupil in this hub
            if (!TryGetAttendingPupil(idPupil, idHub, out var attendee))
            {
                throw new UnknownAttendeeException();
            }

            // Assert that this module is reachable for this attendee
            var isModuleReachable = _context.AvailableModules.Include(_ => _.Hub)
                .Any(_ => 
                    _.Hub.Id == attendee.Hub.Id);

            if (!isModuleReachable)
            {
                throw new UnknownModuleException();
            }

            // Retrieve the module to be associated with the current module
            var module = _context.Modules.FirstOrDefault(_ =>
                    _.Id == idModule)
                ?? throw new UnknownModuleException();

            // Retrieve all ids that connect each resource to the following one
            var nextResources = _context.Resources
                .Include(_ => _.Module)
                .Include(_ => _.NextResource)
                .Where(_ => _.Module.Id == module.Id)
                .Select(_ => _.NextResource.Id);

            // Retrieve the first resource of this module
            var startingResource = _context.Resources.Include(_ => _.Module)
                .FirstOrDefault(_ => 
                    _.Module.Id == module.Id
                    // The first resource of the module is the one that is not used as next resource for any existing resource
                    && !nextResources.Contains(_.Id));

            // Create the initial state of the user's module completion
            _context.States.Add(new State
            {
                Attendee = attendee,
                Resource = startingResource,
            });

            // Set this module as the current one
            _context.CurrentModules.Add(new CurrentModule
            {
                Attendee = attendee,
                Module = module,
            });

            // Commit changes
            _context.SaveChanges();
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

        /// <inheritdoc cref="IModuleService.ValidateCurrentResource"/>
        public void ValidateCurrentResource(int idPupil, int idHub, int idModule)
        {
            // Retrieve the attendee associated to the pupil in this hub
            if (!TryGetAttendingPupil(idPupil, idHub, out var attendee))
            {
                throw new UnknownAttendeeException();
            }

            // Get the current state of the pupil
            var currentStep = _context.States
                    .Include(_ => _.Attendee)
                    .Include(_ => _.Resource)
                    .SingleOrDefault(_ => _.Attendee.Id == attendee.Id)
                ?? throw new UnknownStateException();

            // Get the associated resource
            var resource = _context.Resources
                    .Include(_ => _.NextResource)
                    .FirstOrDefault(_ => _.Id == currentStep.Resource.Id)
                ?? throw new UnknownResourceException();

            // Set the current resource to the next one
            currentStep.Resource = resource.NextResource;

            // Commit changes
            _context.SaveChanges();
        }
    }
}
