using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManagementSystemVersionTwo.Startup))]
namespace ManagementSystemVersionTwo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
