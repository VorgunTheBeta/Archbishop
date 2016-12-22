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
using Archbishop2.Classes;

namespace Archbishop2.Modules
{
    [Name("Search")]
    public class Search : ModuleBase
    {
        [Command("anilist")]
        [Alias("al")]
        [Remarks("Gets the first user from anilist 'NOT ACCURATE AT TIMES'")]
        public async Task anilistSearch([Remainder] string user = null)
        {
            if(string.IsNullOrWhiteSpace(user))
            {
                return;
            }
            try
            {
                if(user.StartsWith("<@"))
                {
                    string usr = Context.Message.Channel.GetUserAsync(Convert.ToUInt64(user.Substring(2, 18))).Result.Username;
                    EmbedBuilder embed = (await SearchHelper.GetUserData(usr).ConfigureAwait(false)).ToEmbed();
                    await Context.Channel.SendMessageAsync("", embed: embed);
                }
            }
            catch
            {

            }

        }
    }
}
