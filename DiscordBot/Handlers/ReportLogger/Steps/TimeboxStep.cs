using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.ReportLogger.Steps
{
    public class TimeboxStep : RepportLoggerStepBase
    {
        private readonly IReportLoggerStep _nextStep;

        public TimeboxStep(string content, IReportLoggerStep nextStep) : base(content, Enums.Step.TimeBox)
        {
            _nextStep = nextStep;
        }

        public Action<string> OnValidResult { get; set; } = delegate { };

        public override IReportLoggerStep NextStep => _nextStep;

        public async override Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var meetingTypes = String.Join(", ", Enum.GetNames(typeof(Enums.ReportType)));
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Please respond with the timebox of the meeting",
                Description = "Format your timebox as {0h 00m}",
            };

            embedBuilder.AddField("Note", "Type command '!cancel' to stop and remove report");

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

                OnMessageAdded(embed);

                var messageResult = await interactivity.WaitForMessageAsync(m =>
                    m.ChannelId == channel.Id &&
                    m.Author.Id == user.Id)
                    .ConfigureAwait(false);

                OnMessageAdded(messageResult.Result);

                if (messageResult.Result.Content.Equals("!cancel", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (!ValidateTimebox(messageResult.Result.Content))
                {
                    await TryAgain(channel, $"The timebox '{messageResult.Result.Content}' is not written in a valid format.")
                        .ConfigureAwait(false);
                    continue;
                }

                OnValidResult(messageResult.Result.Content);

                return false;
            }
        }

        private bool ValidateTimebox(string timebox)
        {
            return true;
        }
    }
}
