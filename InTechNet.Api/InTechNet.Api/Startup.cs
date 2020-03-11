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
using InTechNet.Service.Authentication.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using InTechNet.DataAccessLayer.Entity.EntityFrameworkStoresFix;

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
            services.AddCors();

            // Bind API's meta data to the Swagger UI
            ConfigureSwagger(services);

            services.AddDbContext<InTechNetContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("InTechNetDatabase")));           

            // Start Identity Setup
            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("InTechNetAuthenticationDatabase")));

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
                    ValidAudience = Configuration["JwtToken:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["JwtToken:SecretKey"]))
                };
            });

            /*
            .AddConfigurationStore(option =>
                   option.ConfigureDbContext = builder => builder.UseNpgsql(Configuration.GetConnectionString("InTechNetAuthenticationDatabase"), options =>
                   options.MigrationsAssembly("InTechNet.DataAccessLayer")))
            .AddOperationalStore(option =>
                   option.ConfigureDbContext = builder => builder.UseNpgsql(Configuration.GetConnectionString("InTechNetAuthenticationDatabase"), options =>
                   options.MigrationsAssembly("InTechNet.DataAccessLayer")));*/

            /*services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(
                    options =>
                        {
                            // auth server base endpoint (will use to search for disco doc)
                            options.Authority = "http://localhost:5000";
                            options.ApiName = "apiModerator"; // required audience of access tokens
                            options.RequireHttpsMetadata = false; // dev only!
                        });*/
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

            // Populate swagger UI
            services.AddSwaggerGen(_ =>
            {
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

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                _.IncludeXmlComments(xmlPath);
            });
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
    }
}
