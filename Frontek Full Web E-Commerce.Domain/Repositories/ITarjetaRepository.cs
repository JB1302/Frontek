using Frontek_Full_Web_E_Commerce.Domain.Entities;

namespace Frontek_Full_Web_E_Commerce.Domain.Repositories
{
    public interface ITarjetaRepository
    {
        Tarjeta GetByUsuarioId(string userId);
        bool ExistePorUsuarioId(string userId);
        void Add(Tarjeta tarjeta);
        void Delete(Tarjeta tarjeta);
        void Save();
    }
}
