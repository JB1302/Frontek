using Frontek_Full_Web_E_Commerce.Presentacion.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Controllers
{
    [Authorize]
    public class AnunciosController : Controller
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task<ActionResult> Index()
        {
            var anuncios = await ObtenerAnunciosDesdeApiAsync();
            var hoy = DateTime.Today;

            var anunciosActivos = anuncios
                .Where(a => a.Activo && a.FechaPublicacion.Date <= hoy && a.FechaExpiracion.Date >= hoy)
                .OrderByDescending(a => a.FechaPublicacion)
                .ToList();

            return View(anunciosActivos);
        }

        private async Task<List<AnuncioViewModel>> ObtenerAnunciosDesdeApiAsync()
        {
            var baseUrl = ConfigurationManager.AppSettings["AnunciosApiBaseUrl"];

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                baseUrl = "https://localhost:44372/";
            }

            try
            {
                var apiBaseUrl = NormalizarApiBaseUrl(baseUrl);
                var requestUrl = new Uri(apiBaseUrl, "api/anuncios");
                var response = await HttpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorAnuncios = $"No fue posible cargar anuncios del API. Código: {(int)response.StatusCode}.";
                    return new List<AnuncioViewModel>();
                }

                var json = await response.Content.ReadAsStringAsync();
                var anuncios = JsonConvert.DeserializeObject<List<AnuncioViewModel>>(json);

                return anuncios ?? new List<AnuncioViewModel>();
            }
            catch (Exception)
            {
                ViewBag.ErrorAnuncios = "Error de conexión con el API de anuncios.";
                return new List<AnuncioViewModel>();
            }
        }

        private static Uri NormalizarApiBaseUrl(string baseUrl)
        {
            var parsed = new Uri(baseUrl, UriKind.Absolute);
            var root = parsed.GetLeftPart(UriPartial.Authority);
            return new Uri(root.EndsWith("/") ? root : root + "/");
        }
    }
}
