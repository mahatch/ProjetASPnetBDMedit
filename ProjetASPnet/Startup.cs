using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjetASPnet.Startup))]
namespace ProjetASPnet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
