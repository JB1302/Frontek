using Frontek_Full_Web_E_Commerce.Models;
using Frontek_Full_Web_E_Commerce.Models.Utils;
using System;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Controllers
{
    public class RegisterController : Controller
    {
        private FrontekController db = new FrontekController();

        // GET: Register
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Email,Contrasenia")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Contrasenia = SecretService.HashSecret(usuario.Contrasenia);
                // guardar en base de datos
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Register");
        }
    }
}