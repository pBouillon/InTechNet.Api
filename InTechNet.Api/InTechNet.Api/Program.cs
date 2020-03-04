using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace InTechNet.Api
{
    /// <summary>
    /// Default .NET Core API entry-point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method to be called on startup
        /// </summary>
        /// <param name="args">Provided arguments from command line</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the <see cref="IHostBuilder"/> from which the <see cref="IWebHost"/> will be built
        /// </summary>
        /// <param name="args">Provided args from <see cref="Program.Main"/></param>
        /// <returns>The configured <see cref="IHostBuilder"/></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(configuration);
                    webBuilder.UseStartup<Startup>();
                });

            return host;
        }
    }
}
