using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }

        public Bot(IServiceProvider services)
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

            Client.UseInteractivity(new InteractivityConfiguration
            {

            });

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { Environment.GetEnvironmentVariable("PREFIX") },
                // Enables users to "@" bot instead of using the assigned prefix
                EnableMentionPrefix = true,
                CaseSensitive = false,
                DmHelp = true,
                EnableDms = true,
                EnableDefaultHelp = true,
                Services = services
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            // Add command groups here
            // Automate this when your not in a bad mood
            Commands.RegisterCommands<Stress>();
            Commands.RegisterCommands<Polls>();
            Commands.RegisterCommands<RoleManager>();
            Commands.RegisterCommands<ReportCommands>();
            //Commands.RegisterCommands<ConnectToGitHub>();

            Client.ConnectAsync();

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
