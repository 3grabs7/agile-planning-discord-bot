using DataAccessLayer;
using DataAccessLayer.Models;
using DiscordBot.Handlers.MeetingScheduler;
using DiscordBot.Handlers.MeetingScheduler.Steps;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Meetings : BaseCommandModule
    {
        private readonly AgilePlanningDiscordBotDbContext _dbContext;
        public Meetings(AgilePlanningDiscordBotDbContext context)
        {
            _dbContext = context;
        }

        [Command("ScheduleMeeting")]
        public async Task ScheduleMeeting(CommandContext context)
        {
            var newMeeting = new Meeting()
            {
                CreatorUserId = context.Message.Author.Id.ToString(),
                GuildId = context.Guild.Id.ToString()
            };

            var dateTimeStep = new DateTimeStep("Enter summary", null);
            var timeboxStep = new TimeboxStep("Enter timebox", dateTimeStep);
            var meetingTypeStep = new MeetingTypeStep("Enter meeting type", timeboxStep);
            var titleStep = new TitleStep("Enter meeting type", meetingTypeStep);

            titleStep.OnValidResult += (result) => newMeeting.Title = result;
            meetingTypeStep.OnValidResult += (result) => newMeeting.MeetingType = result;
            timeboxStep.OnValidResult += (result) => newMeeting.Timebox = result;
            dateTimeStep.OnValidResult += (result) => newMeeting.DateTime = result;

            // open dm
            // var dm = await context.Member.CreateDmChannelAsync().ConfigureAwait(false);

            var meetingScheduler = new MeetingSchedulerHandler(
                context.Client,
                context.Channel,
                context.User,
                titleStep
                );

            var succeeded = await meetingScheduler.ProcessReportLogger().ConfigureAwait(false);

            if (!succeeded) return;

            await _dbContext.AddAsync(newMeeting).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            await context.Channel.SendMessageAsync("Report successfully saved to database").ConfigureAwait(false);
        }

    }
}
