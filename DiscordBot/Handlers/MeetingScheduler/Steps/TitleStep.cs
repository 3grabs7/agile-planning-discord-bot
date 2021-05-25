using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.MeetingScheduler.Steps
{
    public class TitleStep : MeetingSchedulerStepBase
    {
        private readonly IMeetingSchedulerStep _nextStep;

        public TitleStep(string content, IMeetingSchedulerStep nextStep) : base(content)
        {
            _nextStep = nextStep;
        }

        public Action<string> OnValidResult { get; set; } = delegate { };

        public override IMeetingSchedulerStep NextStep => _nextStep;

        public async override Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = "Please respond with the title of the meeting to schedule"
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

                OnValidResult(messageResult.Result.Content);

                return false;
            }
        }
    }
}
