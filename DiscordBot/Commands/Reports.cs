using DataAccessLayer;
using DataAccessLayer.Models;
using DiscordBot.Attributes;
using DiscordBot.Handlers.ReportLogger;
using DiscordBot.Handlers.ReportLogger.Steps;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    [RequireCategories(CategoryCheckMode.Any, "Reports")]
    public class Reports : BaseCommandModule
    {
        private readonly AgilePlanningDiscordBotDbContext _dbContext;
        public Reports(AgilePlanningDiscordBotDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Create report with Handler
        // Steps -> MeetingType, Timebox, Summary
        [Command("ReportInit")]
        public async Task InitializeReportLogger(CommandContext context)
        {
            var newReport = new Report()
            {
                UserId = context.Message.Author.Id.ToString(),
                GuildId = context.Guild.Id.ToString(),
                Date = DateTime.Now.ToString("dddd, dd MMMM HH:mm")
            };

            var summaryStep = new SummaryStep("Enter summary", null);
            var timeboxStep = new TimeboxStep("Enter timebox", summaryStep);
            var meetingTypeStep = new MeetingTypeStep("Enter meeting type", timeboxStep);

            meetingTypeStep.OnValidResult += (result) => newReport.MeetingType = result;
            summaryStep.OnValidResult += (result) => newReport.Summary = result;
            timeboxStep.OnValidResult += (result) => newReport.Timebox = result;

            // open dm
            // var dm = await context.Member.CreateDmChannelAsync().ConfigureAwait(false);

            var reportLogger = new ReportLoggerHandler(
                context.Client,
                context.Channel,
                context.User,
                meetingTypeStep
                );

            var succeeded = await reportLogger.ProcessReportLogger().ConfigureAwait(false);

            if (!succeeded) return;

            await _dbContext.AddAsync(newReport).ConfigureAwait(false);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            await context.Channel.SendMessageAsync("Report successfully saved to database").ConfigureAwait(false);
        }

        [Command("ReportTemplate")]
        public async Task ReportTemplate(CommandContext context, string type, string timebox, string summary)
        {
            var reportMessage = new DiscordEmbedBuilder()
            {
                Title = $"Report for {type}"
            };
            reportMessage.AddField("Summary", summary);
            reportMessage.AddField("Timebox", timebox);
            reportMessage.WithFooter($"Created At {DateTime.Now.ToString("dddd, dd MMMM HH:mm")}");

            await context.Channel.SendMessageAsync(reportMessage).ConfigureAwait(false);
            await context.Message.DeleteAsync().ConfigureAwait(false);

            var reportEntity = new Report
            {
                UserId = context.Message.Author.Id.ToString(),
                MeetingType = type,
                Summary = summary,
                Timebox = timebox,
                Date = reportMessage.Footer.Text.Replace("Created At ", "")
            };

            await _dbContext.AddAsync(reportEntity);
            await _dbContext.SaveChangesAsync();
        }

        [Command("GetAllReports")]
        [Description("Get all reports")]
        public async Task GetAllReports(CommandContext context)
        {
            var reports = _dbContext.Reports
                .AsNoTracking()
                .Where(r => r.UserId == context.Message.Author.Id.ToString());

            foreach (var report in reports)
            {
                var reportMessage = new DiscordEmbedBuilder()
                {
                    Title = $"Report for {report.MeetingType}"
                };
                reportMessage.AddField("Summary", report.Summary);
                reportMessage.AddField("Timebox", report.Timebox);
                reportMessage.WithFooter($"Created At {report.Date}");

                await context.Channel.SendMessageAsync(reportMessage).ConfigureAwait(false);
            }

            await context.Message.DeleteAsync().ConfigureAwait(false);

        }

        [Command("GetReportsByType")]
        [Description("Get report of specific type")]
        public async Task GetReportsByType(CommandContext context, string type)
        {
            var reports = _dbContext.Reports
                .AsNoTracking()
                .Where(r => r.UserId == context.Message.Author.Id.ToString() &&
                    r.MeetingType == type);

            foreach (var report in reports)
            {
                var reportMessage = new DiscordEmbedBuilder()
                {
                    Title = $"Report for {report.MeetingType}"
                };
                reportMessage.AddField("Summary", report.Summary);
                reportMessage.AddField("Timebox", report.Timebox);
                reportMessage.WithFooter($"Created At {report.Date}");

                await context.Channel.SendMessageAsync(reportMessage).ConfigureAwait(false);
            }

            await context.Message.DeleteAsync().ConfigureAwait(false);
        }

        [Command("GetReportsByDate")]
        [Description("Get report from specific date {00 'Month'}")]
        public async Task GetReportsByDate(CommandContext context, string date)
        {
            var reports = _dbContext.Reports
            .AsNoTracking()
            .Where(r => r.UserId == context.Message.Author.Id.ToString() &&
                r.Date == date);

            foreach (var report in reports)
            {
                var reportMessage = new DiscordEmbedBuilder()
                {
                    Title = $"Report for {report.MeetingType}"
                };
                reportMessage.AddField("Summary", report.Summary);
                reportMessage.AddField("Timebox", report.Timebox);
                reportMessage.WithFooter($"Created At {report.Date}");

                await context.Channel.SendMessageAsync(reportMessage).ConfigureAwait(false);
            }

            await context.Message.DeleteAsync().ConfigureAwait(false);
        }

        [Command("GoToReports")]
        [Description("Get links that move to all reports found in channel")]
        public async Task GoToReports(CommandContext context)
        {
            var reportMessage = new DiscordEmbedBuilder()
            {
                Title = $"Links to all reports"
            };

            var channelMessages = await context.Channel.GetMessagesAsync(9999);
            for (int i = 0; i < channelMessages.Count; i++)
            {
                if (channelMessages[i].Embeds != null)
                {
                    reportMessage.AddField($"Link {channelMessages[i].Embeds.First().Title}",
                        channelMessages[i].JumpLink.AbsoluteUri);
                }
            }

            await context.Channel.SendMessageAsync(reportMessage).ConfigureAwait(false);
            await context.Message.DeleteAsync().ConfigureAwait(false);

        }
    }
}
