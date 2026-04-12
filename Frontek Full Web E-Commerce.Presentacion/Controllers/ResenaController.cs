using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Controllers
{
    [Authorize]
    public class ResenaController : Controller
    {
        private readonly IResenaService _resenaService;
        private readonly IProductoService _productoService;

        public ResenaController(IResenaService resenaService, IProductoService productoService)
        {
            _resenaService = resenaService;
            _productoService = productoService;
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var resenas = User.IsInRole("Administrador")
                ? _resenaService.ListarResenas()
                : _resenaService.ListarResenasUsuario(userId);

            var lista = resenas.OrderByDescending(r => r.FechaCreacion).ToList();

            ViewBag.TotalPendientes = lista.Count(r => r.Estado == 0);
            ViewBag.TotalAprobadas = lista.Count(r => r.Estado == 1);
            ViewBag.TotalRechazadas = lista.Count(r => r.Estado == 2);

            if (TempData["Mensaje"] != null)
            {
                ViewBag.Mensaje = TempData["Mensaje"];
            }

            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(lista);
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult Create(int? productoId)
        {
            if (!productoId.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = _productoService.ObtenerPorId(productoId.Value);
            if (producto == null)
            {
                return HttpNotFound();
            }

            ViewBag.Producto = producto;
            ViewBag.ProductoId = productoId.Value;

            return View(new Resena { ProductoId = productoId.Value });
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Resena resena)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Producto = _productoService.ObtenerPorId(resena.ProductoId);
                ViewBag.ProductoId = resena.ProductoId;
                return View(resena);
            }

            var producto = _productoService.ObtenerPorId(resena.ProductoId);
            if (producto == null)
            {
                return HttpNotFound();
            }

            resena.UsuarioId = User.Identity.GetUserId();

            try
            {
                _resenaService.CrearResena(resena);
                TempData["Mensaje"] = "Reseña enviada y pendiente de moderación.";
                return RedirectToAction("Index", "Producto");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Producto = producto;
                ViewBag.ProductoId = resena.ProductoId;
                return View(resena);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Moderar(int? id, int accion)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                _resenaService.ModerarResena(id.Value, accion);
                TempData["Mensaje"] = "Reseña moderada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                _resenaService.EliminarResena(id.Value);
                TempData["Mensaje"] = "Reseña eliminada correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}