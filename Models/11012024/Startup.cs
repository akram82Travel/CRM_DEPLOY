using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRMSTUBSOFT.Startup))]
namespace CRMSTUBSOFT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
