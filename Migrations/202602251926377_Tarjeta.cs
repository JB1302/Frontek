namespace Frontek_Full_Web_E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tarjeta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tarjetas",
                c => new
                    {
                        IdUsuario = c.Int(nullable: false),
                        NumeroTarjeta = c.String(nullable: false),
                        FechaVencimiento = c.DateTime(nullable: false),
                        CCV = c.String(nullable: false),
                        Propietario = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IdUsuario)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario)
                .Index(t => t.IdUsuario);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tarjetas", "IdUsuario", "dbo.Usuarios");
            DropIndex("dbo.Tarjetas", new[] { "IdUsuario" });
            DropTable("dbo.Tarjetas");
        }
    }
}
