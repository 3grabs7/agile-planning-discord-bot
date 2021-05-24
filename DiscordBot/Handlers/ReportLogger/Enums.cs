using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Handlers.ReportLogger
{
    public class Enums
    {
        public enum Step
        {
            MeetingType, TimeBox, Summary
        }
        public enum ReportType
        {
            StandUp, Chill, Review, SprintMadness
        }
    }
}
