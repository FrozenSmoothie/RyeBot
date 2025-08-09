using RyeBot.Services;
using Discord.Addons.Interactive;
using Microsoft.Extensions.DependencyInjection;

namespace RyeBot.DependencyInjectionModules
{
    public static class ServicesDependencyModule
    {
        public static void AddRegistrations(IServiceCollection services)
        {
            services
                .AddSingleton<CommandRegistrationService>()
                .AddSingleton<StartupService>()
                .AddSingleton<LoggingService>()
                .AddSingleton<RscriptService>()
                .AddSingleton<PythonScriptService>()
                .AddSingleton<InteractiveService>();
        }
    }
}
