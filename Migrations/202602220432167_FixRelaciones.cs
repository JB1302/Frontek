namespace Frontek_Full_Web_E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRelaciones : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Usuarios", "Rol_Id", "dbo.Rols");
            DropIndex("dbo.Usuarios", new[] { "Rol_Id" });
            RenameColumn(table: "dbo.Usuarios", name: "Rol_Id", newName: "RolId");
            AlterColumn("dbo.Usuarios", "RolId", c => c.Int(nullable: false));
            CreateIndex("dbo.Usuarios", "RolId");
            AddForeignKey("dbo.Usuarios", "RolId", "dbo.Rols", "Id", cascadeDelete: true);
            DropColumn("dbo.Usuarios", "IdRol");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "IdRol", c => c.Int(nullable: false));
            DropForeignKey("dbo.Usuarios", "RolId", "dbo.Rols");
            DropIndex("dbo.Usuarios", new[] { "RolId" });
            AlterColumn("dbo.Usuarios", "RolId", c => c.Int());
            RenameColumn(table: "dbo.Usuarios", name: "RolId", newName: "Rol_Id");
            CreateIndex("dbo.Usuarios", "Rol_Id");
            AddForeignKey("dbo.Usuarios", "Rol_Id", "dbo.Rols", "Id");
        }
    }
}
