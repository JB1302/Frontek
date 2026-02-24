using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Frontek_Full_Web_E_Commerce.Models
{
    public class Resena
    {
        [Key]
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int UsuarioId { get; set; }
        [StringLength(100)]
        public string Titulo { get; set; }

        [Required, StringLength(1000)]
        public string Cuerpo { get; set; }

        [Range(1, 5)]
        public int Calificacion { get; set; }

        public int Estado { get; set; } = 0;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        //fks
        public virtual Producto Producto { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
