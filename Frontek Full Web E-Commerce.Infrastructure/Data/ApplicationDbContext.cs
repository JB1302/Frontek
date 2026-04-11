using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Infrastructure.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Frontek_Full_Web_E_Commerce.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<OrdenDetalle> OrdenDetalles { get; set; }
        public DbSet<Resena> Resenas { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(u => u.Tarjeta)
                .WithRequired(t => t.Usuario);

            modelBuilder.Entity<Orden>()
                .HasRequired(o => o.Usuario)
                .WithMany(u => u.Ordenes)
                .HasForeignKey(o => o.IdUsuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Resena>()
                .HasRequired(r => r.Usuario)
                .WithMany(u => u.Resenas)
                .HasForeignKey(r => r.UsuarioId)
                .WillCascadeOnDelete(false);
        }
    }
}