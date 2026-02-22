using Frontek_Full_Web_E_Commerce.Models;
using Frontek_Full_Web_E_Commerce.Models.Utils;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Controllers
{
    public class LoginController : Controller
    {
        private FrontekController db = new FrontekController();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Verificar(string email, string contrasenia)
        {
            var usuario = db.Usuarios.FirstOrDefault(u => u.Email == email);

            if (usuario != null)
            {

                bool esValido = SecretService.VerifySecret(contrasenia, usuario.Contrasenia);

                if (esValido)
                {
                    Session["UsuarioId"] = usuario.Id;
                    Session["Nombre"] = usuario.Nombre;

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

    }
}