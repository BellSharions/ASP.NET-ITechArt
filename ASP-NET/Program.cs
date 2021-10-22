using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using DAL;
using Serilog;

namespace ASP_NET
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message} {NewLine} {Exception}";
                Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().
                    WriteTo.Map(
                    evt => evt.Level,
                    (level, wt) => wt.File($"../Logs/{level}-{DateTime.Now.Date.ToShortDateString().Replace("/", "-")}.log", outputTemplate: outputTemplate)).
                    CreateLogger();
                var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            }

            host.Run();
            CreateHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                Log.Error(ex, ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
