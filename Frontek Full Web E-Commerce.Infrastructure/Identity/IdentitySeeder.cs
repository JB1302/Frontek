using Frontek_Full_Web_E_Commerce.Infrastructure.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Frontek_Full_Web_E_Commerce.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            string[] roles = { "Administrador", "Vendedor", "Cliente" };

            foreach (var r in roles)
            {
                if (!roleManager.RoleExists(r))
                    roleManager.Create(new IdentityRole(r));
            }
            EnsureUser(userManager,"admin@frontek.com", "Admin123!","Administrador General", "Administrador");
            EnsureUser(userManager,"vendedor@frontek.com", "Admin123!","Vendedor Principal", "Vendedor");
            EnsureUser(userManager,"cliente@frontek.com", "Admin123!","Cliente Uno", "Cliente");
            db.SaveChanges();
        }

        private static void EnsureUser(
            UserManager<ApplicationUser> um,
            string email, string pass,
            string nombre, string role)
        {
            var u = um.FindByEmail(email);

            if (u == null)
            {
                u = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    Nombre = nombre,
                    Activo = true
                };
                um.Create(u, pass);
            }

            if (!um.IsInRole(u.Id, role))
                um.AddToRole(u.Id, role);
        }
    }
}