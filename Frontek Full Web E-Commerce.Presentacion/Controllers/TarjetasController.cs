using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Controllers
{
    [Authorize]
    public class TarjetasController : Controller
    {
        private readonly ITarjetaService _tarjetaService;

        public TarjetasController(ITarjetaService tarjetaService)
        {
            _tarjetaService = tarjetaService;
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var tarjeta = _tarjetaService.ObtenerTarjeta(userId);

            if (tarjeta == null)
            {
                return RedirectToAction("Create");
            }

            return View(new List<Tarjeta> { tarjeta });
        }

        public ActionResult Details()
        {
            var userId = User.Identity.GetUserId();
            var tarjeta = _tarjetaService.ObtenerTarjeta(userId);
            if (tarjeta == null)
            {
                return HttpNotFound();
            }

            return View(tarjeta);
        }

        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            if (_tarjetaService.TieneTarjeta(userId))
            {
                return RedirectToAction("Details");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tarjeta tarjeta)
        {
            if (!ModelState.IsValid)
            {
                return View(tarjeta);
            }

            var userId = User.Identity.GetUserId();

            try
            {
                _tarjetaService.AgregarTarjeta(tarjeta, userId);
                TempData["Mensaje"] = "Tarjeta agregada correctamente.";
                return RedirectToAction("Index", "Manage");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(tarjeta);
            }
        }

        public ActionResult Delete()
        {
            var userId = User.Identity.GetUserId();
            var tarjeta = _tarjetaService.ObtenerTarjeta(userId);
            if (tarjeta == null)
            {
                return HttpNotFound();
            }

            return View(tarjeta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            var userId = User.Identity.GetUserId();

            try
            {
                _tarjetaService.EliminarTarjeta(userId);
                TempData["Mensaje"] = "Tarjeta eliminada correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Mensaje"] = ex.Message;
            }

            return RedirectToAction("Index", "Manage");
        }
    }
}