using DiscordBot.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    [RequireCategories(CategoryCheckMode.Any, "bot-channels")]
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
                $"In {context.Client.CurrentApplication.Name}s safe hands.")
                .ConfigureAwait(false);
        }

        [Command("Riddle")]
        public async Task Riddle(CommandContext context, string riddle, string answer)
        {
            var interactivity = context.Client.GetInteractivity();

            await context.Channel.SendMessageAsync(
                $"Hey! {context.Message.Author.Username} got a riddle for you. '{riddle}'"
                );

            var msg = await interactivity.WaitForMessageAsync(
                x => x.Content.Equals(answer, StringComparison.InvariantCultureIgnoreCase)
                ).ConfigureAwait(false);
            await context.Channel.SendMessageAsync(
                $"Nice '{msg.Result.Author.Username}'! '{msg.Result.Content}' was correct. You smart osv..."
                );
        }
    }
}
