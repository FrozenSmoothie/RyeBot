using Discord;

namespace RyeBot.Builder
{
    public class EmbedTemplateBuilder
    {
        public EmbedBuilder BuildEmbed(
            string authorIconUrl = null,
            string authorName = null,
            string authorUrl = null,
            string title = null,
            string titleUrl = null,
            string thumbnailUrl = null,
            string imageUrl = null,
            string description = null,
            string fullFooterText = null,
            string footerTextAddOn = null,
            Color? color = null)
        {
            var footerText = fullFooterText ?? $"RyeBot - {footerTextAddOn}";

            var builder = new EmbedBuilder
            {
                Title = title,
                Url = titleUrl,
                ThumbnailUrl = thumbnailUrl,
                Color = color ?? new Color(80, 227, 194),
                ImageUrl = imageUrl,
                Description = description,
                Author = new EmbedAuthorBuilder
                {
                    IconUrl = authorIconUrl,
                    Name = authorName,
                    Url = authorUrl
                },
                Footer = new EmbedFooterBuilder
                {
                    Text = footerText
                }
            };

            return builder;
        }
    }
}
