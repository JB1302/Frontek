using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Frontek_Full_Web_E_Commerce.Presentacion.Startup))]
namespace Frontek_Full_Web_E_Commerce.Presentacion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
