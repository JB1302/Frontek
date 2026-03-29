using Frontek.Models;
using Frontek_Full_Web_E_Commerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Controllers
{
    [Authorize]
    public class ResenaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Resena/Index
        public ActionResult Index()
        {
            IQueryable<Resena> query = db.Resenas
                .Include(r => r.Producto)
                .Include(r => r.Usuario);

            // Los administradores ven todas las reseñas; los clientes solo las suyas
            if (User.IsInRole("Cliente") && !User.IsInRole("Administrador"))
            {
                var userId = User.Identity.GetUserId();
                query = query.Where(r => r.UsuarioId == userId);
            }

            var resenas = query
                .OrderByDescending(r => r.FechaCreacion)
                .ToList() ?? new List<Resena>();

            ViewBag.TotalPendientes = resenas.Count(r => r.Estado == 0);
            ViewBag.TotalAprobadas = resenas.Count(r => r.Estado == 1);
            ViewBag.TotalRechazadas = resenas.Count(r => r.Estado == 2);

            if (TempData["Mensaje"] != null)
                ViewBag.Mensaje = TempData["Mensaje"].ToString();

            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"].ToString();

            return View(resenas);
        }

        // POST: Resena/Moderar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Moderar(int id, int accion)
        {
            if (accion != 1 && accion != 2)
            {
                TempData["Error"] = "La accion no es valida";
                return RedirectToAction("Index");
            }

            var resena = db.Resenas.Find(id);
            if (resena == null)
            {
                TempData["Error"] = "No se encontro la resena";
                return RedirectToAction("Index");
            }

            resena.Estado = accion;
            db.SaveChanges();

            string estadoTexto = accion == 1 ? "aprobada" : "rechazada";
            TempData["Mensaje"] = "Resena " + estadoTexto + " correctamente";

            return RedirectToAction("Index");
        }

        // POST: Resena/Eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            var resena = db.Resenas.Find(id);
            if (resena == null)
            {
                TempData["Error"] = "Resena no encontrada";
                return RedirectToAction("Index");
            }

            db.Resenas.Remove(resena);
            db.SaveChanges();

            TempData["Mensaje"] = "Resena eliminada correctamente";
            return RedirectToAction("Index");
        }

        // GET: Resena/Create?productoId=X
        [Authorize(Roles = "Cliente")]
        public ActionResult Create(int? productoId)
        {
            if (productoId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var producto = db.Producto.Find(productoId);
            if (producto == null) return HttpNotFound();

            ViewBag.Producto = producto;
            ViewBag.ProductoId = productoId;
            return View();
        }

        // POST: Resena/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public ActionResult Create(
            [Bind(Include = "ProductoId,Titulo,Cuerpo,Calificacion")] Resena resena)
        {
            if (ModelState.IsValid)
            {
                resena.UsuarioId = User.Identity.GetUserId();
                resena.Estado = 0;
                resena.FechaCreacion = DateTime.Now;

                db.Resenas.Add(resena);
                db.SaveChanges();

                TempData["Mensaje"] = "Resena enviada y pendiente de ser evaluada";
                return RedirectToAction("Index", "Producto");
            }

            var prod = db.Producto.Find(resena.ProductoId);
            ViewBag.Producto = prod;
            ViewBag.ProductoId = resena.ProductoId;
            return View(resena);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
