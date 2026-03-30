using Frontek.Models;
using Frontek_Full_Web_E_Commerce.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Controllers
{
    public class OrdenesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ordenes
        [Authorize(Roles = "Administrador,Vendedor")]
        public async Task<ActionResult> Index()
        {
            var ordenes = await db.Ordenes
                .Include(o => o.Usuario)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();

            return View(ordenes);
        }

        // GET: Ordenes/Details/5
        [Authorize(Roles = "Administrador,Vendedor,Cliente")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var orden = await db.Ordenes
                .Include(o => o.Usuario)
                .Include(o => o.Detalles.Select(d => d.Producto))
                .FirstOrDefaultAsync(o => o.OrdenId == id);

            if (orden == null)
                return HttpNotFound();

            if (User.IsInRole("Cliente"))
            {
                var userId = User.Identity.GetUserId();

                if (orden.IdUsuario != userId)
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(orden);
        }

        // GET: Ordenes/Create
        [Authorize(Roles = "Administrador,Vendedor")]
        public async Task<ActionResult> Create()
        {
            ViewBag.IdUsuario = new SelectList(
                await db.Users
                    .OrderBy(u => u.Nombre)
                    .ToListAsync(),
                "Id",
                "Nombre"
            );

            return View();
        }

        // POST: Ordenes/Create
        [Authorize(Roles = "Administrador,Vendedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "NumeroOrden,FechaCreacion,ClienteId,NombreCliente,EmailCliente,DireccionEnvio,Ciudad,Pais,Total,MetodoPago,Estado,FechaEntregaEstimada,IdUsuario")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(orden.NumeroOrden))
                    orden.NumeroOrden = "FRK-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                if (orden.FechaCreacion == default(DateTime))
                    orden.FechaCreacion = DateTime.Now;

                db.Ordenes.Add(orden);
                await db.SaveChangesAsync();

                TempData["Mensaje"] = "Orden creada correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(
                await db.Users.OrderBy(u => u.Nombre).ToListAsync(),
                "Id",
                "Nombre",
                orden.IdUsuario
            );

            return View(orden);
        }

        // GET: Ordenes/Edit/5
        [Authorize(Roles = "Administrador,Vendedor")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var orden = await db.Ordenes.FindAsync(id);

            if (orden == null)
                return HttpNotFound();

            ViewBag.IdUsuario = new SelectList(
                await db.Users.OrderBy(u => u.Nombre).ToListAsync(),
                "Id",
                "Nombre",
                orden.IdUsuario
            );

            return View(orden);
        }

        // POST: Ordenes/Edit/5
        [Authorize(Roles = "Administrador,Vendedor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OrdenId,NumeroOrden,FechaCreacion,ClienteId,NombreCliente,EmailCliente,DireccionEnvio,Ciudad,Pais,Total,MetodoPago,Estado,FechaEntregaEstimada,IdUsuario")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orden).State = EntityState.Modified;
                await db.SaveChangesAsync();

                TempData["Mensaje"] = "Orden actualizada correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(
                await db.Users.OrderBy(u => u.Nombre).ToListAsync(),
                "Id",
                "Nombre",
                orden.IdUsuario
            );

            return View(orden);
        }

        // GET: Ordenes/Delete/5
        [Authorize(Roles = "Administrador,Vendedor")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var orden = await db.Ordenes
                .Include(o => o.Usuario)
                .FirstOrDefaultAsync(o => o.OrdenId == id);

            if (orden == null)
                return HttpNotFound();

            return View(orden);
        }

        // POST: Ordenes/Delete/5
        [Authorize(Roles = "Administrador,Vendedor")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var orden = await db.Ordenes
                .Include(o => o.Detalles)
                .FirstOrDefaultAsync(o => o.OrdenId == id);

            if (orden == null)
                return HttpNotFound();

            if (orden.Detalles?.Any() == true)
            {
                db.OrdenDetalles.RemoveRange(orden.Detalles);
            }

            db.Ordenes.Remove(orden);
            await db.SaveChangesAsync();

            TempData["Mensaje"] = "Orden eliminada correctamente.";
            return RedirectToAction("Index");
        }
        // GET: Ordenes/MisPedidos
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult> MisPedidos()
        {
            var userId = User.Identity.GetUserId();

            var ordenes = await db.Ordenes
                .Where(o => o.IdUsuario == userId)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();

            return View("Index", ordenes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}