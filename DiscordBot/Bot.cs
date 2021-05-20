using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            var config = new DiscordConfiguration
            {
                Token = Environment.GetEnvironmentVariable("BOT_TOKEN"),
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { Environment.GetEnvironmentVariable("PREFIX") },
                // Enables users to "@" bot instead of using the assigned prefix
                EnableMentionPrefix = true,
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            await Client.ConnectAsync();

            // Lock method in infinite loop to prevent main from returning and closing
            await Task.Delay(-1);
        }

        private async Task<Task> OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            var guildNames = await GetGuildNames(client, client.Guilds.Keys);
            Console.WriteLine(
                $"Bot \"{client.CurrentApplication.Name}\" ready. Currently serving {String.Join(", ", guildNames)}");
            return Task.CompletedTask;
        }

        private async Task<List<string>> GetGuildNames(DiscordClient client, IEnumerable<ulong> ids)
        {
            var guildNames = new List<string>();
            foreach (var id in ids)
            {
                var guild = await client.GetGuildAsync(id, true);
                var name = guild.Name;
                guildNames.Add(name);
            }
            return guildNames;
        }

    }
}
