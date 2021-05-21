using DataAccessLayer;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    class ConnectToGitHub : BaseCommandModule
    {
        private readonly AgilePlanningDiscordBotDbContext _context;
        public ConnectToGitHub(AgilePlanningDiscordBotDbContext context)
        {
            _context = context;
        }
        [Command("Connect")]
        public async Task Connect(CommandContext context, string clientId, string login)
        {

        }

    }
}
