using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BettingTips.Startup))]
namespace BettingTips
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
