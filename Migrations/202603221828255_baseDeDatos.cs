namespace Frontek_Full_Web_E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class baseDeDatos : DbMigration
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
                        IdUsuario = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.OrdenId)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUsuario)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(nullable: false, maxLength: 75),
                        UltimaConexion = c.DateTime(),
                        Activo = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Resenas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductoId = c.Int(nullable: false),
                        UsuarioId = c.String(nullable: false, maxLength: 128),
                        Titulo = c.String(maxLength: 100),
                        Cuerpo = c.String(nullable: false, maxLength: 1000),
                        Calificacion = c.Int(nullable: false),
                        Estado = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Productoes", t => t.ProductoId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UsuarioId)
                .Index(t => t.ProductoId)
                .Index(t => t.UsuarioId);
            
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
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Tarjetas",
                c => new
                    {
                        IdUsuario = c.String(nullable: false, maxLength: 128),
                        NumeroTarjeta = c.String(nullable: false),
                        FechaVencimiento = c.DateTime(nullable: false),
                        CCV = c.String(nullable: false),
                        Propietario = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.IdUsuario)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUsuario)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Ordens", "IdUsuario", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tarjetas", "IdUsuario", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Resenas", "UsuarioId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Resenas", "ProductoId", "dbo.Productoes");
            DropForeignKey("dbo.OrdenDetalles", "ProductoId", "dbo.Productoes");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrdenDetalles", "OrdenId", "dbo.Ordens");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Tarjetas", new[] { "IdUsuario" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Resenas", new[] { "UsuarioId" });
            DropIndex("dbo.Resenas", new[] { "ProductoId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Ordens", new[] { "IdUsuario" });
            DropIndex("dbo.OrdenDetalles", new[] { "ProductoId" });
            DropIndex("dbo.OrdenDetalles", new[] { "OrdenId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Tarjetas");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Productoes");
            DropTable("dbo.Resenas");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Ordens");
            DropTable("dbo.OrdenDetalles");
        }
    }
}
