using DiscordBot.Handlers.GitHubIssueCreation.Steps;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.GitHubIssueCreation
{
    public class GitHubIssueCreationHandler
    {
        private readonly DiscordClient _client;
        private readonly DiscordChannel _channel;
        private readonly DiscordUser _user;
        private IGitHubIssueCreationStep _currentStep { get; set; }
        private List<DiscordMessage> messages = new List<DiscordMessage>();

        public GitHubIssueCreationHandler(
            DiscordClient client,
            DiscordChannel channel,
            DiscordUser user,
            IGitHubIssueCreationStep startingStep)
        {
            _client = client;
            _channel = channel;
            _user = user;
            _currentStep = startingStep;
        }

        public async Task<bool> ProcessReportLogger()
        {
            while (_currentStep != null)
            {
                _currentStep.OnMessageAdded += (m) => messages.Add(m);
                var isCancled = await _currentStep.ProcessStep(_client, _channel, _user)
                    .ConfigureAwait(false);
                if (isCancled)
                {
                    await DeleteMessagesAsync().ConfigureAwait(false);
                    var cancelEmbed = new DiscordEmbedBuilder
                    {
                        Title = "The issue has been successfully saved and posted to github",
                        Color = DiscordColor.PhthaloGreen
                    };

                    await _channel.SendMessageAsync(embed: cancelEmbed).ConfigureAwait(false);

                    return false;
                }

                _currentStep = _currentStep.NextStep;
            }
            await DeleteMessagesAsync().ConfigureAwait(false);
            return true;
        }

        private async Task DeleteMessagesAsync()
        {
            for (int i = 0; i < messages.Count; i++)
            {
                await messages[i].DeleteAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
