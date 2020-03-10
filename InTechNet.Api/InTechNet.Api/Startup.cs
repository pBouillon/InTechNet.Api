using InTechNet.Common.Utils.Configuration.Helper;
using InTechNet.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace InTechNet.Api
{
    public class Startup
    {
        /// <summary>
        /// InTechNet metadata
        /// </summary>
        private ProjectDto _metadata;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _metadata = new ProjectDto();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<InTechNetContext>(options
                => options.UseNpgsql(Configuration.GetConnectionString("InTechNetDatabase")));

            // Load project's definition
            Configuration.GetSection("Project").Bind(_metadata);
            Configuration.GetSection("Project:Contact").Bind(_metadata.Contact);
            Configuration.GetSection("Project:License").Bind(_metadata.License);

            // Populate swagger UI
            services.AddSwaggerGen(_ =>
            {
                _.SwaggerDoc(_metadata.Version, new OpenApiInfo
                {
                    Version = _metadata.Version,
                    Title = _metadata.Title,
                    Description = _metadata.Description,
                    Contact = new OpenApiContact()
                    {
                        Name = _metadata.Contact.Name,
                        Email = _metadata.Contact.Email,
                        Url = new Uri(_metadata.Contact.Url)
                    },
                    License = new OpenApiLicense()
                    {
                        Name = _metadata.License.Name,
                        Url = new Uri(_metadata.License.Url)
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                _.IncludeXmlComments(xmlPath);
            });

            //Start Identity Setup
            services.AddDbContext<AuthDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("InTechNetAuthenticationDatabase")));

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddConfigurationStore(option =>
                           option.ConfigureDbContext = builder => builder.UseNpgsql(Configuration.GetConnectionString("InTechNetAuthenticationDatabase"), options =>
                           options.MigrationsAssembly("InTechNet.DataAccessLayer")))
                    .AddOperationalStore(option =>
                           option.ConfigureDbContext = builder => builder.UseNpgsql(Configuration.GetConnectionString("InTechNetAuthenticationDatabase"), options =>
                           options.MigrationsAssembly("InTechNet.DataAccessLayer")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable swagger middleware
            app.UseSwagger();

            app.UseSwaggerUI(_ =>
            {
                _.SwaggerEndpoint(
                    $"/swagger/{_metadata.Version}/swagger.json", 
                    _metadata.Title);
            });


            //Start Identity Setup
            DatabaseInitializer.Initialize(app, context);
            app.UseIdentityServer();
        }
    }
}
