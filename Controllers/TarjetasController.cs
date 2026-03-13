using Frontek.Models;
using Frontek_Full_Web_E_Commerce.Models.Tarjeta;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Controllers
{
    public class TarjetasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tarjetas
        public ActionResult Index()
        {
            var tarjetas = db.Tarjetas.Include(t => t.Usuario);
            return View(tarjetas.ToList());
        }

        // GET: Tarjetas/Details/5
        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tarjeta tarjeta = db.Tarjetas.Find(id);
            if (tarjeta == null)
            {
                return HttpNotFound();
            }

            return View(tarjeta);
        }

        // GET: Tarjetas/Create
        public ActionResult Create()
        {
            ViewBag.IdUsuario = new SelectList(db.Users.ToList(), "Id", "Nombre");
            return View();
        }

        // POST: Tarjetas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUsuario,TarjetaEncriptada,FechaVencimiento,CCVEncriptado,Propietario")] Tarjeta tarjeta)
        {
            if (ModelState.IsValid)
            {
                db.Tarjetas.Add(tarjeta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.Users.ToList(), "Id", "Nombre", tarjeta.IdUsuario);
            return View(tarjeta);
        }

        // GET: Tarjetas/Edit/5
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tarjeta tarjeta = db.Tarjetas.Find(id);
            if (tarjeta == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdUsuario = new SelectList(db.Users.ToList(), "Id", "Nombre", tarjeta.IdUsuario);
            return View(tarjeta);
        }

        // POST: Tarjetas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUsuario,TarjetaEncriptada,FechaVencimiento,CCVEncriptado,Propietario")] Tarjeta tarjeta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarjeta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUsuario = new SelectList(db.Users.ToList(), "Id", "Nombre", tarjeta.IdUsuario);
            return View(tarjeta);
        }

        // GET: Tarjetas/Delete/5
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tarjeta tarjeta = db.Tarjetas.Find(id);
            if (tarjeta == null)
            {
                return HttpNotFound();
            }

            return View(tarjeta);
        }

        // POST: Tarjetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Tarjeta tarjeta = db.Tarjetas.Find(id);
            db.Tarjetas.Remove(tarjeta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}