using BettingTips.Tasks;
using Hangfire;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(BettingTips.Startup))]
namespace BettingTips
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection");

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            ConfigureAuth(app);

            RecurringJob.AddOrUpdate(() => Jobs.ScheduleTipMessages(), Cron.Daily, TimeZoneInfo.Local);
        }
    }
}
