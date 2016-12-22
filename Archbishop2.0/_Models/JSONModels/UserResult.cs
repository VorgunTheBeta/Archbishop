using Discord;
namespace Archbishop2.Classes.JSONModels
{
    public class UserResult
    {
        public int id;
        public string display_name;
        public int anime_time;
        public int manga_chap;
        public string image_url_lge;
        public string about;

        public object ToEmbed()
        {
            var embed = new EmbedBuilder()
                    .WithTitle(display_name)
                    .WithDescription(about)
                    .WithImageUrl(image_url_lge)
                    .WithColor(new Color(255, 26, 198))
                    .AddField(x => x.WithName("Time spent watching anine").WithValue(CalculateTime(anime_time)).WithIsInline(true))
                    .AddField(x => x.WithName("Number of manga chapters read").WithValue(manga_chap.ToString()).WithIsInline(true))
                    .AddField(x => x.WithName("Link").WithValue($"http://anilist.co/manga/{id}").WithIsInline(true))
            ;
            return embed;
        }
        public static string CalculateTime(int minutes)
        {
            if (minutes == 0)
                return "No time.";

            int years, months, days, hours = 0;

            hours = minutes / 60;
            minutes %= 60;
            days = hours / 24;
            hours %= 24;
            months = days / 30;
            days %= 30;
            years = months / 12;
            months %= 12;

            string animeWatched = "";

            if (years > 0)
            {
                animeWatched += years;
                if (years == 1)
                    animeWatched += " **year**";
                else
                    animeWatched += " **years**";
            }

            if (months > 0)
            {
                if (animeWatched.Length > 0)
                    animeWatched += ", ";
                animeWatched += months;
                if (months == 1)
                    animeWatched += " **month**";
                else
                    animeWatched += " **months**";
            }

            if (days > 0)
            {
                if (animeWatched.Length > 0)
                    animeWatched += ", ";
                animeWatched += days;
                if (days == 1)
                    animeWatched += " **day**";
                else
                    animeWatched += " **days**";
            }

            if (hours > 0)
            {
                if (animeWatched.Length > 0)
                    animeWatched += ", ";
                animeWatched += hours;
                if (hours == 1)
                    animeWatched += " **hour**";
                else
                    animeWatched += " **hours**";
            }

            if (minutes > 0)
            {
                if (animeWatched.Length > 0)
                    animeWatched += " and ";
                animeWatched += minutes;
                if (minutes == 1)
                    animeWatched += " **minute**";
                else
                    animeWatched += " **minutes**";
            }

            return animeWatched;
        }
    }
}
