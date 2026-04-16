using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Models
{
    public class TarjetaDetailsViewModel
    {
        public string Propietario { get; set; }
        public string NumeroMascarado { get; set; }
        public DateTime? FechaVencimiento { get; set; }
    }
}