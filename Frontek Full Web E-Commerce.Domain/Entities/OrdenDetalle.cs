namespace Frontek_Full_Web_E_Commerce.Domain.Entities  
{
    public class OrdenDetalle
    {
        public int OrdenDetalleId { get; set; }

        public int OrdenId { get; set; }
        public virtual Orden Orden { get; set; }
        public string NombreProducto { get; set; }
        public string SKU { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public string Garantia { get; set; }
        public int ProductoId { get; set; } = 1;
        public virtual Producto Producto { get; set; }
    }
}