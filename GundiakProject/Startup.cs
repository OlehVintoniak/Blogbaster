using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GundiakProject.Startup))]
namespace GundiakProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
