using DataAccessLayer;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class ConnectToGitHub : BaseCommandModule
    {
        private readonly AgilePlanningDiscordBotDbContext _context;
        public ConnectToGitHub(AgilePlanningDiscordBotDbContext context)
        {
            _context = context;
        }

        [Command("Connect")]
        public async Task Connect(CommandContext context, string githubToken)
        {
            //var guildId = context.Guild.Id.ToString();
            //var findGuild = _context.GitHubIdentitys
            //    .FirstOrDefault(g => g.GuildId == guildId);

            //if (findGuild == default)
            //{
            //    await context.Channel.SendMessageAsync("Seems you already registered a github application");
            //    return;
            //}

            var authResponse = await GitHubRequests.AuthorizeAsync(githubToken);
            if (authResponse == null)
            {
                await context.Channel.SendMessageAsync("Couldn't authorize github app");
            }

            //var githubApp = new GitHubIdentity()
            //{
            //    GuildId = context.Guild.Id.ToString(),
            //    ClientId = clientId,
            //    Login = login
            //};

            //await _context.AddAsync(githubApp);
            //await _context.SaveChangesAsync();

            //await context.Channel.SendMessageAsync("App saved osv -> random info");
        }

    }
}
