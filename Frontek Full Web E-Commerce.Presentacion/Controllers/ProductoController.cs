using System.Web.Mvc;
using Frontek_Full_Web_E_Commerce.Application.Interfaces;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public ActionResult Index()
        {
            var productos = _productoService.ListarProductos();
            return View(productos);
        }
    }
}