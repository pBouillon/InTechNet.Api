using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace InTechNet.DataAccessLayer
{
    public class DatabaseInitializer
    {
        public static void Initialize(IApplicationBuilder app, AuthDbContext context)
        {
            context.Database.EnsureCreated();

            context.SaveChanges();

            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            scope.ServiceProvider.GetRequiredService<InTechNetContext>().Database.Migrate();
        }
    }
}
