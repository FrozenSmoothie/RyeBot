using System.Threading.Tasks;
using RyeBot.Builder;
using RyeBot.Services;
using Discord;
using Discord.Interactions;
using NodaTime;
using NodaTime.Extensions;

namespace RyeBot.Modules
{
    public class DebugModule : InteractionModuleBase<ShardedInteractionContext>
    {
        private readonly EmbedTemplateBuilder _embedTemplateBuilder;

        public DebugModule(
            EmbedTemplateBuilder embedTemplateBuilder)
        {
            _embedTemplateBuilder = embedTemplateBuilder;
        }

        [SlashCommand("ping", "Get RyeBots's average response time")]
        public async Task DisplayBotAverageResponseTimeAsync()
        {
            var difference = Period.Between(
                Context.Interaction.CreatedAt.DateTime.ToLocalTime().ToLocalDateTime(),
                DateTimeService.GetCurrentLocalDateTime(),
                PeriodUnits.Milliseconds);

            var embedBuilder = _embedTemplateBuilder.BuildEmbed(
                    description: $"Pong! `{difference.Milliseconds}ms`");

            await Context.Interaction.RespondAsync("", new Embed[] { embedBuilder.Build() }, false);
        }
    }
}
