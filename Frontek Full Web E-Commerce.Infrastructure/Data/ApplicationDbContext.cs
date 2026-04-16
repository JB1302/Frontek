using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Infrastructure.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Frontek_Full_Web_E_Commerce.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            System.Diagnostics.Debug.WriteLine(Database.Connection.ConnectionString);
        }
        public DbSet<Producto> Productos { get; set; }
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

            modelBuilder.Entity<Tarjeta>()
                .HasKey(t => t.IdUsuario);

            modelBuilder.Entity<Orden>()
                .HasKey(o => o.OrdenId);

            modelBuilder.Entity<Orden>()
                .Property(o => o.IdUsuario)
                .IsRequired();

            modelBuilder.Entity<Resena>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Resena>()
                .Property(r => r.UsuarioId)
                .IsRequired();

            modelBuilder.Entity<OrdenDetalle>()
                .HasKey(od => od.OrdenDetalleId);

            modelBuilder.Entity<OrdenDetalle>()
                .Property(od => od.OrdenId)
                .IsRequired();

            modelBuilder.Entity<OrdenDetalle>()
                .Property(od => od.ProductoId)
                .IsRequired();
            modelBuilder.Entity<Producto>()
                .HasKey(p => p.Id);
        }
    }
}