using DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AgilePlanningDiscordBotDbContext>(options =>
                {
                    options.UseSqlServer(
                        "Server=(localdb)\\mssqllocaldb;Database=AgilePlanningDiscordBot;Trusted_Connection=True;MultipleActiveResultSets=True"
                        );
                });

            var serviceProvider = services.BuildServiceProvider();

            var bot = new Bot(serviceProvider);

            services.AddSingleton(bot);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}
