using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Stress : BaseCommandModule
    {
        [Command("Hi")]
        [Description("If no one answers you, I got ya")]
        public async Task Hi(CommandContext context)
        {
            await context.Channel.SendMessageAsync("'Sup?")
                .ConfigureAwait(false);
        }

        [Command("Panic")]
        [Description("Don't panic it's all good, I got you")]
        public async Task Panic(CommandContext context)
        {
            await context.Channel.SendMessageAsync($"Don't worry child, you're {context.Member.DisplayName}, " +
                $"right now your in {context.Channel.Name}. " +
                $"In {context.Guild.Owner.DisplayName} safe hands.")
                .ConfigureAwait(false);
        }
    }
}
