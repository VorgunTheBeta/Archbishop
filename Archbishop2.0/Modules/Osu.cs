using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Archbishop2.Modules
{
    [Name("Osu")]
    public class Osu : ModuleBase
    {
        [Command("osuSearch")]
        [Remarks("Shows osu stats for a player.\n**Usage**: `$osu Name` or `$osu Name taiko`")]
        public async Task OsuSearch(IUserMessage umsg, string usr, [Remainder] string mode = null)
        {
            var channel = (ITextChannel)umsg.Channel;

            if (string.IsNullOrWhiteSpace(usr))
            {
                await channel.TriggerTypingAsync();
                await channel.SendMessageAsync("Please specify a user");
            }

            using (HttpClient http = new HttpClient())
            {
                try
                {
                    var m = 0;
                    if (!string.IsNullOrWhiteSpace(mode))
                    {
                        m = ResolveGameMode(mode);
                    }
                    http.DefaultRequestHeaders.Clear();
                    http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.202 Safari/535.1");
                    http.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    var res = await http.GetStreamAsync(new Uri($"http://lemmmy.pw/osusig/sig.php?uname={ usr }&flagshadow&xpbar&xpbarhex&pp=2&mode={m}")).ConfigureAwait(false);

                    MemoryStream ms = new MemoryStream();
                    res.CopyTo(ms);
                    ms.Position = 0;
                    await channel.SendFileAsync(ms, $"{usr}.png", $"`Profile Link:`https://osu.ppy.sh/u/{Uri.EscapeDataString(usr)}\n`Image provided by https://lemmmy.pw/osusig`").ConfigureAwait(false);
                
                }
                catch(Exception ex)
                {
                    await channel.SendMessageAsync("💢 Failed retrieving osu signature :\\").ConfigureAwait(false);
                    Console.WriteLine(ex);
                }
            }

        }
        //https://osu.ppy.sh/wiki/Accuracy
        private static Double CalculateAcc(JToken play, int mode)
        {
            if (mode == 0)
            {
                var hitPoints = Double.Parse($"{play["count50"]}") * 50 + Double.Parse($"{play["count100"]}") * 100 + Double.Parse($"{play["count300"]}") * 300;
                var totalHits = Double.Parse($"{play["count50"]}") + Double.Parse($"{play["count100"]}") + Double.Parse($"{play["count300"]}") + Double.Parse($"{play["countmiss"]}");
                totalHits *= 300;
                return Math.Round(hitPoints / totalHits * 100, 2);
            }
            else if (mode == 1)
            {
                var hitPoints = Double.Parse($"{play["countmiss"]}") * 0 + Double.Parse($"{play["count100"]}") * 0.5 + Double.Parse($"{play["count300"]}") * 1;
                var totalHits = Double.Parse($"{play["countmiss"]}") + Double.Parse($"{play["count100"]}") + Double.Parse($"{play["count300"]}");
                hitPoints *= 300;
                totalHits *= 300;
                return Math.Round(hitPoints / totalHits * 100, 2);
            }
            else if (mode == 2)
            {
                var fruitsCaught = Double.Parse($"{play["count50"]}") + Double.Parse($"{play["count100"]}") + Double.Parse($"{play["count300"]}");
                var totalFruits = Double.Parse($"{play["countmiss"]}") + Double.Parse($"{play["count50"]}") + Double.Parse($"{play["count100"]}") + Double.Parse($"{play["count300"]}") + Double.Parse($"{play["countkatu"]}");
                return Math.Round(fruitsCaught / totalFruits * 100, 2);
            }
            else
            {
                var hitPoints = Double.Parse($"{play["count50"]}") * 50 + Double.Parse($"{play["count100"]}") * 100 + Double.Parse($"{play["countkatu"]}") * 200 + (Double.Parse($"{play["count300"]}") + Double.Parse($"{play["countgeki"]}")) * 300;
                var totalHits = Double.Parse($"{play["countmiss"]}") + Double.Parse($"{play["count50"]}") + Double.Parse($"{play["count100"]}") + Double.Parse($"{play["countkatu"]}") + Double.Parse($"{play["count300"]}") + Double.Parse($"{play["countgeki"]}");
                totalHits *= 300;
                return Math.Round(hitPoints / totalHits * 100, 2);
            }
        }

        private static string ResolveMap(string mapLink)
        {
            Match s = new Regex(@"osu.ppy.sh\/s\/", RegexOptions.IgnoreCase).Match(mapLink);
            Match b = new Regex(@"osu.ppy.sh\/b\/", RegexOptions.IgnoreCase).Match(mapLink);
            Match p = new Regex(@"osu.ppy.sh\/p\/", RegexOptions.IgnoreCase).Match(mapLink);
            Match m = new Regex(@"&m=", RegexOptions.IgnoreCase).Match(mapLink);
            if (s.Success)
            {
                var mapId = mapLink.Substring(mapLink.IndexOf("/s/") + 3);
                return $"s={mapId}";
            }
            else if (b.Success)
            {
                if (m.Success)
                    return $"b={mapLink.Substring(mapLink.IndexOf("/b/") + 3, mapLink.IndexOf("&m") - (mapLink.IndexOf("/b/") + 3))}";
                else
                    return $"b={mapLink.Substring(mapLink.IndexOf("/b/") + 3)}";
            }
            else if (p.Success)
            {
                if (m.Success)
                    return $"b={mapLink.Substring(mapLink.IndexOf("?b=") + 3, mapLink.IndexOf("&m") - (mapLink.IndexOf("?b=") + 3))}";
                else
                    return $"b={mapLink.Substring(mapLink.IndexOf("?b=") + 3)}";
            }
            else
            {
                return $"s={mapLink}"; //just a default incase an ID number was provided by itself (non-url)?
            }
        }

        private static int ResolveGameMode(string mode)
        {
            switch (mode.ToLower())
            {
                case "std":
                case "standard":
                    return 0;
                case "taiko":
                    return 1;
                case "ctb":
                case "catchthebeat":
                    return 2;
                case "mania":
                case "osu!mania":
                    return 3;
                default:
                    return 0;
            }
        }

        //https://github.com/ppy/osu-api/wiki#mods
        private static string ResolveMods(int mods)
        {
            var modString = $"+";

            if (IsBitSet(mods, 0))
                modString += "NF";
            if (IsBitSet(mods, 1))
                modString += "EZ";
            if (IsBitSet(mods, 8))
                modString += "HT";

            if (IsBitSet(mods, 3))
                modString += "HD";
            if (IsBitSet(mods, 4))
                modString += "HR";
            if (IsBitSet(mods, 6) && !IsBitSet(mods, 9))
                modString += "DT";
            if (IsBitSet(mods, 9))
                modString += "NC";
            if (IsBitSet(mods, 10))
                modString += "FL";

            if (IsBitSet(mods, 5))
                modString += "SD";
            if (IsBitSet(mods, 14))
                modString += "PF";

            if (IsBitSet(mods, 7))
                modString += "RX";
            if (IsBitSet(mods, 11))
                modString += "AT";
            if (IsBitSet(mods, 12))
                modString += "SO";
            return modString;
        }

        private static bool IsBitSet(int mods, int pos)
        {
            return (mods & (1 << pos)) != 0;
        }

    }

}

