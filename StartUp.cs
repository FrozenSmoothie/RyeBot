using RyeBot.Handlers;
using RyeBot.Handlers.InteractionHandlers;
using RyeBot.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace RyeBot
{
    public class Startup
    {
        public Startup(string[] args)
        {
        }

        public static async Task RunAsync(string[] args)
        {
            var startup = new Startup(args);
            await startup.RunAsync();
        }

        public async Task RunAsync()
        {
            var services = new ServiceCollection();             // Create a new instance of a service collection
            var dependencyRegistrationHandler = new DependencyRegistrationHandler();

            dependencyRegistrationHandler.RegisterDependencies(services);

            var provider = services.BuildServiceProvider();     // Build the service provider

            provider.GetRequiredService<CommandRegistrationService>();
            provider.GetRequiredService<LoggingService>();      // Start the logging service
            provider.GetRequiredService<SlashAndContextCommandResultHandler>(); 		// Start the interaction handler service

            await provider.GetRequiredService<StartupService>().StartAsync();       // Start the startup service
            await Task.Delay(-1);                               // Keep the program alive
        }
    }
}
