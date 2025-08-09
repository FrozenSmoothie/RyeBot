using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using System;
using System.Threading.Tasks;

namespace RyeBot.Handlers.InteractionHandlers
{
    public class SlashAndContextCommandResultHandler
    {
        private readonly DiscordShardedClient _discord;
        private readonly InteractionService _interactionService;
        private readonly IServiceProvider _provider;

        public SlashAndContextCommandResultHandler(
            InteractionService interactionService,
            DiscordShardedClient discord,
            IServiceProvider provider)
        {
            _discord = discord;
            _interactionService = interactionService;
            _provider = provider;

            discord.InteractionCreated += OnInteractionCreatedAsync;
            interactionService.ContextCommandExecuted += OnContextCommandExecutedAsync;
            interactionService.SlashCommandExecuted += OnSlashCommandExecutedAsync;
        }

        private Task OnInteractionCreatedAsync(
            SocketInteraction interaction)
        {
            Task.Run(async () => {
                try
                {
                    // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules
                    var context = new ShardedInteractionContext(_discord, interaction);

                    await _interactionService.ExecuteCommandAsync(context, _provider);

                }
                catch (Exception)
                {
                    // If a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist.
                    // It is a good idea to delete the original response, or at least let the user know that something went wrong
                    // during the command execution.
                    if (interaction.Type == InteractionType.ApplicationCommand)
                        await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
                }
            });

            return Task.CompletedTask;
        }

        private Task OnContextCommandExecutedAsync(
            ContextCommandInfo contextCommandInfo,
            IInteractionContext interactionContext,
            IResult interactionResult)
        {
            if (!interactionResult.IsSuccess)
            {
                switch (interactionResult.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;

                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;

                    case InteractionCommandError.BadArgs:
                        // implement
                        break;

                    case InteractionCommandError.Exception:
                        // implement
                        break;

                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;

                    default:
                        break;
                }
            }
            else
            {
            }

            return Task.CompletedTask;
        }

        private Task OnSlashCommandExecutedAsync(
            SlashCommandInfo slashCommandInfo,
            IInteractionContext interactionContext,
            IResult interactionResult)
        {
            if (!interactionResult.IsSuccess)
            {
                switch (interactionResult.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;

                    case InteractionCommandError.UnknownCommand:
                        // implement
                        break;

                    case InteractionCommandError.BadArgs:
                        // implement
                        break;

                    case InteractionCommandError.Exception:
                        // implement
                        break;

                    case InteractionCommandError.Unsuccessful:
                        // implement
                        break;

                    default:
                        break;
                }
            }
            else
            {
            }

            return Task.CompletedTask;
        }
    }
}
