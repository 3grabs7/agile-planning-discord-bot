using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class RoleManager : BaseCommandModule
    {
        [Command("Join")]
        public async Task Join(CommandContext context, DiscordUser user, DiscordRole role)
        {
            var joinEmbed = new DiscordEmbedBuilder
            {
                Title = "Ready or not",
                Description =
                    $"The role '{role.Name}' is free for the taking. You want it?",
                ImageUrl = context.Client.CurrentUser.AvatarUrl,
                Color = DiscordColor.Blurple,
            };

            var joinMessage = await context.Channel.SendMessageAsync(embed: joinEmbed).ConfigureAwait(false);

            var reactionYes = DiscordEmoji.FromName(context.Client, ":+1:");
            var reactionNo = DiscordEmoji.FromName(context.Client, ":-1:");
            await joinMessage.CreateReactionAsync(reactionYes).ConfigureAwait(false);
            await joinMessage.CreateReactionAsync(reactionNo).ConfigureAwait(false);

            var interactivity = context.Client.GetInteractivity();
            var reaction = await interactivity.WaitForReactionAsync(m =>
               m.Message == joinMessage &&
               m.User == user &&
               (m.Emoji == reactionYes || m.Emoji == reactionNo))
               .ConfigureAwait(false);

            if (reaction.Result.Emoji == reactionYes)
            {
                await context.Member.ReplaceRolesAsync(new List<DiscordRole> { role }).ConfigureAwait(false);
                return;
            }

            await joinMessage.DeleteAsync().ConfigureAwait(false);
        }
    }
}
