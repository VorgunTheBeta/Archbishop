using Discord;

namespace Archbishop2._Models.JSONModels
{
    public class AnimeResult
    {
        public int id;
        public string airing_status;
        public string title_english;
        public int total_episodes;
        public string description;
        public string image_url_lge;

        public object ToEmbed()
        {
            var embed = new EmbedBuilder()
                    .WithTitle(title_english)
                    .WithDescription(airing_status)
                    .WithImageUrl(image_url_lge)
                    .WithColor(new Color(255, 26, 198))
                    .AddField(x => x.WithName("Episodes").WithValue($"{total_episodes} total episodes").WithIsInline(true))
                    .AddField(x => x.WithName("Link").WithValue($"http://anilist.co/anime/{id}").WithIsInline(true))
            ;
            return embed;
        }
    }
}
