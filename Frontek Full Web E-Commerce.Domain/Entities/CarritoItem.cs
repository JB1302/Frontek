namespace Frontek_Full_Web_E_Commerce.Models.Carrito
{
    public class CarritoItem
    {
        public int ProductoId { get; private set; }
        public int Cantidad { get; private set; }
        public CarritoItem(int productoId, int cantidad)
        {
            ProductoId = productoId;
            Cantidad = cantidad;
        }
        public void AumentarCantidad(int cantidad)
        {
            Cantidad += cantidad;
        }
    }
}