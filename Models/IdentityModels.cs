using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Frontek_Full_Web_E_Commerce.Models;
using Frontek_Full_Web_E_Commerce.Models.Tarjeta;

namespace Frontek.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(75)]
        public string Nombre { get; set; }

        [Display(Name = "Última conexión")]
        public DateTime? UltimaConexion { get; set; } = DateTime.Now;

        public bool Activo { get; set; } = true;

        public virtual Tarjeta Tarjeta { get; set; }
        public virtual ICollection<Orden> Ordenes { get; set; }
        public virtual ICollection<Resena> Resenas { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(
                this,
                DefaultAuthenticationTypes.ApplicationCookie
            );

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<OrdenDetalle> OrdenDetalles { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Resena> Resenas { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }

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