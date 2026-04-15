namespace Frontek_Full_Web_E_Commerce.Domain.Entities
{
    public class CarritoItem
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public byte[] Imagen { get; set; }

        public CarritoItem(int productoId, int cantidad)
        {
            ProductoId = productoId;
            Cantidad = cantidad;
        }

        public CarritoItem()
        {
        }

        public void AumentarCantidad(int cantidad)
        {
            Cantidad += cantidad;
        }
    }
}