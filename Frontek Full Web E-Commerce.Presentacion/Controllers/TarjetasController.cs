using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Frontek_Full_Web_E_Commerce.Presentacion.Models;
using Microsoft.Ajax.Utilities;
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
        private readonly ICryptoService _cryptoService;

        public TarjetasController(ITarjetaService tarjetaService, ICryptoService cryptoService)
        {
            _tarjetaService = tarjetaService;
            _cryptoService = cryptoService;
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var tarjeta = _tarjetaService.ObtenerTarjeta(userId);

            if (tarjeta == null)
            {
                return RedirectToAction("Create");
            }

            return View(tarjeta);
        }

        public ActionResult Details()
        {
            var userId = User.Identity.GetUserId();
            var tarjeta = _tarjetaService.ObtenerTarjeta(userId);

            if (tarjeta == null)
                return HttpNotFound();

            /*
             Bug extraño que me topé. NI IDEA en que parte del flujo se encripta dos veces tarjeta
             entonces mi solución fue usar Decrypt dos veces
            También reescribí todo el código fuente del cryptohelper
             */
            //Debugging
            /*
            var X = tarjeta.TarjetaEncriptada;
            */

            var numero = _cryptoService.Decrypt(tarjeta.TarjetaEncriptada);
            numero = _cryptoService.Decrypt(numero);

            var model = new TarjetaDetailsViewModel
            {
                Propietario = tarjeta.Propietario,
                FechaVencimiento = tarjeta.FechaVencimiento,
                NumeroMascarado = string.IsNullOrEmpty(numero) || numero.Length < 4
                    ? "No disponible"
                    : "Mi tarjeta acaba en: " + numero.Substring(numero.Length - 4)
            };

            return View(model);
        }

        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();

            if (_tarjetaService.TieneTarjeta(userId))
                return RedirectToAction("Details");

            return View(new TarjetaCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TarjetaCreateViewModel model)
        {
            var userId = User.Identity.GetUserId();

            if (_tarjetaService.TieneTarjeta(userId))
                return RedirectToAction("Index", "Manage");

            if (!ModelState.IsValid)
                return View(model);

            if (model.FechaVencimiento <= DateTime.Today)
            {
                ModelState.AddModelError("FechaVencimiento", "La tarjeta está vencida.");
                return View(model);
            }


            var tarjeta = new Tarjeta
            {
                TarjetaEncriptada = _cryptoService.Encrypt(model.NumeroTarjeta),
                CCVEncriptado = _cryptoService.Encrypt(model.CCV),
                FechaVencimiento = model.FechaVencimiento,
                Propietario = model.Propietario,
                IdUsuario = userId,

            };
            
            _tarjetaService.AgregarTarjeta(tarjeta, userId);

            return RedirectToAction("Index", "Manage");
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