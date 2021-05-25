using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Meeting : Entity
    {
        public string CreatorUserId { get; set; }
        public string GuildId { get; set; }
        public string DateTime { get; set; }
        public string Title { get; set; }
        public string MeetingType { get; set; }
        public string Timebox { get; set; }
    }
}
