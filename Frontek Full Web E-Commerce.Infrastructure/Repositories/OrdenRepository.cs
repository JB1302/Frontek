using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Domain.Repositories;
using Frontek_Full_Web_E_Commerce.Infrastructure.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Frontek_Full_Web_E_Commerce.Infrastructure.Repositories
{
    public class OrdenRepository : IOrdenRepository
    {
        private readonly ApplicationDbContext _context;

        public OrdenRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Orden orden)
        {
            _context.Ordenes.Add(orden);
        }
        public void Delete(Orden orden)
        {
            _context.Ordenes.Remove(orden);
        }
        public Orden GetById(int id)
        {
            return _context.Ordenes
                .Include(o => o.Detalles)
                .Include(o => o.NombreCliente)
                .SingleOrDefault(o => o.OrdenId == id);
        }
        public IEnumerable<Orden> GetAll()
        {
            return _context.Ordenes
                .Include(o => o.NombreCliente)
                .OrderByDescending(o => o.FechaCreacion)
                .ToList();
        }
        public IEnumerable<Orden> GetByUserId(string userId)
        {
            return _context.Ordenes
                .Where(o => o.IdUsuario == userId)
                .OrderByDescending(o => o.FechaCreacion)
                .ToList();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Update(Orden orden)
        {
            _context.Entry(orden).State = EntityState.Modified;
        }
    }
}