
using System.Data.Entity;
using System.Reflection.Emit;

namespace Frontek_Full_Web_E_Commerce.Models
{
    public class FrontekController : DbContext
    {
        public FrontekController() : base("FrontekContext")
        {
        }

        public DbSet<Models.Usuario> Usuarios { get; set; }
        public DbSet<Models.Rol> Roles { get; set; }
        public DbSet<Models.Orden> Ordenes { get; set; }
        public DbSet<Models.OrdenDetalle> OrdenDetalles { get; set; }
        public DbSet<Models.Producto> Producto { get; set; }
        public DbSet<Models.Resena> Resena { get; set; }
        public DbSet<Models.Tarjeta.Tarjeta> Tarjetas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasOptional(u => u.Tarjeta)
                .WithRequired(t => t.Usuario);
        }
    }
}