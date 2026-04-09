using Frontek_Full_Web_E_Commerce.Domain.Entities;
using System.Collections.Generic;

namespace Frontek_Full_Web_E_Commerce.Domain.Repositories
{
    public interface IResenaRepository
    {
        Resena GetById(int id);
        IEnumerable<Resena> GetAll();
        IEnumerable<Resena> GetByUsuarioId(string userId);
        IEnumerable<Resena> GetByProductoId(int productoId);
        void Add(Resena resena);
        void Moderar(int id, int accion);
        void Delete(Resena resena);
        void Save();
    }
}
