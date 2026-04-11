using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace Frontek_Full_Web_E_Commerce.Application.Services
{
    public class ResenaService : IResenaService
    {
        private readonly IResenaRepository _resenaRepository;

        public ResenaService(IResenaRepository resenaRepository)
        {
            _resenaRepository = resenaRepository;
        }

        public bool PuedenOpinar(int productoId, string userId)
        {
            var resenas = _resenaRepository.GetByUserId(userId);

            foreach (var r in resenas)
            {
                if (r.ProductoId == productoId)
                    return false;
            }

            return true;
        }

        public void CrearResena(Resena resena)
        {
            if (!PuedenOpinar(resena.ProductoId, resena.UsuarioId))
                throw new InvalidOperationException("Ya existe una resena de este usuario para ese producto");
            resena.Estado = 0;
            resena.FechaCreacion = DateTime.Now;
            _resenaRepository.Add(resena);
            _resenaRepository.Save();
        }

        public void ModerarResena(int id, int accion)
        {
            if (accion != 1 && accion != 2)
                throw new ArgumentException("La accion no es valida, utilice 1: Aprobar o 2:Rechazar");

            var resena = _resenaRepository.GetById(id);

            if (resena == null)
                throw new InvalidOperationException("La resena no existe");

            _resenaRepository.Moderate(id, accion);
            _resenaRepository.Save();
        }

        public void EliminarResena(int id)
        {
            var resena = _resenaRepository.GetById(id);

            if (resena == null)
                throw new InvalidOperationException("La resena no existe");

            _resenaRepository.Delete(resena);
            _resenaRepository.Save();
        }

        public IEnumerable<Resena> ListarResenas()
        {
            return _resenaRepository.GetAll();
        }

        public IEnumerable<Resena> ListarResenasUsuario(string userId)
        {
            return _resenaRepository.GetByUserId(userId);
        }

        public IEnumerable<Resena> ListarResenasPorProducto(int productoId)
        {
            return _resenaRepository.GetByProductId(productoId);
        }
    }
}