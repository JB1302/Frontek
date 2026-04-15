using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace Frontek_Full_Web_E_Commerce.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public bool NombreDisponible(string nombre, int? idExcluir = null)
        {
            return !_productoRepository.ExistsName(nombre, idExcluir);
        }

        public void CrearProducto(Producto producto)
        {
            if (_productoRepository.ExistsName(producto.NombreProducto))
                throw new InvalidOperationException("Ya existe un producto con ese nombre");

            producto.FechaIngreso = DateTime.Now;
            _productoRepository.Add(producto);
            _productoRepository.Save();
        }

        public void EditarProducto(Producto producto)
        {
            if (_productoRepository.ExistsName(producto.NombreProducto, producto.Id))
                throw new InvalidOperationException("Ya existe un producto con ese nombre");

            producto.FechaMod = DateTime.Now;
            _productoRepository.Update(producto);
            _productoRepository.Save();
        }

        public void EliminarProducto(int id)
        {
            var producto = _productoRepository.GetById(id);

            if (producto == null)
                throw new InvalidOperationException("El producto no existe");

            _productoRepository.Delete(producto);
            _productoRepository.Save();
        }

        public IEnumerable<Producto> ListarProductos()
        {
            return _productoRepository.GetAll();
        }

        public IEnumerable<Producto> ListarProductosActivos()
        {
            return _productoRepository.GetActive();
        }

        public Producto ObtenerPorId(int id)
        {
            return _productoRepository.GetById(id);
        }
    }
}