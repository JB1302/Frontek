using System;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Models
{
    public class ResenaViewModel
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; }
        public string NombreUsuario { get; set; }
        public string Titulo { get; set; }
        public string Cuerpo { get; set; }
        public int Calificacion { get; set; }
        public int Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}