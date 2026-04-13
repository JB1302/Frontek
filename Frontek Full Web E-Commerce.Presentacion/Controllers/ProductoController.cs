using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Domain.Entities;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public ActionResult Index()
        {
            var productos = _productoService.ListarProductos();
            return View(productos);
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = _productoService.ObtenerPorId(id.Value);
            if (producto == null)
            {
                return HttpNotFound();
            }

            return View(producto);
        }

        [Authorize(Roles = "Administrador,Vendedor")]
        public ActionResult Create()
        {
            return View(new Producto { Activo = true });
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Vendedor")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Producto producto, HttpPostedFileBase ImagenFile1, HttpPostedFileBase ImagenFile2, HttpPostedFileBase ImagenFile3)
        {
            if (!ModelState.IsValid)
            {
                return View(producto);
            }

            producto.Imagen1 = LeerImagen(ImagenFile1);
            producto.Imagen2 = LeerImagen(ImagenFile2);
            producto.Imagen3 = LeerImagen(ImagenFile3);

            try
            {
                _productoService.CrearProducto(producto);
                TempData["Mensaje"] = "Producto creado correctamente.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(producto);
            }
        }

        [Authorize(Roles = "Administrador,Vendedor")]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = _productoService.ObtenerPorId(id.Value);
            if (producto == null)
            {
                return HttpNotFound();
            }

            return View(producto);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Vendedor")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Producto producto, HttpPostedFileBase ImagenFile1, HttpPostedFileBase ImagenFile2, HttpPostedFileBase ImagenFile3)
        {
            if (!ModelState.IsValid)
            {
                return View(producto);
            }

            var actual = _productoService.ObtenerPorId(producto.Id);
            if (actual == null)
            {
                return HttpNotFound();
            }

            producto.Imagen1 = LeerImagen(ImagenFile1) ?? actual.Imagen1;
            producto.Imagen2 = LeerImagen(ImagenFile2) ?? actual.Imagen2;
            producto.Imagen3 = LeerImagen(ImagenFile3) ?? actual.Imagen3;

            try
            {
                _productoService.EditarProducto(producto);
                TempData["Mensaje"] = "Producto actualizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(producto);
            }
        }

        [Authorize(Roles = "Administrador,Vendedor")]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var producto = _productoService.ObtenerPorId(id.Value);
            if (producto == null)
            {
                return HttpNotFound();
            }

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador,Vendedor")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _productoService.EliminarProducto(id);
                TempData["Mensaje"] = "Producto eliminado correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Mensaje"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Catalogo()
        {
            var productos = _productoService.ListarProductosActivos();
            return View("Index", productos);
        }

        private static byte[] LeerImagen(HttpPostedFileBase archivo)
        {
            if (archivo == null || archivo.ContentLength == 0)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                archivo.InputStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}