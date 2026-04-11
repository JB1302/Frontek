using Frontek_Full_Web_E_Commerce.Domain.Entities;
using System.Collections.Generic;

namespace Frontek_Full_Web_E_Commerce.Application.Interfaces
{
    public interface IProductoService
    {
        bool NombreDisponible(string nombre, int? idExcluir = null);
        void CrearProducto(Producto producto);
        void EditarProducto(Producto producto);
        void EliminarProducto(int id);
        IEnumerable<Producto> ListarProductos();
        IEnumerable<Producto> ListarProductosActivos();
        Producto ObtenerPorId(int id);
    }
}