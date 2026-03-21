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
    [Authorize]
    public class ResenaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        //este ajax get traeria las resenas para mostrar una tabla en este caso que solo ej
        // admin y vendedor pueden ver todas las resenas y el cliente solo las suyas
        public async Task<ActionResult> GetResenas()
        {
            IQueryable<Resena> query = db.Resenas
                .Include(r => r.Producto)
                .Include(r => r.Usuario);

            if (User.IsInRole("Cliente"))
            {
                var userId = User.Identity.GetUserId();
                query = query.Where(r => r.UsuarioId == userId);
            }

            var resenas = await query
                .OrderByDescending(r => r.FechaCreacion)
                .Select(r => new
                {
                    id = r.Id,
                    producto = r.Producto.NombreProducto,
                    usuario = r.Usuario.Nombre,
                    titulo = r.Titulo,
                    cuerpo = r.Cuerpo,
                    calificacion = r.Calificacion,
                    estado = r.Estado,
                    estadoTexto = r.Estado == 0 ? "Pendiente"
                                  : r.Estado == 1 ? "Aprobada"
                                  : "Rechazada",
                    fechaCreacion = r.FechaCreacion.ToString()
                })
                .ToListAsync();

            return Json(resenas, JsonRequestBehavior.AllowGet);
        }
        //esta accion es para que el admin pueda aprobar o rechazar una resena
        // esto depende de lo que se le envie a ajax
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Moderar(int id, int accion)
        {
            if (accion != 1 && accion != 2)
                return Json(new { ok = false, mensaje = "La accion no es valida" });

            var resena = await db.Resenas.FindAsync(id);
            if (resena == null)
                return Json(new { ok = false, mensaje = "No se encontro la resena" });
            resena.Estado = accion; //aqui cambia la accion dependendo si es aprobada o rechazada
            await db.SaveChangesAsync();

            string estadoTexto = accion == 1 ? "Aprobada" : "Rechazada";

            return Json(new
            {
                ok = true,
                mensaje = "Resena " + estadoTexto.ToLower() + " correctamente",
                estadoTexto = estadoTexto
            });
        }

      //para eliminar resena 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var resena = await db.Resenas.FindAsync(id);
            if (resena == null)
                return Json(new { ok = false, mensaje = "Resena no encontrada" });

            db.Resenas.Remove(resena);
            await db.SaveChangesAsync();

            return Json(new { ok = true, mensaje = "Resena eliminada" });
        }

       
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<ActionResult> Create(
            [Bind(Include = "ProductoId,Titulo,Cuerpo,Calificacion")] Resena resena)
        {
            if (ModelState.IsValid)
            {
                resena.UsuarioId = User.Identity.GetUserId();
                resena.Estado = 0; 
                resena.FechaCreacion = DateTime.Now;

                db.Resenas.Add(resena);
                await db.SaveChangesAsync();

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