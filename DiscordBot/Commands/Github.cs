using DataAccessLayer;
using DataAccessLayer.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Github : BaseCommandModule
    {
        private readonly AgilePlanningDiscordBotDbContext _dbContext;
        public Github(AgilePlanningDiscordBotDbContext context)
        {
            _dbContext = context;
        }

        [Command("GitHubInit")]
        public async Task InitializeConnectionToGitHub(CommandContext context, string githubToken, string username, string repoName)
        {
            var existingConnection = await _dbContext.GitHubInformations
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.GuildId == context.Guild.Id.ToString())
                .ConfigureAwait(false);
            if (existingConnection != default)
            {
                var embedBuilder = new DiscordEmbedBuilder
                {
                    Title = "Already connected"
                };
                embedBuilder.AddField("Connected", $"{context.User.Mention}, " +
                    $"this server is already connected to repo '{existingConnection.RepositoryName}'");

                await context.Channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);
                return;
            }

            var githubInformation = new GitHubInformation
            {
                GuildId = context.Guild.Id.ToString(),
                GitHubToken = githubToken,
                DiscordUserId = context.User.Id.ToString(),
                RepositoryName = repoName,
                Url = $"https://api.github.com/repos/{username}/{repoName}"
            };

            await _dbContext.AddAsync(githubInformation);
            await _dbContext.SaveChangesAsync();

            await context.Channel.SendMessageAsync("Successfully saved github information, you're now ready to connect.");
        }

        [Command("GitHubCreateIssue")]
        public async Task CreateGitHubIssue(CommandContext context)
        {

        }



    }
}
