using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.GitHubIssueCreation.Steps
{
    public interface IGitHubIssueCreationStep
    {
        Action<DiscordMessage> OnMessageAdded { get; set; }
        IGitHubIssueCreationStep NextStep { get; }
        Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);
    }
}
