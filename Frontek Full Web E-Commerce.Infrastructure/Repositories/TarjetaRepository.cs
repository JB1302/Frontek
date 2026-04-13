using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Domain.Repositories;
using Frontek_Full_Web_E_Commerce.Infrastructure.Data;
using System.Linq;

namespace Frontek_Full_Web_E_Commerce.Infrastructure.Repositories
{
    public class TarjetaRepository : ITarjetaRepository
    {
        private readonly ApplicationDbContext _context;

        public TarjetaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Tarjeta GetByUsuarioId(string userId)
        {
            return _context.Tarjetas.Find(userId);
        }

        public bool ExistePorUsuarioId(string userId)
        {
            return _context.Tarjetas.Any(t => t.IdUsuario == userId);
        }

        public void Add(Tarjeta tarjeta)
        {
            _context.Tarjetas.Add(tarjeta);
        }

        public void Delete(Tarjeta tarjeta)
        {
            _context.Tarjetas.Remove(tarjeta);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}