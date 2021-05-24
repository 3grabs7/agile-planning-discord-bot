﻿using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.ReportLogger.Steps
{
    public class MeetingTypeStep : RepportLoggerStepBase
    {
        private readonly IReportLoggerStep _nextStep;

        public MeetingTypeStep(string content, IReportLoggerStep nextStep) : base(content, Enums.Step.MeetingType)
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
                Title = "Please respond with meeting type",
                Description = $"Available options : {meetingTypes}"
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

                if (!ValidateMeetingType(messageResult.Result.Content))
                {
                    await TryAgain(channel, $"The meeting type '{messageResult.Result.Content}' is not vaild.")
                        .ConfigureAwait(false);
                    continue;
                }

                OnValidResult(messageResult.Result.Content);

                return false;

            }
        }

        private bool ValidateMeetingType(string meetingType)
        {
            return Enum.GetNames(typeof(Enums.ReportType)).Contains(meetingType);
        }
    }
}
