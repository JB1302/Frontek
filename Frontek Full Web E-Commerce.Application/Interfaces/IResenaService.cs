using Frontek_Full_Web_E_Commerce.Domain.Entities;
using System.Collections.Generic;

namespace Frontek_Full_Web_E_Commerce.Application.Interfaces
{
    public interface IResenaService
    {
        bool PuedenOpinar(int productoId, string userId);
        void CrearResena(Resena resena);
        void ModerarResena(int id, int accion);
        void EliminarResena(int id);


        IEnumerable<Resena> ListarResenas();
        IEnumerable<Resena> ListarResenasUsuario(string userId);
        IEnumerable<Resena> ListarResenasPorProducto(int productoId);
    }
}