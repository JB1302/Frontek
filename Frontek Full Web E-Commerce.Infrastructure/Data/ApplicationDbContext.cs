using Microsoft.AspNet.Identity.EntityFramework;

namespace Frontek_Full_Web_E_Commerce.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {

        }

        /*
            Aqui como antes haciamos en DbContext
         
            Ejemplo

            public DbSet<Producto> Productos { get; set; }
         
         */

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
