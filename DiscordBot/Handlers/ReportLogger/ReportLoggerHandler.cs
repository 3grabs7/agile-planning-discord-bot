using DataAccessLayer;
using DataAccessLayer.Models;
using DiscordBot.Handlers.ReportLogger.Steps;
using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.ReportLogger
{
    public class ReportLoggerHandler
    {
        private readonly DiscordClient _client;
        private readonly DiscordChannel _channel;
        private readonly DiscordUser _user;
        private IReportLoggerStep _currentStep { get; set; }
        private Report reportEntity { get; set; }
        private List<DiscordMessage> messages = new List<DiscordMessage>();

        public ReportLoggerHandler(
            DiscordClient client,
            DiscordChannel channel,
            DiscordUser user,
            IReportLoggerStep startingStep)
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
                        Title = "The report has been successfully saved",
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
