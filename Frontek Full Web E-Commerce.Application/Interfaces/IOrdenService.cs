using Frontek_Full_Web_E_Commerce.Domain.Entities;
using System.Collections.Generic;

namespace Frontek_Full_Web_E_Commerce.Application.Interfaces
{
    public interface IOrdenService
    {
        void CrearOrden(Orden orden);
        void EditarOrden(Orden orden);
        void ActualizarEstado(int ordenId, string nuevoEstado);
        void EliminarOrden(int id);
        IEnumerable<Orden> ListarOrdenes();
        IEnumerable<Orden> ListarOrdenesPorUsuario(string userId);
        Orden ObtenerPorId(int id);
    }
}