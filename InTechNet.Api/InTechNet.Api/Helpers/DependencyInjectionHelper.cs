using InTechNet.Common.Utils.Authentication.Jwt;
using InTechNet.DataAccessLayer;
using InTechNet.Service.Authentication;
using InTechNet.Service.Authentication.Interfaces;
using InTechNet.Service.Authentication.Jwt;
using InTechNet.Service.User;
using InTechNet.Service.User.Interfaces;
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

            _services.AddTransient<IUserService, UserService>();
        }

        /// <summary>
        /// Register the used DbContexts
        /// </summary>
        private static void RegisterDatabaseContexts()
        {
            // InTechNet database registration
            _services.AddDbContext<InTechNetContext>(options =>
                options.UseNpgsql(_configuration.GetConnectionString("InTechNetDatabase")));

            // IdentityServer database registration
            _services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(_configuration.GetConnectionString("InTechNetAuthenticationDatabase")));
        }
    }
}
