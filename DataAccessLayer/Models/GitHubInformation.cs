using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class GitHubInformation : Entity
    {
        public string GuildId { get; set; }
        public string GitHubToken { get; set; }
        public string DiscordUserId { get; set; }
        public string RepositoryName { get; set; }
        public string Url { get; set; }
    }
}
