using Frontek_Full_Web_E_Commerce.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Frontek_Full_Web_E_Commerce.Domain.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaMod { get; set; }
        public byte[] Imagen1 { get; set; }
        public byte[] Imagen2 { get; set; }
        public byte[] Imagen3 { get; set; }
        public virtual ICollection<Resena> Resenas { get; set; }
        public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; }
    }
}