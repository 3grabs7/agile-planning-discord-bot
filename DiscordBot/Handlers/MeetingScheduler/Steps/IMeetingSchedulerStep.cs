using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.MeetingScheduler.Steps
{
    public interface IMeetingSchedulerStep
    {
        Action<DiscordMessage> OnMessageAdded { get; set; }
        IMeetingSchedulerStep NextStep { get; }
        Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);
    }
}
