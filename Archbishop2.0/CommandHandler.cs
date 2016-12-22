using System.Threading.Tasks;
using System;
using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Archbishop2.Classes.JSONModels;

namespace Archbishop2
{
    public class CommandHandler
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private IDependencyMap map;

        public async Task Install(IDependencyMap _map)
        {
            //Create Command Service, inject into Dependency Map
            client = _map.Get<DiscordSocketClient>();
            commands = new CommandService();
            _map.Add(commands);
            map = _map;

            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
            client.MessageReceived += HandleCommand;
        }

        public async Task HandleCommand(SocketMessage parameterMessage)
        {
            //Don't handle the command if it is a system message
            var message = parameterMessage as SocketUserMessage;
            if (message != null)
            {
                var context = new CommandContext(client, message);
                
                int argPos = 0;
                if (message.HasStringPrefix(Configuration.Load().Prefix, ref argPos))
                {
                    Console.WriteLine(context.Message);
                    var result = await commands.ExecuteAsync(context, argPos);

                    if (!result.IsSuccess)
                        await context.Channel.SendMessageAsync(result.ToString());
                }
            }
        }

    }
        
}
