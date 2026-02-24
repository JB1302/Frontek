
using System.Data.Entity;

namespace Frontek_Full_Web_E_Commerce.Models
{
    public class FrontekController : DbContext
    {
        public FrontekController() : base("FrontekContext")
        {
        }

        public DbSet<Models.Usuario> Usuarios { get; set; }
        public DbSet<Models.Rol> Roles { get; set; }
        public DbSet<Models.Tarjeta.Tarjeta> Tarjetas { get; set; }
    }
}