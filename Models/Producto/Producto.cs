using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Frontek_Full_Web_E_Commerce.Models
{
        public class Producto
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [StringLength(150)]
            [Display(Name = "Nombre del Producto")]
            public string NombreProducto { get; set; }
            public decimal Precio { get; set; }
            public int Stock { get; set; }
            public bool Activo { get; set; }

            [Display(Name = "Fecha de Ingreso")]
            public DateTime FechaIngreso { get; set; }

            [Display(Name = "Fecha de Modificación")]
            public DateTime? FechaMod { get; set; }

            public byte[] Imagen1 { get; set; }
            public byte[] Imagen2 { get; set; }
            public byte[] Imagen3 { get; set; }

            public virtual ICollection<Resena> Resenas { get; set; }
            public virtual ICollection<OrdenDetalle> OrdenDetallle { get; set; }
    }
    }