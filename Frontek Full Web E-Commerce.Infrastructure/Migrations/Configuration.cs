namespace Frontek_Full_Web_E_Commerce.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Frontek_Full_Web_E_Commerce.Infrastructure.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Frontek_Full_Web_E_Commerce.Infrastructure.Data.ApplicationDbContext context)
        {
            Frontek_Full_Web_E_Commerce.Infrastructure.Identity.IdentitySeeder.Seed(context);
        }
    }
}
