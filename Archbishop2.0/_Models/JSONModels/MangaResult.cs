using Discord;
namespace Archbishop2.Classes.JSONModels
{
    public class MangaResult
    {
        public int id;
        public string publishing_status;
        public string image_url_lge;
        public string title_english;
        public int total_chapters;
        public int total_volumes;
        public string description;

        public object ToEmbed()
        {
            var embed = new EmbedBuilder()
                    .WithTitle(title_english)
                    .WithDescription(publishing_status)
                    .WithImageUrl(image_url_lge)
                    .WithColor(new Color(255, 26, 198))
                    .AddField(x => x.WithName("Chapters").WithValue($"{total_chapters} total chapters").WithIsInline(true))
                    .AddField(x => x.WithName("Volumes").WithValue($"{total_volumes} total volumes").WithIsInline(true))
                    .AddField(x => x.WithName("Synopsis").WithValue($"{description}").WithIsInline(false))
                    .AddField(x => x.WithName("Link").WithValue($"http://anilist.co/manga/{id}").WithIsInline(true))
            ;
            return embed;
        }
    }
}
