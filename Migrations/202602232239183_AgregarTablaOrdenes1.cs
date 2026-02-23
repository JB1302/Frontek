namespace Frontek_Full_Web_E_Commerce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgregarTablaOrdenes1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrdenDetalles",
                c => new
                    {
                        OrdenDetalleId = c.Int(nullable: false, identity: true),
                        OrdenId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        NombreProducto = c.String(nullable: false, maxLength: 150),
                        SKU = c.String(maxLength: 50),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Subtotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Garantia = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.OrdenDetalleId)
                .ForeignKey("dbo.Ordens", t => t.OrdenId, cascadeDelete: true)
                .Index(t => t.OrdenId);
            
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
                    })
                .PrimaryKey(t => t.OrdenId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrdenDetalles", "OrdenId", "dbo.Ordens");
            DropIndex("dbo.OrdenDetalles", new[] { "OrdenId" });
            DropTable("dbo.Ordens");
            DropTable("dbo.OrdenDetalles");
        }
    }
}
