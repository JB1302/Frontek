namespace Frontek_Full_Web_E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrimeraMigracion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rols",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 75),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 75),
                        Email = c.String(nullable: false, maxLength: 75),
                        Contrasenia = c.String(nullable: false, maxLength: 150),
                        UltimaConexion = c.DateTime(),
                        Activo = c.Boolean(nullable: false),
                        RolId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rols", t => t.RolId, cascadeDelete: true)
                .Index(t => t.RolId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "RolId", "dbo.Rols");
            DropIndex("dbo.Usuarios", new[] { "RolId" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.Rols");
        }
    }
}
