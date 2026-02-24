namespace Frontek_Full_Web_E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductoResenaOrden : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ordens", "IdUsuario", c => c.Int(nullable: false));
            AddColumn("dbo.Ordens", "Usuario_Id", c => c.Int());
            CreateIndex("dbo.Ordens", "Usuario_Id");
            AddForeignKey("dbo.Ordens", "Usuario_Id", "dbo.Usuarios", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ordens", "Usuario_Id", "dbo.Usuarios");
            DropIndex("dbo.Ordens", new[] { "Usuario_Id" });
            DropColumn("dbo.Ordens", "Usuario_Id");
            DropColumn("dbo.Ordens", "IdUsuario");
        }
    }
}
