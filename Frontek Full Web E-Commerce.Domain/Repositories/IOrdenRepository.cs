using Frontek_Full_Web_E_Commerce.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Frontek_Full_Web_E_Commerce.Domain.Repositories
{
    public interface IOrdenRepository
    {
        Task<Orden> GetByIdAsync(int id);
        Task<Orden> GetByIdConDetallesAsync(int id);
        Task<IEnumerable<Orden>> GetAllAsync();
        Task<IEnumerable<Orden>> GetByUsuarioIdAsync(string userId);
        void Add(Orden orden);
        void Update(Orden orden);
        void Delete(Orden orden);
        void DeleteDetalles(IEnumerable<OrdenDetalle> detalles);
        Task SaveAsync();
    }
}

