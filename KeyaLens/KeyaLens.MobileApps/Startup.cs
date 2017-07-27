using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(KeyaLens.MobileApps.Startup))]

namespace KeyaLens.MobileApps
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}