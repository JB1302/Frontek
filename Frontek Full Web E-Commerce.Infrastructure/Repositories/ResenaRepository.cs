using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Domain.Repositories;
using Frontek_Full_Web_E_Commerce.Infrastructure.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Frontek_Full_Web_E_Commerce.Infrastructure.Repositories
{
    public class ResenaRepository : IResenaRepository
    {
        private readonly ApplicationDbContext _context;
        public ResenaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Resena resena)
        {
            _context.Resenas.Add(resena);
        }
        public void Delete(Resena resena)
        {
            _context.Resenas.Remove(resena);
        }

        public Resena GetById(int id)
        {
            return _context.Resenas
                .Include(r => r.Producto)
                .SingleOrDefault(r => r.Id == id);
        }
        public IEnumerable<Resena> GetAll()
        {
            return _context.Resenas
                .Include(r => r.Producto)
                .OrderByDescending(r => r.FechaCreacion)
                .ToList();
        }
        public IEnumerable<Resena> GetReviewsByUserId(string userId)
        {
            return _context.Resenas
                .Include(r => r.Producto)
                .Where(r => r.UsuarioId == userId)
                .OrderByDescending(r => r.FechaCreacion)
                .ToList();
        }
        public IEnumerable<Resena> GetByProductId(int productoId)
        {
            return _context.Resenas
                .Where(r => r.ProductoId == productoId)
                .OrderByDescending(r => r.FechaCreacion)
                .ToList();
        }
        public void Moderate(int id, int accion)
        {
            var resena = _context.Resenas.Find(id);
            if (resena != null)
                resena.Estado = accion;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}