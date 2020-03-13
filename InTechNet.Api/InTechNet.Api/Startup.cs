using InTechNet.Api.Helpers;
using InTechNet.Common.Utils.Configuration.Helper;
using InTechNet.DataAccessLayer;
using InTechNet.DataAccessLayer.Entity.EntityFrameworkStoresFix;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace InTechNet.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// InTechNet metadata
        /// </summary>
        private ProjectDto _metadata;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _metadata = new ProjectDto();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();

            // Fill the DI container
            DependencyInjectionHelper.InitializeContainer(Configuration, services);

            // Bind API's meta data to the Swagger UI
            ConfigureSwagger(services);

            // Configure InTechNet IdentityServer
            ConfigureIdentityServer(services);
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

            //Start Identity Setup
            //DatabaseInitializer.Initialize(app, context);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(option => option
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();

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
        }

        /// <summary>
        /// Configure InTechNet IdentityServer parameters
        /// </summary>
        private void ConfigureIdentityServer(IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryPersistedGrants()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryClients(Config.GetClients())
                    .AddAspNetIdentity<User>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // TODO: create helper (in InTechNet.Common.Utils.Configuration) for config fields
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JwtToken:Issuer"],
                    ValidAudience = Configuration["JwtToken:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["JwtToken:SecretKey"]))
                };
            });
        }

        /// <summary>
        /// Loads the API's meta data from the appsettings file
        /// and bind it to the Swagger UI
        /// </summary>
        private void ConfigureSwagger(IServiceCollection services)
        {
            // Load project's definition
            Configuration.GetSection("Project").Bind(_metadata);
            Configuration.GetSection("Project:Contact").Bind(_metadata.Contact);
            Configuration.GetSection("Project:License").Bind(_metadata.License);

            // Populate Swagger UI
            services.AddSwaggerGen(_ =>
            {
                // Swagger UI overall documentation
                _.SwaggerDoc(_metadata.Version, new OpenApiInfo
                {
                    Version = _metadata.Version,
                    Title = _metadata.Title,
                    Description = _metadata.Description,
                    Contact = new OpenApiContact
                    {
                        Name = _metadata.Contact.Name,
                        Email = _metadata.Contact.Email,
                        Url = new Uri(_metadata.Contact.Url)
                    },
                    License = new OpenApiLicense
                    {
                        Name = _metadata.License.Name,
                        Url = new Uri(_metadata.License.Url)
                    }
                });

                // Security definition
                _.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                // Swagger UI security feature

                // Endpoint documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                _.IncludeXmlComments(xmlPath);
            });
        }
    }
}
