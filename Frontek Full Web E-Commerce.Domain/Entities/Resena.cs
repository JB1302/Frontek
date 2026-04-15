using System;

namespace Frontek_Full_Web_E_Commerce.Domain.Entities
{
    public class Resena
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string UsuarioId { get; set; }
        public string Titulo { get; set; }
        public string Cuerpo { get; set; }
        public int Calificacion { get; set; }
        public int Estado { get; set; } = 0;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public virtual Producto Producto { get; set; }
    }
}