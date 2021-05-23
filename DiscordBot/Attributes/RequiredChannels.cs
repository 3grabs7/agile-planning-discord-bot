using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequireChannels : CheckBaseAttribute
    {
        public ChannelCheckMode CheckMode { get; }
        public IReadOnlyList<string> ChannelNames { get; }
        public RequireChannels(ChannelCheckMode checkMode, params string[] channelNames)
        {
            CheckMode = checkMode;
            ChannelNames = new ReadOnlyCollection<string>(channelNames);
        }

        public override Task<bool> ExecuteCheckAsync(CommandContext context, bool help)
        {
            if (context.Guild == null || context.Member == null)
            {
                return Task.FromResult(false);
            }

            var isChannelAllowed = ChannelNames.Contains(
                context.Channel.Name,
                StringComparer.OrdinalIgnoreCase);

            return CheckMode switch
            {
                ChannelCheckMode.Allow => Task.FromResult(isChannelAllowed),
                ChannelCheckMode.Ban => Task.FromResult(!isChannelAllowed),
                _ => Task.FromResult(false)
            };
        }
    }

    public enum ChannelCheckMode
    {
        Allow, Ban
    }
}
