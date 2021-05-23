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
    public class RequireCategories : CheckBaseAttribute
    {
        public CategoryCheckMode CheckMode { get; }
        public IReadOnlyList<string> CategoryNames { get; }
        public RequireCategories(CategoryCheckMode checkMode, params string[] categoriesNames)
        {
            CheckMode = checkMode;
            CategoryNames = new ReadOnlyCollection<string>(categoriesNames);
        }

        public override Task<bool> ExecuteCheckAsync(CommandContext context, bool help)
        {
            if (context.Guild == null || context.Member == null)
            {
                return Task.FromResult(false);
            }

            var isChannelAllowed = CategoryNames.Contains(
                context.Channel.Parent.Name,
                StringComparer.OrdinalIgnoreCase);

            return CheckMode switch
            {
                CategoryCheckMode.Any => Task.FromResult(isChannelAllowed),
                CategoryCheckMode.None => Task.FromResult(!isChannelAllowed),
                _ => Task.FromResult(false)
            };
        }
    }

    public enum CategoryCheckMode
    {
        Any, None, MineOrParentAny
    }
}
