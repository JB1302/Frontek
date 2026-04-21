using Newtonsoft.Json;
using System;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Models
{
    public class AnuncioViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public DateTime FechaPublicacion { get; set; }

        [JsonProperty("FechaExperiacion")]
        public DateTime FechaExpiracion { get; set; }

        public bool Activo { get; set; }

        [JsonProperty("categoria")]
        public string Categoria { get; set; }

        [JsonProperty("imagenUrl")]
        public string ImagenUrl { get; set; }
    }
}