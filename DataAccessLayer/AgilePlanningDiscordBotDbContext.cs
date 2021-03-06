using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AgilePlanningDiscordBotDbContext : DbContext
    {
        public AgilePlanningDiscordBotDbContext(DbContextOptions options) : base(options) { }

        public DbSet<GitHubInformation> GitHubInformations { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
    }
}
