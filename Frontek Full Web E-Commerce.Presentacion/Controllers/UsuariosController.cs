using Frontek_Full_Web_E_Commerce.Presentacion.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var usuarios = db.Users.OrderBy(u => u.Nombre).ToList();
                return View(usuarios);
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new ApplicationDbContext())
            {
                var usuario = await db.Users.FindAsync(id);
                if (usuario == null)
                {
                    return HttpNotFound();
                }

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var rolActual = (await userManager.GetRolesAsync(usuario.Id)).FirstOrDefault();

                ViewBag.RolesDisponibles = new SelectList(
                    roleManager.Roles.OrderBy(r => r.Name).ToList(),
                    "Name",
                    "Name",
                    rolActual);

                return View(usuario);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, string nombre, string email, bool activo, string rolSeleccionado)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new ApplicationDbContext())
            {
                var usuario = await db.Users.FindAsync(id);
                if (usuario == null)
                {
                    return HttpNotFound();
                }

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    ModelState.AddModelError("Nombre", "El nombre es obligatorio.");
                }

                if (string.IsNullOrWhiteSpace(email))
                {
                    ModelState.AddModelError("Email", "El correo es obligatorio.");
                }

                if (!ModelState.IsValid)
                {
                    var roleManagerReload = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                    ViewBag.RolesDisponibles = new SelectList(
                        roleManagerReload.Roles.OrderBy(r => r.Name).ToList(),
                        "Name",
                        "Name",
                        rolSeleccionado);

                    usuario.Nombre = nombre;
                    usuario.Email = email;
                    usuario.UserName = email;
                    usuario.Activo = activo;

                    return View(usuario);
                }

                usuario.Nombre = nombre;
                usuario.Email = email;
                usuario.UserName = email;
                usuario.Activo = activo;

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var rolesActuales = await userManager.GetRolesAsync(usuario.Id);
                if (rolesActuales.Any())
                {
                    await userManager.RemoveFromRolesAsync(usuario.Id, rolesActuales.ToArray());
                }

                if (!string.IsNullOrWhiteSpace(rolSeleccionado))
                {
                    await userManager.AddToRoleAsync(usuario.Id, rolSeleccionado);
                }

                await userManager.UpdateAsync(usuario);

                TempData["Mensaje"] = "Usuario actualizado correctamente.";
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new ApplicationDbContext())
            {
                var usuario = await db.Users.FindAsync(id);
                if (usuario == null)
                {
                    return HttpNotFound();
                }

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ViewBag.Roles = await userManager.GetRolesAsync(usuario.Id);

                return View(usuario);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var usuario = await userManager.FindByIdAsync(id);
                if (usuario == null)
                {
                    return HttpNotFound();
                }

                var roles = await userManager.GetRolesAsync(usuario.Id);
                if (roles.Any())
                {
                    await userManager.RemoveFromRolesAsync(usuario.Id, roles.ToArray());
                }

                var result = await userManager.DeleteAsync(usuario);
                if (!result.Succeeded)
                {
                    TempData["Error"] = string.Join(" | ", result.Errors);
                    return RedirectToAction("Index");
                }

                TempData["Mensaje"] = "Usuario eliminado correctamente.";
                return RedirectToAction("Index");
            }
        }
    }
}