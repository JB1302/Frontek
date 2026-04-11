using Frontek_Full_Web_E_Commerce.Domain.Entities;
using System.Collections.Generic;

namespace Frontek_Full_Web_E_Commerce.Domain.Repositories
{
    public interface IResenaRepository
    {
        void Add(Resena resena);
        void Delete(Resena resena);
        Resena GetById(int id);
        IEnumerable<Resena> GetAll();
        IEnumerable<Resena> GetByUserId(string userId);
        IEnumerable<Resena> GetByProductId(int productoId);
        void Moderate(int id, int accion);
        void Save();
    }
}