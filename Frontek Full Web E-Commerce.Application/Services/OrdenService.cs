using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace Frontek_Full_Web_E_Commerce.Application.Services
{
    public class OrdenService : IOrdenService
    {
        private readonly IOrdenRepository _ordenRepository;

        public OrdenService(IOrdenRepository ordenRepository)
        {
            _ordenRepository = ordenRepository;
        }

        public void CrearOrden(Orden orden)
        {
            if (orden == null)
                throw new ArgumentNullException(nameof(orden));

            orden.FechaCreacion = DateTime.Now;
            orden.NumeroOrden = "FRK-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            _ordenRepository.Add(orden);
            _ordenRepository.Save();
        }

        public void ActualizarEstado(int ordenId, string nuevoEstado)
        {
            var orden = _ordenRepository.GetById(ordenId);

            if (orden == null)
                throw new InvalidOperationException("La orden no existe");

            orden.Estado = nuevoEstado;
            _ordenRepository.Update(orden);
            _ordenRepository.Save();
        }

        public void EliminarOrden(int id)
        {
            var orden = _ordenRepository.GetById(id);

            if (orden == null)
                throw new InvalidOperationException("La orden no existe");

            _ordenRepository.Delete(orden);
            _ordenRepository.Save();
        }

        public IEnumerable<Orden> ListarOrdenes()
        {
            return _ordenRepository.GetAll();
        }

        public IEnumerable<Orden> ListarOrdenesPorUsuario(string userId)
        {
            return _ordenRepository.GetByUserId(userId);
        }

        public Orden ObtenerPorId(int id)
        {
            return _ordenRepository.GetById(id);
        }
    }
}