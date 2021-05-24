using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Report : Entity
    {
        public string UserId { get; set; }
        public string MeetingType { get; set; }
        public string Summary { get; set; }
        public string Timebox { get; set; }
        public string Date { get; set; }

    }
}
