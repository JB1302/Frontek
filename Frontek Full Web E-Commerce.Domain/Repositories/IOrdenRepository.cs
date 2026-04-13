using Frontek_Full_Web_E_Commerce.Domain.Entities;
using System.Collections.Generic;

namespace Frontek_Full_Web_E_Commerce.Domain.Repositories
{
    public interface IOrdenRepository
    {
        void Add(Orden orden);
        void Delete(Orden orden);
        Orden GetById(int id);
        IEnumerable<Orden> GetAll();
        IEnumerable<Orden> GetByUserId(string userId);
        void Save();
        void Update(Orden orden);
    }
}