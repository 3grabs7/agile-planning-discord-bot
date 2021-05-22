using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class GitHubIdentity : Entity
    {
        public string GuildId { get; set; }
        public int ClientId { get; set; }
        public string State { get; set; }
        public string Login { get; set; }
    }
}
