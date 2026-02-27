namespace Frontek_Full_Web_E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrdenDetalles",
                c => new
                    {
                        OrdenDetalleId = c.Int(nullable: false, identity: true),
                        OrdenId = c.Int(nullable: false),
                        NombreProducto = c.String(),
                        SKU = c.String(maxLength: 50),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Garantia = c.String(maxLength: 200),
                        ProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrdenDetalleId)
                .ForeignKey("dbo.Ordens", t => t.OrdenId, cascadeDelete: true)
                .ForeignKey("dbo.Productoes", t => t.ProductoId, cascadeDelete: true)
                .Index(t => t.OrdenId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Ordens",
                c => new
                    {
                        OrdenId = c.Int(nullable: false, identity: true),
                        NumeroOrden = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                        ClienteId = c.String(),
                        NombreCliente = c.String(),
                        EmailCliente = c.String(),
                        DireccionEnvio = c.String(),
                        Ciudad = c.String(),
                        Pais = c.String(),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MetodoPago = c.String(),
                        Estado = c.String(),
                        FechaEntregaEstimada = c.DateTime(),
                        IdUsuario = c.Int(nullable: false),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.OrdenId)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .Index(t => t.Usuario_Id);
            
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
            
            CreateTable(
                "dbo.Rols",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 75),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resenas", "UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.Resenas", "ProductoId", "dbo.Productoes");
            DropForeignKey("dbo.OrdenDetalles", "ProductoId", "dbo.Productoes");
            DropForeignKey("dbo.Ordens", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Tarjetas", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.Usuarios", "RolId", "dbo.Rols");
            DropForeignKey("dbo.OrdenDetalles", "OrdenId", "dbo.Ordens");
            DropIndex("dbo.Resenas", new[] { "UsuarioId" });
            DropIndex("dbo.Resenas", new[] { "ProductoId" });
            DropIndex("dbo.Tarjetas", new[] { "IdUsuario" });
            DropIndex("dbo.Usuarios", new[] { "RolId" });
            DropIndex("dbo.Ordens", new[] { "Usuario_Id" });
            DropIndex("dbo.OrdenDetalles", new[] { "ProductoId" });
            DropIndex("dbo.OrdenDetalles", new[] { "OrdenId" });
            DropTable("dbo.Resenas");
            DropTable("dbo.Productoes");
            DropTable("dbo.Tarjetas");
            DropTable("dbo.Rols");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Ordens");
            DropTable("dbo.OrdenDetalles");
        }
    }
}
