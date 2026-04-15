using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Controllers
{
    public class ProductoController : Controller
    {
        private const int MaxImagenBytes = 5 * 1024 * 1024; // 5 MB por imagen
        private static readonly HashSet<string> ExtensionesPermitidas = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

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
            try
            {
                producto.Imagen1 = ProcesarImagenSiValida(ImagenFile1, nameof(ImagenFile1));
                producto.Imagen2 = ProcesarImagenSiValida(ImagenFile2, nameof(ImagenFile2));
                producto.Imagen3 = ProcesarImagenSiValida(ImagenFile3, nameof(ImagenFile3));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error procesando imágenes: " + ex.Message);
                return View(producto);
            }
            if (!ModelState.IsValid)
            {
                return View(producto);
            }


            try
            {
                _productoService.CrearProducto(producto);
                TempData["Mensaje"] = "Producto creado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "No se pudo guardar el producto. Verificá las imágenes e intentá nuevamente.");
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

            var actual = _productoService.ObtenerPorId(producto.Id);
            if (actual == null)
            {
                return HttpNotFound();
            }

            var nuevaImagen1 = ProcesarImagenSiValida(ImagenFile1, nameof(ImagenFile1));
            var nuevaImagen2 = ProcesarImagenSiValida(ImagenFile2, nameof(ImagenFile2));
            var nuevaImagen3 = ProcesarImagenSiValida(ImagenFile3, nameof(ImagenFile3));

            producto.Imagen1 = nuevaImagen1 ?? actual.Imagen1;
            producto.Imagen2 = nuevaImagen2 ?? actual.Imagen2;
            producto.Imagen3 = nuevaImagen3 ?? actual.Imagen3;

            if (!ModelState.IsValid)
            {
                return View(producto);
            }

            try
            {
                _productoService.EditarProducto(producto);
                TempData["Mensaje"] = "Producto actualizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "No se pudo actualizar el producto. Verificá las imágenes e intentá nuevamente.");
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

        private byte[] ProcesarImagenSiValida(HttpPostedFileBase archivo, string campo)
        {
            if (archivo == null || archivo.ContentLength == 0)
            {
                return null;
            }

            var extension = Path.GetExtension(archivo.FileName ?? string.Empty);
            if (!ExtensionesPermitidas.Contains(extension))
            {
                ModelState.AddModelError(campo, "Solo se permiten archivos JPG, JPEG o PNG.");
                return null;
            }

            if (archivo.ContentLength > MaxImagenBytes)
            {
                ModelState.AddModelError(campo, "La imagen supera el tamaño máximo permitido de 5 MB.");
                return null;
            }

            if (!EsTipoContenidoPermitido(archivo.ContentType))
            {
                ModelState.AddModelError(campo, "El archivo seleccionado no es una imagen válida.");
                return null;
            }

            using (var ms = new MemoryStream())
            {
                archivo.InputStream.CopyTo(ms);
                return ms.ToArray();
            }

        }

            private static bool EsTipoContenidoPermitido(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
            {
                return false;
            }

            var tiposPermitidos = new[] { "image/jpeg", "image/jpg", "image/png" };
            return tiposPermitidos.Contains(contentType, StringComparer.OrdinalIgnoreCase);
        }
    }
    
}