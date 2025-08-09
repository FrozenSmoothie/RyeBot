using RyeBot.Handlers;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RyeBot.Services
{
    public class CommandRegistrationService
    {
        private static bool isCommandRegistrationCompleted = false;
        private readonly DiscordShardedClient _client;
        private readonly IConfigurationRoot _config;
        private readonly InteractionService _interactionService;

        public CommandRegistrationService(
            DiscordShardedClient client,
            IConfigurationRoot config,
            InteractionService interactionService)
        {
            _client = client;
            _config = config;
            _interactionService = interactionService;

            _client.ShardReady += OnShardReadyAsync;
        }

        private Task OnShardReadyAsync(DiscordSocketClient _)
        {
            Task.Run(async () => {
                bool isCommandRegistrationEnabled;

                var isParsedSuccessfully =
                    bool.TryParse(_config["discord:registerCommands"],
                    out isCommandRegistrationEnabled);

                if (!isParsedSuccessfully)
                {
                    throw new Exception("Please ensure that the 'discord:registerCommands' property is configured with either a 'true' " +
                        "or 'false' value.");
                }

                // This ensures that we only run command registration once, no matter how many shards we may have.
                if (isCommandRegistrationEnabled)
                {
                    if (!isCommandRegistrationCompleted)
                    {
                        await RegisterAllSlashMessageAndUserCommands();

                        isCommandRegistrationCompleted = true;
                    }
                }
            });

            return Task.CompletedTask;
        }

        public async Task RegisterAllSlashMessageAndUserCommands()
        {
            if (DebugIdentificationHandler.IsDebug())
            {
                var testServerGuildId = _config["discord:testServerId"];

                if (string.IsNullOrWhiteSpace(testServerGuildId))
                {
                    throw new Exception("Please enter your test server's id into the 'discord:testServerId' property in " +
                        "the `configuration.yml` file found in the application root directory.");
                }

                var testGuild = _client.GetGuild(ulong.Parse(testServerGuildId));

                if (testGuild == null)
                {
                    Console.WriteLine($"{GetType().Name}: Failed to acquire value for 'testGuild'.");

                    return;
                }

                var registeredCommands = await _interactionService.RegisterCommandsToGuildAsync(testGuild.Id, true);

                LogApplicationCommandRegistrationOutcome(
                    "guild",
                    registeredCommands.Count());
            }
            else
            {
                var registeredCommands = await _interactionService.RegisterCommandsGloballyAsync(true);

                LogApplicationCommandRegistrationOutcome(
                    "global",
                    registeredCommands.Count());
            }
        }

        private void LogApplicationCommandRegistrationOutcome(
            string registrationType,
            int updatedCommandCount)
        {
            Console.WriteLine(
                $"{GetType().Name}: Registered a total of {updatedCommandCount} {registrationType} commands.");
        }
    }
}
