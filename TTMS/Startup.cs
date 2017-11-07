using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TTMS.Startup))]
namespace TTMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
