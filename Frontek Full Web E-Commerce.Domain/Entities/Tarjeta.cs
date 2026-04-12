using System;

namespace Frontek_Full_Web_E_Commerce.Domain.Entities
{
    public class Tarjeta
{
    public string IdUsuario { get; set; }
    public string TarjetaEncriptada { get; set; }
    public string CCVEncriptado { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string Propietario { get; set; }
}
}