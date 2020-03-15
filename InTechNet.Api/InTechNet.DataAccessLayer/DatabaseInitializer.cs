using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InTechNet.DataAccessLayer
{
    /// <summary>
    /// Code-first database initialization object
    /// </summary>
    public class DatabaseInitializer
    {
        /// <summary>
        /// Code-first database initialization
        /// </summary>
        public static void Initialize(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            scope.ServiceProvider.GetRequiredService<InTechNetContext>().Database.Migrate();
        }
    }
}
