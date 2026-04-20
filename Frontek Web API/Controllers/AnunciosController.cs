using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Frontek_Web_API.Models;
namespace Frontek_Web_API.Controllers
{
    [RoutePrefix("api/anuncios")]
    public class AnunciosController : ApiController
    {
        private static readonly List<Anuncio> anuncios = new List<Anuncio>
        {
            new Anuncio { Id = 1, Titulo = "Envío gratis en compras mayores a ₡50.000", Contenido = "Aprovecha nuestro envío gratis por tiempo limitado en compras superiores a ₡50.000.", FechaPublicacion = DateTime.Now.AddDays(-5), FechaExperiacion = DateTime.Now.AddDays(10), Activo = true, categoria = "Promoción", imagenUrl = "https://picsum.photos/seed/anuncio1/1200/600" },
            new Anuncio { Id = 2, Titulo = "Nueva colección deportiva disponible", Contenido = "Descubre nuestra nueva colección de ropa, calzado y accesorios deportivos.", FechaPublicacion = DateTime.Now.AddDays(-3), FechaExperiacion = DateTime.Now.AddDays(15), Activo = true, categoria = "Novedad", imagenUrl = "https://picsum.photos/seed/anuncio2/1200/600" },
            new Anuncio { Id = 3, Titulo = "Descuento del 25% en calzado seleccionado", Contenido = "Encuentra modelos de calzado con un 25% de descuento hasta agotar existencias.", FechaPublicacion = DateTime.Now.AddDays(-7), FechaExperiacion = DateTime.Now.AddDays(7), Activo = true, categoria = "Oferta", imagenUrl = "https://picsum.photos/seed/anuncio3/1200/600" },
            new Anuncio { Id = 4, Titulo = "Promoción 2x1 en accesorios", Contenido = "Lleva dos accesorios participantes por el precio de uno durante esta semana.", FechaPublicacion = DateTime.Now.AddDays(-2), FechaExperiacion = DateTime.Now.AddDays(8), Activo = true, categoria = "Promoción", imagenUrl = "https://picsum.photos/seed/anuncio4/1200/600" },
            new Anuncio { Id = 5, Titulo = "Compra en línea y recoge en tienda", Contenido = "Realiza tu pedido desde la web y retíralo fácilmente en nuestra tienda física.", FechaPublicacion = DateTime.Now.AddDays(-1), FechaExperiacion = DateTime.Now.AddDays(20), Activo = true, categoria = "Servicio", imagenUrl = "https://picsum.photos/seed/anuncio5/1200/600" }
        };

        // GET api/anuncios
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            return Ok(anuncios);
        }

        // GET api/anuncios/{id}
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var anuncio = anuncios.FirstOrDefault(a => a.Id == id);

            if (anuncio == null)
                return NotFound();

            return Ok(anuncio);
        }

        // POST api/anuncios
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] Anuncio nuevoAnuncio)
        {
            if (nuevoAnuncio == null)
                return BadRequest("El anuncio es requerido.");

            nuevoAnuncio.Id = anuncios.Any() ? anuncios.Max(a => a.Id) + 1 : 1;
            anuncios.Add(nuevoAnuncio);

            return Created($"api/anuncios/{nuevoAnuncio.Id}", nuevoAnuncio);
        }

        // PUT api/anuncios/{id}
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Put(int id, [FromBody] Anuncio anuncioActualizado)
        {
            if (anuncioActualizado == null)
                return BadRequest("El anuncio es requerido.");

            var anuncioExistente = anuncios.FirstOrDefault(a => a.Id == id);

            if (anuncioExistente == null)
                return NotFound();

            anuncioExistente.Titulo = anuncioActualizado.Titulo;
            anuncioExistente.Contenido = anuncioActualizado.Contenido;
            anuncioExistente.FechaPublicacion = anuncioActualizado.FechaPublicacion;
            anuncioExistente.FechaExperiacion = anuncioActualizado.FechaExperiacion;
            anuncioExistente.Activo = anuncioActualizado.Activo;
            anuncioExistente.categoria = anuncioActualizado.categoria;
            anuncioExistente.imagenUrl = anuncioActualizado.imagenUrl;

            return Ok(anuncioExistente);
        }

        // DELETE api/anuncios/{id}
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var anuncio = anuncios.FirstOrDefault(a => a.Id == id);

            if (anuncio == null)
                return NotFound();

            anuncios.Remove(anuncio);

            return Ok($"Anuncio con Id {id} eliminado correctamente.");
        }
    }
}