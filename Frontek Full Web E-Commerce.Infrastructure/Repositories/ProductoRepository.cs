using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Domain.Repositories;
using Frontek_Full_Web_E_Commerce.Infrastructure.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Frontek_Full_Web_E_Commerce.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Producto producto)
        {
            _context.Productos.Add(producto);
        }
        public void Delete(Producto producto)
        {
            _context.Productos.Remove(producto);
        }
        public bool ExistsName(string nombre, int? idExcluir = null)
        {
            return _context.Productos.Any(p =>
                p.NombreProducto == nombre &&
                (!idExcluir.HasValue || p.Id != idExcluir.Value));
        }
        public Producto GetById(int id)
        {
            return _context.Productos
                .SingleOrDefault(p => p.Id == id);
        }
        public IEnumerable<Producto> GetAll()
        {
            return _context.Productos.ToList();
        }
        public IEnumerable<Producto> GetActive()
        {
            return _context.Productos
                .Where(p => p.Activo)
                .ToList();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public void Update(Producto producto)
        {
            _context.Entry(producto).State = EntityState.Modified;
        }
    }
}