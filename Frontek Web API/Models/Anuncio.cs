using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontek_Web_API.Models
{
    public class Anuncio
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;
        public DateTime FechaExperiacion { get; set; } = DateTime.Now.AddDays(30);
        public bool Activo { get; set; } = true;
        public string categoria { get; set; } = string.Empty;
        public string imagenUrl { get; set; } = string.Empty;
    }
}