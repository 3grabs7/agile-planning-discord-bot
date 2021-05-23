using DiscordBot.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Polls : BaseCommandModule
    {
        [RequireChannels(ChannelCheckMode.Allow, "standup🍟")]
        [Command("StandUp")]
        public async Task StandUp(CommandContext context, string time, params DiscordEmoji[] options)
        {
            var emojis = new List<DiscordEmoji>();
            if (options.Count() < 3)
            {
                emojis.AddRange(new List<string>()
                {
                    ":+1:",
                    ":sick:",
                    ":-1:"
                }
                .Select(e => DiscordEmoji.FromName(context.Client, e)));
            }
            var interactivity = context.Client.GetInteractivity();

            var embed = new DiscordEmbedBuilder
            {
                Title = $"Dags för en standup vid {time}?",
                Description = "Gogogogogogogogogogogogo? Get your votes in, you got 5 min"
            };

            var pollMessage = await context.Channel.SendMessageAsync(embed).ConfigureAwait(false);

            var result = await interactivity.DoPollAsync(pollMessage, emojis, PollBehaviour.DeleteEmojis, TimeSpan.FromMinutes(5))
                .ConfigureAwait(false);

            var pollResult = result.OrderByDescending(r => r.Total).ToList();

            var pollResponse =
                pollResult[0].Emoji == emojis[0] ?
                    "@everyone You got it, let's do it" :
                pollResult[0].Emoji == emojis[1] ?
                    "@everyone I guess we'd rather die." :
                    "@everyone No go, no show right now";

            await context.Channel.SendMessageAsync(pollResponse).ConfigureAwait(false);
        }
    }
}
