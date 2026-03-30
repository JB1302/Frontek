using Frontek.Models;
using Frontek_Full_Web_E_Commerce.Models;
using Frontek_Full_Web_E_Commerce.Models.Carrito;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Controllers.Carrito
{
    [Authorize]
    public class CarritoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var carrito = ObtenerCarrito();
            return View(carrito);
        }

        private List<CarritoItem> ObtenerCarrito()
        {
            if (Session["Carrito"] == null)
            {
                Session["Carrito"] = new List<CarritoItem>();
            }

            return (List<CarritoItem>)Session["Carrito"];
        }

        public ActionResult Agregar(int id)
        {
            var carrito = ObtenerCarrito();
            var producto = db.Producto.Find(id);

            if (producto == null)
                return HttpNotFound();

            var item = carrito.FirstOrDefault(p => p.ProductoId == id);

            if (item != null)
            {
                item.Cantidad++;
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    ProductoId = producto.Id,
                    NombreProducto = producto.NombreProducto,
                    Precio = producto.Precio,
                    Cantidad = 1,
                    Imagen = producto.Imagen1
                });
            }

            Session["Carrito"] = carrito;

            return RedirectToAction("Catalogo", "Producto");
        }

        public ActionResult Eliminar(int id)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(p => p.ProductoId == id);

            if (item != null)
            {
                carrito.Remove(item);
            }

            Session["Carrito"] = carrito;
            return RedirectToAction("Index");
        }

        public ActionResult Aumentar(int id)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(p => p.ProductoId == id);

            if (item != null)
            {
                item.Cantidad++;
            }

            Session["Carrito"] = carrito;
            return RedirectToAction("Index");
        }

        public ActionResult Disminuir(int id)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(p => p.ProductoId == id);

            if (item != null)
            {
                item.Cantidad--;

                if (item.Cantidad <= 0)
                {
                    carrito.Remove(item);
                }
            }

            Session["Carrito"] = carrito;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public ActionResult ProcesarCompra()
        {
            var carrito = ObtenerCarrito();

            if (carrito == null || !carrito.Any())
            {
                TempData["Error"] = "El carrito está vacío.";
                return RedirectToAction("Index");
            }

            var userId = User.Identity.GetUserId();
            var usuario = db.Users.FirstOrDefault(u => u.Id == userId);

            if (usuario == null)
            {
                TempData["Error"] = "No se pudo identificar el usuario.";
                return RedirectToAction("Index");
            }

            foreach (var item in carrito)
            {
                var producto = db.Producto.Find(item.ProductoId);

                if (producto == null)
                {
                    TempData["Error"] = "Uno de los productos ya no existe.";
                    return RedirectToAction("Index");
                }

                if (producto.Stock < item.Cantidad)
                {
                    TempData["Error"] = $"No hay suficiente stock para {producto.NombreProducto}. Disponible: {producto.Stock}.";
                    return RedirectToAction("Index");
                }
            }

            var orden = new Orden
            {
                NumeroOrden = "FRK-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                FechaCreacion = DateTime.Now,
                ClienteId = userId,
                IdUsuario = userId,
                NombreCliente = usuario.Nombre,
                EmailCliente = usuario.Email,
                DireccionEnvio = "",
                Ciudad = "",
                Pais = "Costa Rica",
                MetodoPago = "Pendiente",
                Estado = "Pendiente",
                FechaEntregaEstimada = DateTime.Now.AddDays(5),
                Total = carrito.Sum(x => x.Precio * x.Cantidad)
            };

            db.Ordenes.Add(orden);
            db.SaveChanges();

            foreach (var item in carrito)
            {
                var producto = db.Producto.Find(item.ProductoId);

                var detalle = new OrdenDetalle
                {
                    OrdenId = orden.OrdenId,
                    ProductoId = item.ProductoId,
                    NombreProducto = item.NombreProducto,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.Precio,
                    Subtotal = item.Precio * item.Cantidad
                };

                db.OrdenDetalles.Add(detalle);

                producto.Stock -= item.Cantidad;
            }

            db.SaveChanges();

            Session["Carrito"] = new List<CarritoItem>();

            TempData["Mensaje"] = "Tu orden fue creada correctamente.";
            return RedirectToAction("MisPedidos", "Ordenes");
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