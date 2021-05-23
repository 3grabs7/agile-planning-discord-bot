using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class RoleManager : BaseCommandModule
    {
        [Command("AssignRole")]
        public async Task AssignRole(CommandContext context, DiscordUser user, DiscordRole role)
        {
            var joinEmbed = new DiscordEmbedBuilder
            {
                Title = "Ready or not",
                Description =
                    $"The role '{role.Name}' is free for the taking. You want it?",
                ImageUrl = user.AvatarUrl,
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

                await context.Member.ReplaceRolesAsync(
                    new List<DiscordRole>()
                    {
                        role
                    })
                    .ConfigureAwait(false);
            }

            await context.Message.DeleteAsync().ConfigureAwait(false);
            await joinMessage.DeleteAsync().ConfigureAwait(false);
        }

        [Command("GetRoles")]
        public async Task GetRoles(CommandContext context)
        {
            // === TODO ===
            // =============================================================
            // Get back to this, can't get all users from guild
            // this async method freezes / .Members only gets online members
            // =============================================================
            var members = await context.Guild.GetAllMembersAsync().ConfigureAwait(false);
            var dunder = members
                .GroupBy(g => g.Roles.FirstOrDefault())
                .Select(g => $"{g.Key.Name} : {string.Join(", ", g.Select(m => m.Username))}");

            var rolesEmbed = new DiscordEmbedBuilder()
            {
                Title = "Currently held roles",
                Description = String.Join("\n", members),
                Color = DiscordColor.Blurple,
            };

            await context.RespondAsync(rolesEmbed).ConfigureAwait(false);
        }
    }
}
