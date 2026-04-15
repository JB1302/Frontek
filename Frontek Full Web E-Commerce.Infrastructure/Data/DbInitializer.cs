using Frontek_Full_Web_E_Commerce.Infrastructure.Identity;
using System.Data.Entity;

namespace Frontek_Full_Web_E_Commerce.Infrastructure.Data
{
    public class DbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            IdentitySeeder.Seed(context);

            base.Seed(context);
        }
    }
}