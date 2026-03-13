using Frontek_Full_Web_E_Commerce.Helpers;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Frontek.Startup))]
namespace Frontek
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CrearRoles.CrearRolesBase();
        }
    }
}
