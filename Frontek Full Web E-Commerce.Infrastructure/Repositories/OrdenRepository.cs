using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Domain.Repositories;
using Frontek_Full_Web_E_Commerce.Infrastructure.Data;
using System;
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
                .SingleOrDefault(o => o.OrdenId == id);
        }

        public IEnumerable<Orden> GetAll()
        {
            return _context.Ordenes
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
            try
            {
                _context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errores = string.Join("\n", ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));

                throw new InvalidOperationException("Errores de validacion:\n" + errores);
            }
        }

        public void Update(Orden ordenEditada)
        {
            var local = _context.Set<Orden>().Local
                                .FirstOrDefault(o => o.OrdenId == ordenEditada.OrdenId);

            if (local != null)
                _context.Entry(local).State = EntityState.Detached;

            _context.Entry(ordenEditada).State = EntityState.Modified;
        }
    }
}