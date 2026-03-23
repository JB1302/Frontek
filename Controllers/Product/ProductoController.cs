using Frontek.Models;
using Frontek_Full_Web_E_Commerce.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Controllers.Product
{
    public class ProductoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Producto
        public ActionResult Index()
        {
            return View(db.Producto.ToList());
        }

        // GET: Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Producto producto = db.Producto.Find(id);
            if (producto == null)
                return HttpNotFound();

            return View(producto);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "NombreProducto,Precio,Stock,Activo,FechaIngreso,FechaMod")] Producto producto,
            HttpPostedFileBase ImagenFile1,
            HttpPostedFileBase ImagenFile2,
            HttpPostedFileBase ImagenFile3)
        {
            if (ModelState.IsValid)
            {
                producto.Imagen1 = LeerImagen(ImagenFile1);
                producto.Imagen2 = LeerImagen(ImagenFile2);
                producto.Imagen3 = LeerImagen(ImagenFile3);

                db.Producto.Add(producto);
                db.SaveChanges();
                TempData["Mensaje"] = "Producto creado correctamente.";
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Producto producto = db.Producto.Find(id);
            if (producto == null)
                return HttpNotFound();

            return View(producto);
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "Id,NombreProducto,Precio,Stock,Activo,FechaIngreso,FechaMod")] Producto producto,
            HttpPostedFileBase ImagenFile1,
            HttpPostedFileBase ImagenFile2,
            HttpPostedFileBase ImagenFile3)
        {
            if (ModelState.IsValid)
            {
                // Recuperar imágenes actuales si no se subió una nueva
                Producto productoActual = db.Producto.AsNoTracking().FirstOrDefault(p => p.Id == producto.Id);

                producto.Imagen1 = LeerImagen(ImagenFile1) ?? productoActual?.Imagen1;
                producto.Imagen2 = LeerImagen(ImagenFile2) ?? productoActual?.Imagen2;
                producto.Imagen3 = LeerImagen(ImagenFile3) ?? productoActual?.Imagen3;

                producto.FechaMod = DateTime.Now;

                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensaje"] = "Producto actualizado correctamente.";
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Producto producto = db.Producto.Find(id);
            if (producto == null)
                return HttpNotFound();

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);
            db.Producto.Remove(producto);
            db.SaveChanges();
            TempData["Mensaje"] = "Producto eliminado correctamente.";
            return RedirectToAction("Index");
        }

        // Vista pública del catálogo (solo productos activos)
        public ActionResult Catalogo()
        {
            return View("Index", db.Producto.Where(p => p.Activo).ToList());
        }

        // Panel admin
        public ActionResult Admin()
        {
            return View("IndexAdmin", db.Producto.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }

        // Método auxiliar para leer imagen desde archivo subido
        private byte[] LeerImagen(HttpPostedFileBase archivo)
        {
            if (archivo == null || archivo.ContentLength == 0)
                return null;

            using (var ms = new MemoryStream())
            {
                archivo.InputStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}