using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Interactions;

namespace RyeBot.Services
{
    public class StartupService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordShardedClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
        private readonly InteractionService _interactionService;

        public StartupService(
            IServiceProvider provider,
            DiscordShardedClient discord,
            CommandService commands,
            IConfigurationRoot config,
            InteractionService interactionService)
        {
            _provider = provider;
            _config = config;
            _discord = discord;
            _commands = commands;
            _interactionService = interactionService;
        }

        public async Task StartAsync()
        {
            var discordToken = _config["discord:token"];     // Get the discord token from the config file
            if (string.IsNullOrWhiteSpace(discordToken))
            {
                throw new Exception(
                    "Please enter your bot's token into the `_configuration.json` file found in the applications root directory.");
            }

            await _discord.LoginAsync(TokenType.Bot, discordToken);     // Login to discord
            await _discord.StartAsync();                                // Connect to the websocket

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);     // Load commands and modules into the command service
            await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _provider); // Add the public modules that inherit InteractionModuleBase<T> to the InteractionService

            // Set up the bot's displayed activity
            await _discord.SetActivityAsync(
                new Game("Rye's World",
                    ActivityType.Playing));
        }
    }
}
