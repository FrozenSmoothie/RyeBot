using System;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using RyeBot.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RyeBot.DependencyInjectionModules;

namespace RyeBot.Handlers
{
    public class DependencyRegistrationHandler
    {
        private IConfigurationRoot _configuration { get; }

        public DependencyRegistrationHandler()
        {
            var builder = new ConfigurationBuilder()        // Create a new instance of the config builder
                .SetBasePath(AppContext.BaseDirectory)      // Specify the default location for the config file
                .AddYamlFile("config.yml");                // Add this (yaml encoded) file to the configuration
            _configuration = builder.Build();                // Build the configuration
        }

        public void RegisterDependencies(IServiceCollection services)
        {
            services.AddSingleton(new DiscordShardedClient(new DiscordSocketConfig
            {
                // Add discord to the collection
                LogLevel = LogSeverity.Verbose, // Tell the logger to give Verbose amount of info
                MessageCacheSize = 1000, // Cache 1,000 messages per channel
                AlwaysDownloadUsers = true,
                TotalShards = 1,
                GatewayIntents = GatewayIntents.Guilds |
                                 GatewayIntents.GuildMembers |
                                 GatewayIntents.GuildBans |
                                 GatewayIntents.GuildEmojis |
                                 GatewayIntents.GuildVoiceStates |
                                 GatewayIntents.GuildMessages |
                                 GatewayIntents.GuildMessageReactions |
                                 GatewayIntents.DirectMessages |
                                 GatewayIntents.DirectMessageReactions |
                                 GatewayIntents.MessageContent
            }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    // Add the command service to the collection
                    LogLevel = LogSeverity.Verbose // Tell the logger to give Verbose amount of info
                }))
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordShardedClient>()))
                .AddSingleton(_configuration) // Add the configuration to the collection
                .AddSingleton<EmbedTemplateBuilder>();

            ServicesDependencyModule.AddRegistrations(services);

            HandlersDependencyModule.AddRegistrations(services);
        }
    }
}
