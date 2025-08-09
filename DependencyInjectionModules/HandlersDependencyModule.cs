using RyeBot.Handlers.InteractionHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace RyeBot.DependencyInjectionModules
{
    public static class HandlersDependencyModule
    {
        public static void AddRegistrations(IServiceCollection services)
        {
            services
                .AddSingleton<SlashAndContextCommandResultHandler>();
        }
    }
}
