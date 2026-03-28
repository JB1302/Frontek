using Frontek.Models;
using Frontek_Full_Web_E_Commerce.Models.Tarjeta;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Controllers
{
    [Authorize]
    public class TarjetasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tarjetas
        /*
        public ActionResult Index()
        {
            var tarjetas = db.Tarjetas.Include(t => t.Usuario);
            return View(tarjetas.ToList());
        }*/

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Manage");
        }

        // GET: Tarjetas/Details/5

        public ActionResult Details()
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var tarjeta = db.Tarjetas.Find(userId);

            if (tarjeta == null)
            {
                return RedirectToAction("Create");
            }

            return View(tarjeta);
        }

        // GET: Tarjetas/Create
        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();

            var tarjeta = db.Tarjetas.Find(userId);

            if (tarjeta != null)
            {
                return RedirectToAction("Details");
            }
            ViewBag.IdUsuario = new SelectList(db.Users.ToList(), "Id", "Nombre");
            return View();
        }

        // POST: Tarjetas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tarjeta tarjeta)
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            tarjeta.IdUsuario = userId;

            if (db.Tarjetas.Any(t => t.IdUsuario == userId))
            {
                ModelState.AddModelError("", "Ya tienes una tarjeta registrada.");
                return RedirectToAction("Edit");
            }

            if (db.Tarjetas.Any(t => t.IdUsuario == userId))
            {
                return RedirectToAction("Details");
            }

            if (!ModelState.IsValid)
            {
                return View(tarjeta);
            }

            db.Tarjetas.Add(tarjeta);
            db.SaveChanges();

            return RedirectToAction("index");
        }

        // GET: Tarjetas/Delete/5
        public ActionResult Delete()
        {
            string userId = User.Identity.GetUserId();

            var tarjeta = db.Tarjetas.Find(userId);

            if (tarjeta == null)
                return RedirectToAction("Create");

            return View(tarjeta);
        }

        // POST: Tarjetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            string userId = User.Identity.GetUserId();

            var tarjeta = db.Tarjetas.Find(userId);

            if (tarjeta != null)
            {
                db.Tarjetas.Remove(tarjeta);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Manage");
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