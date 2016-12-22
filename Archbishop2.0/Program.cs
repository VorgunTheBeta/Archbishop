using Discord;
using Discord.WebSocket;
using Archbishop2.Classes.JSONModels;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Discord.Commands;

namespace Archbishop2
{

    class Program
    {
        static void Main(string[] args)
                 => new Program().Start().GetAwaiter().GetResult();

        private DiscordSocketClient client;
        private CommandHandler handler;
        public static Configuration config;
        public static Credentials creds;
        public async Task Start()
        {
            EnsureConfigExists();                            // Ensure the configuration file has been created.
                                                             // Create a new instance of DiscordSocketClient.
            client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info                  // Specify console log information level.
            });

            client.Log += (l)                               // Register the console log event.
                => Task.Run(()
                => Console.WriteLine($"[{l.Severity}] {l.Source}: {l.Exception?.ToString() ?? l.Message}"));

            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.ConnectAsync();

            var map = new DependencyMap();
            map.Add(client);

            handler = new CommandHandler();
            await handler.Install(map);
            await Task.Delay(-1);                            // Prevent the console window from closing.
        }

        public static void EnsureConfigExists()
        {
            if (!Directory.Exists(Path.Combine(AppContext.BaseDirectory, "data")))
                Directory.CreateDirectory(Path.Combine(AppContext.BaseDirectory, "data"));
            string configLoc = Path.Combine(AppContext.BaseDirectory, "data/config.json");
            string configExam = Path.Combine(AppContext.BaseDirectory, "data/config_example.json");
            string credsLoc = Path.Combine(AppContext.BaseDirectory, "credentials.json");
            try
            {
                if (!File.Exists(configLoc))
                    File.Copy(configExam, configLoc);
                
            }
            catch
            {
                Console.WriteLine("Failed writing data/config_example.json");
            }
            try
            {
                config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configLoc));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed loading configuration.");
                Console.WriteLine(ex);
                Console.ReadKey();
                return;
            }
            try
            {
                //load credentials from credentials.json
                creds = JsonConvert.DeserializeObject<Credentials>(File.ReadAllText(credsLoc));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load stuff from credentials.json, RTFM\n{ex.Message}");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Configuration Loaded...");
        }
    }
}