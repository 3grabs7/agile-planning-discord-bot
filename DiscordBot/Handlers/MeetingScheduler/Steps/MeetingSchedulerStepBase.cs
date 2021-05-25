using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.MeetingScheduler.Steps
{
    public abstract class MeetingSchedulerStepBase : IMeetingSchedulerStep
    {
        protected readonly string _content;

        public MeetingSchedulerStepBase(string content)
        {
            _content = content;
        }

        public Action<DiscordMessage> OnMessageAdded { get; set; } = delegate { };
        public abstract IMeetingSchedulerStep NextStep { get; }

        public abstract Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);

        protected async Task TryAgain(DiscordChannel channel, string problem)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Something went wrong, try again",
                Color = DiscordColor.IndianRed
            };

            embed.AddField("Error", problem);

            var embedResult = await channel.SendMessageAsync(embed: embed).ConfigureAwait(false);

            OnMessageAdded(embedResult);
        }

    }
}
