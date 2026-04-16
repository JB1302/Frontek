using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Models
{
    public class TarjetaCreateViewModel
    {
        [Required]
        public string NumeroTarjeta { get; set; }

        [Required]
        public string CCV { get; set; }

        [Required]
        public DateTime? FechaVencimiento { get; set; }

        [Required]
        public string Propietario { get; set; }
    }
}