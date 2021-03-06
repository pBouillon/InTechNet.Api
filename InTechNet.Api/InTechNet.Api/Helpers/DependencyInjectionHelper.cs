﻿using InTechNet.Common.Utils.Authentication.Jwt;
using InTechNet.DataAccessLayer;
using InTechNet.DataAccessLayer.Context;
using InTechNet.Services.Attendee;
using InTechNet.Services.Attendee.Interfaces;
using InTechNet.Services.Authentication;
using InTechNet.Services.Authentication.Interfaces;
using InTechNet.Services.Authentication.Jwt;
using InTechNet.Services.Hub;
using InTechNet.Services.Hub.Interfaces;
using InTechNet.Services.Module;
using InTechNet.Services.Module.Interfaces;
using InTechNet.Services.SubscriptionPlan;
using InTechNet.Services.SubscriptionPlan.Interfaces;
using InTechNet.Services.User;
using InTechNet.Services.User.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InTechNet.Api.Helpers
{
    /// <summary>
    /// Provides a helper for dependency injection operations
    /// </summary>
    public class DependencyInjectionHelper
    {
        private static IConfiguration _configuration;

        private static IServiceCollection _services;

        /// <summary>
        /// Initialize the DI container with all classes
        /// </summary>
        public static void InitializeContainer(IConfiguration configuration, IServiceCollection services)
        {
            _configuration = configuration;
            _services = services;

            RegisterModels();

            RegisterDatabaseContexts();

            RegisterServices();
        }

        /// <summary>
        /// Register InTechNet custom services
        /// </summary>
        private static void RegisterModels()
        {
            // HTTP context accessor
            _services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            // JWT resource generation and registration
            var jwtResourcesDto = new JwtResourceHelper();

            _configuration
                .GetSection(JwtResourceHelper.AppSettingsSectionName)
                .Bind(jwtResourcesDto);

            _services.AddSingleton(jwtResourcesDto);
        }

        /// <summary>
        /// Register InTechNet custom services
        /// </summary>
        private static void RegisterServices()
        {
            // Authentication services
            _services.AddTransient<IAuthenticationService, AuthenticationService>();

            _services.AddTransient<IJwtService, JwtService>();

            // User handling services
            _services.AddTransient<IModeratorService, ModeratorService>();

            _services.AddTransient<IPupilService, PupilService>();

            // Hub service
            _services.AddTransient<IHubService, HubService>();

            // Attendee service
            _services.AddTransient<IAttendeeService, AttendeeService>();

            // Subscription plan
            _services.AddTransient<ISubscriptionPlanService, SubscriptionPlanService>();

            // Modules
            _services.AddTransient<IModuleService, ModuleService>();
        }

        /// <summary>
        /// Register the used DbContexts
        /// </summary>
        private static void RegisterDatabaseContexts()
        {
            // InTechNet database registration
            _services.AddDbContext<InTechNetContext>(options =>
                options.UseNpgsql(_configuration.GetConnectionString("InTechNetDatabase")));

            // Register its interface
            _services.AddScoped<IInTechNetContext, InTechNetContext>(_ 
                => _.GetService<InTechNetContext>());
        }
    }
}
