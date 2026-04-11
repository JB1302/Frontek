using Frontek_Full_Web_E_Commerce.Domain.Utils;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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