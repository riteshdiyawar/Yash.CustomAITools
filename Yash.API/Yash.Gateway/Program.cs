using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Yash.API.Gateway
{
    public class Program
    {
        //code review comments
        protected Program()
        {

        }
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
            {
                // JSON files, User secrets, environment variables and command line arguments
                ///config.AddJsonFile("configuration.json");
               config.AddJsonFile($"configuration.{hostingContext.HostingEnvironment.EnvironmentName}.json")
                .AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
