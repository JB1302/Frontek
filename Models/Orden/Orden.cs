using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Frontek_Full_Web_E_Commerce.Models
{
    public class Orden
    {
        [Key]
        public int OrdenId { get; set; }
        public string NumeroOrden { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string EmailCliente { get; set; }
        public string DireccionEnvio { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public DateTime? FechaEntregaEstimada { get; set; }
        public virtual ICollection<OrdenDetalle> Detalles { get; set; }

        public int IdUsuario { get; set; } = 1;

        public virtual Usuario Usuario { get; set; }
    }
}