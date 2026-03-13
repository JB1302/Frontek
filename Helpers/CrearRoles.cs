using System.Linq;
using Frontek.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Frontek_Full_Web_E_Commerce.Helpers
{
    public class CrearRoles
    {
        public static void CrearRolesBase()
        {
            using (var context = new ApplicationDbContext())
            {
                string[] roles = { "Cliente", "Administrador", "Vendedor" };

                foreach (var rol in roles)
                {
                    if (!context.Roles.Any(r => r.Name == rol))
                    {
                        context.Roles.Add(new IdentityRole
                        {
                            Name = rol
                        });
                    }
                }

                context.SaveChanges();
            }
        }
    }
}