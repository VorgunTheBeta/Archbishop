using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Discord;
using Archbishop2.Classes;

namespace Archbishop2.Classes.JSONModels
{
    public class Configuration
    {

        public CommandPrefixesModel CommandPrefixes { get; set; } = new CommandPrefixesModel();

    }
    public class CommandPrefixesModel
    {
        public string Custom { get; set; } = "*";
        public string Help { get; set; } = "-";
        public string Search { get; set; } = "/";
        public string Osu { get; set; } = "$";
    }
    public static class ConfigHandler
    {
        private static readonly object configLock = new object();
        public static void SaveConfig()
        {
            lock (configLock)
            {
                File.WriteAllText("data/config.json", JsonConvert.SerializeObject(Program.config, Formatting.Indented));
            }
        }
    }
}
