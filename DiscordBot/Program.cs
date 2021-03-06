using DataAccessLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            using (IServiceScope scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AgilePlanningDiscordBotDbContext>();
                // Comment out if database models get updated
                // WARNING! Everything will be lost. Stop crying and do your migrations!
                // context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}