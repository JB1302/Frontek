using Frontek_Full_Web_E_Commerce.Domain.Entities;
using System.Collections.Generic;

namespace Frontek_Full_Web_E_Commerce.Domain.Repositories
{
    public interface IProductoRepository
    {
        void Add(Producto producto);
        void Delete(Producto producto);
        bool ExistsName(string nombre, int? idExcluir = null);
        Producto GetById(int id);
        IEnumerable<Producto> GetAll();
        IEnumerable<Producto> GetActive();
        void Save();
        void Update(Producto producto);
    }
}