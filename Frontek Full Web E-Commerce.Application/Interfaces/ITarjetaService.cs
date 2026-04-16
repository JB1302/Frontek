using Frontek_Full_Web_E_Commerce.Domain.Entities;

namespace Frontek_Full_Web_E_Commerce.Application.Interfaces
{
    public interface ITarjetaService
    {
        Tarjeta ObtenerTarjeta(string userId);
        bool TieneTarjeta(string userId);
        void AgregarTarjeta(Tarjeta tarjeta, string userId);
        void EliminarTarjeta(string userId);


    }
}