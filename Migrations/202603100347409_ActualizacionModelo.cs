namespace Frontek_Full_Web_E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActualizacionModelo : DbMigration
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
            
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreProducto = c.String(nullable: false, maxLength: 150),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        FechaIngreso = c.DateTime(nullable: false),
                        FechaMod = c.DateTime(),
                        Imagen1 = c.Binary(),
                        Imagen2 = c.Binary(),
                        Imagen3 = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Resenas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductoId = c.Int(nullable: false),
                        UsuarioId = c.Int(nullable: false),
                        Titulo = c.String(maxLength: 100),
                        Cuerpo = c.String(nullable: false, maxLength: 1000),
                        Calificacion = c.Int(nullable: false),
                        Estado = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Productoes", t => t.ProductoId, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.ProductoId)
                .Index(t => t.UsuarioId);
            
            AddColumn("dbo.Ordens", "IdUsuario", c => c.Int(nullable: false));
            AddColumn("dbo.Ordens", "Usuario_Id", c => c.Int());
            AlterColumn("dbo.OrdenDetalles", "NombreProducto", c => c.String());
            CreateIndex("dbo.OrdenDetalles", "ProductoId");
            CreateIndex("dbo.Ordens", "Usuario_Id");
            AddForeignKey("dbo.Ordens", "Usuario_Id", "dbo.Usuarios", "Id");
            AddForeignKey("dbo.OrdenDetalles", "ProductoId", "dbo.Productoes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resenas", "UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.Resenas", "ProductoId", "dbo.Productoes");
            DropForeignKey("dbo.OrdenDetalles", "ProductoId", "dbo.Productoes");
            DropForeignKey("dbo.Ordens", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Tarjetas", "IdUsuario", "dbo.Usuarios");
            DropIndex("dbo.Resenas", new[] { "UsuarioId" });
            DropIndex("dbo.Resenas", new[] { "ProductoId" });
            DropIndex("dbo.Tarjetas", new[] { "IdUsuario" });
            DropIndex("dbo.Ordens", new[] { "Usuario_Id" });
            DropIndex("dbo.OrdenDetalles", new[] { "ProductoId" });
            AlterColumn("dbo.OrdenDetalles", "NombreProducto", c => c.String(nullable: false, maxLength: 150));
            DropColumn("dbo.Ordens", "Usuario_Id");
            DropColumn("dbo.Ordens", "IdUsuario");
            DropTable("dbo.Resenas");
            DropTable("dbo.Productoes");
            DropTable("dbo.Tarjetas");
        }
    }
}
