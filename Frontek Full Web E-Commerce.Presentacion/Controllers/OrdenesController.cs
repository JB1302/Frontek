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
    public class OrdenesController : Controller
    {
        private readonly IOrdenService _ordenService;
        private readonly Frontek_Full_Web_E_Commerce.Infrastructure.Data.ApplicationDbContext _db;

        public OrdenesController(
            IOrdenService ordenService,
            Frontek_Full_Web_E_Commerce.Infrastructure.Data.ApplicationDbContext db)
        {
            _ordenService = ordenService;
            _db = db;
        }

        private SelectList GetUsuariosSelectList(string selectedId = null)
        {
            var usuarios = _db.Users
                .Select(u => new { u.Id, u.UserName })
                .ToList();

            return new SelectList(usuarios, "Id", "UserName", selectedId);
        }

        [Authorize(Roles = "Administrador,Vendedor")]
        public ActionResult Index()
        {
            var ordenes = _ordenService.ListarOrdenes()
                .OrderByDescending(o => o.FechaCreacion)
                .ToList();

            return View(ordenes);
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult MisPedidos()
        {
            var userId = User.Identity.GetUserId();
            var ordenes = _ordenService.ListarOrdenesPorUsuario(userId)
                .OrderByDescending(o => o.FechaCreacion)
                .ToList();

            return View("Index", ordenes);
        }

        [Authorize(Roles = "Administrador,Vendedor,Cliente")]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var orden = _ordenService.ObtenerPorId(id.Value);
            if (orden == null)
                return HttpNotFound();

            if (User.IsInRole("Cliente") && orden.IdUsuario != User.Identity.GetUserId())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(orden);
        }

        [Authorize(Roles = "Administrador,Vendedor")]
        public ActionResult Create()
        {
            ViewBag.IdUsuario = GetUsuariosSelectList();
            return View(new Orden { FechaEntregaEstimada = DateTime.Now.AddDays(5) });
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Vendedor")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orden orden)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IdUsuario = GetUsuariosSelectList(orden.IdUsuario);
                return View(orden);
            }

            try
            {
                _ordenService.CrearOrden(orden);
                TempData["Mensaje"] = "Orden creada correctamente.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.IdUsuario = GetUsuariosSelectList(orden.IdUsuario);
                return View(orden);
            }
        }

        [Authorize(Roles = "Administrador,Vendedor")]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var orden = _ordenService.ObtenerPorId(id.Value);
            if (orden == null)
                return HttpNotFound();

            ViewBag.IdUsuario = GetUsuariosSelectList(orden.IdUsuario);
            return View(orden);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Vendedor")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Orden orden)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IdUsuario = GetUsuariosSelectList(orden.IdUsuario);
                return View(orden);
            }

            try
            {
                _ordenService.EditarOrden(orden);
                TempData["Mensaje"] = "Orden actualizada correctamente.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.IdUsuario = GetUsuariosSelectList(orden.IdUsuario);
                return View(orden);
            }
        }

        [Authorize(Roles = "Administrador,Vendedor")]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var orden = _ordenService.ObtenerPorId(id.Value);
            if (orden == null)
                return HttpNotFound();

            return View(orden);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador,Vendedor")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _ordenService.EliminarOrden(id);
                TempData["Mensaje"] = "Orden eliminada correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Mensaje"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}