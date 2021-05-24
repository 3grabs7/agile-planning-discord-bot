using DataAccessLayer;
using DataAccessLayer.Models;
using DiscordBot.Attributes;
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
    public class ReportCommands : BaseCommandModule
    {
        private readonly AgilePlanningDiscordBotDbContext _dbContext;
        public ReportCommands(AgilePlanningDiscordBotDbContext dbContext)
        {
            _dbContext = dbContext;
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

            var interactivity = context.Client.GetInteractivity();

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

        [Command("GoToReports")]
        [Description("Get all reports")]
        public async Task GoToReports(CommandContext context)
        {
            var reports = _dbContext.Reports
                .AsNoTracking()
                .Where(r => r.UserId == context.Message.Author.Id.ToString());

            var reportMessage = new DiscordEmbedBuilder()
            {
                Title = $"Links to all reports",
                Description = $"{reports.Count()} reports found"
            };

            var channelMessages = await context.Channel.GetMessagesAsync(9999);

            foreach (var report in channelMessages)
            {
                if (report.Author != context.Message.Author)
                {
                    reportMessage.AddField($"Link : ", report.JumpLink.AbsoluteUri);
                }
            }
            await context.Channel.SendMessageAsync(reportMessage).ConfigureAwait(false);
            await context.Message.DeleteAsync().ConfigureAwait(false);

        }

        [Command("GetReportsByType")]
        [Description("Get report of specific type")]
        public async Task GetReportsByType(CommandContext context, string type)
        {

        }

        [Command("GetReportsByDate")]
        [Description("Get report from specific date {00 'Month'}")]
        public async Task GetReportsByDate(CommandContext context, string date)
        {

        }
    }
}
