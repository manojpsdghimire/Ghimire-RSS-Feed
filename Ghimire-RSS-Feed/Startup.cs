using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ghimire_RSS_Feed.Startup))]
namespace Ghimire_RSS_Feed
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
