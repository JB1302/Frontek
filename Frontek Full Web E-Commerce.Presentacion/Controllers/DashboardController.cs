using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Presentacion.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class DashboardController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly IOrdenService _ordenService;
        private readonly IResenaService _resenaService;

        public DashboardController(
            IProductoService productoService,
            IOrdenService ordenService,
            IResenaService resenaService)
        {
            _productoService = productoService;
            _ordenService = ordenService;
            _resenaService = resenaService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStats()
        {
            var productos = _productoService.ListarProductos().ToList();
            var ordenes = _ordenService.ListarOrdenes().ToList();
            var resenas = _resenaService.ListarResenas().ToList();

            int totalUsuarios;
            int usuariosActivos;

            using (var db = new ApplicationDbContext())
            {
                totalUsuarios = db.Users.Count();
                usuariosActivos = db.Users.Count(u => u.Activo);
            }

            var stats = new
            {
                totalUsuarios,
                usuariosActivos,
                totalProductos = productos.Count,
                productosBajoStock = productos.Count(p => p.Activo && p.Stock <= 5),
                totalOrdenes = ordenes.Count,
                ordenesEntregadas = ordenes.Count(o => o.Estado == "Entregada"),
                ordenesPendientes = ordenes.Count(o => o.Estado == "Pendiente"),
                ordenesEnCamino = ordenes.Count(o => o.Estado == "En camino"),
                ordenesProcesando = ordenes.Count(o => o.Estado == "Procesando"),
                ventasTotal = ordenes.Sum(o => o.Total),
                totalResenas = resenas.Count,
                resenasPendientes = resenas.Count(r => r.Estado == 0)
            };

            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUltimasOrdenes()
        {
            var ordenes = _ordenService.ListarOrdenes()
                .OrderByDescending(o => o.FechaCreacion)
                .Take(5)
                .Select(o => new
                {
                    numeroOrden = o.NumeroOrden,
                    nombreCliente = o.NombreCliente,
                    total = o.Total,
                    estado = o.Estado,
                    fechaCreacion = o.FechaCreacion.ToString("s")
                })
                .ToList();

            return Json(ordenes, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStockBajo()
        {
            var productos = _productoService.ListarProductos()
                .Where(p => p.Activo && p.Stock <= 5)
                .OrderBy(p => p.Stock)
                .Take(10)
                .Select(p => new
                {
                    nombreProducto = p.NombreProducto,
                    stock = p.Stock,
                    precio = p.Precio
                })
                .ToList();

            return Json(productos, JsonRequestBehavior.AllowGet);
        }
    }
}
