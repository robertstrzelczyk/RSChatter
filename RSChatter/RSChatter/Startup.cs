using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RSChatter.Startup))]
namespace RSChatter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
