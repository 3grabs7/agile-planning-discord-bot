using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.ReportLogger.Steps
{
    public interface IReportLoggerStep
    {
        Action<DiscordMessage> OnMessageAdded { get; set; }
        IReportLoggerStep NextStep { get; }
        Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);
    }
}
