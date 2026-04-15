using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Domain.Repositories;
using System;

namespace Frontek_Full_Web_E_Commerce.Application.Services
{
    public class TarjetaService : ITarjetaService
    {
        private readonly ITarjetaRepository _tarjetaRepository;

        public TarjetaService(ITarjetaRepository tarjetaRepository)
        {
            _tarjetaRepository = tarjetaRepository;
        }

        public Tarjeta ObtenerTarjeta(string userId)
        {
            return _tarjetaRepository.GetByUsuarioId(userId);
        }

        public bool TieneTarjeta(string userId)
        {
            return _tarjetaRepository.ExistePorUsuarioId(userId);
        }

        public void AgregarTarjeta(Tarjeta tarjeta, string userId)
        {
            if (_tarjetaRepository.ExistePorUsuarioId(userId))
                throw new InvalidOperationException(
                    "El usuario ya tiene una tarjeta registrada");

            tarjeta.IdUsuario = userId;

            _tarjetaRepository.Add(tarjeta);
            _tarjetaRepository.Save();
        }

        public void EliminarTarjeta(string userId)
        {
            var tarjeta = _tarjetaRepository.GetByUsuarioId(userId);

            if (tarjeta == null)
                throw new InvalidOperationException(
                    "No existe una tarjeta registrada para este usuario");

            _tarjetaRepository.Delete(tarjeta);
            _tarjetaRepository.Save();
        }
    }
}