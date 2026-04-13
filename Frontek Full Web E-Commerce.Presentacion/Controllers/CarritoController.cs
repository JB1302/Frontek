using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Domain.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Presentacion.Controllers
{
    [Authorize]
    public class CarritoController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly IOrdenService _ordenService;

        public CarritoController(IProductoService productoService, IOrdenService ordenService)
        {
            _productoService = productoService;
            _ordenService = ordenService;
        }

        public ActionResult Index()
        {
            return View(ObtenerCarrito());
        }

        public ActionResult Agregar(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var producto = _productoService.ObtenerPorId(id.Value);
            if (producto == null)
            {
                return HttpNotFound();
            }

            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(p => p.ProductoId == id.Value);

            if (item == null)
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
            else
            {
                item.AumentarCantidad(1);
            }

            Session["Carrito"] = carrito;
            TempData["Mensaje"] = "Producto agregado al carrito.";
            return RedirectToAction("Catalogo", "Producto");
        }

        public ActionResult Aumentar(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(i => i.ProductoId == id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }

            item.AumentarCantidad(1);
            Session["Carrito"] = carrito;
            return RedirectToAction("Index");
        }

        public ActionResult Disminuir(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(i => i.ProductoId == id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }

            item.Cantidad--;
            if (item.Cantidad <= 0)
            {
                carrito.Remove(item);
            }

            Session["Carrito"] = carrito;
            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(i => i.ProductoId == id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }

            carrito.Remove(item);
            Session["Carrito"] = carrito;
            TempData["Mensaje"] = "Producto eliminado del carrito.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        [ValidateAntiForgeryToken]
        public ActionResult ProcesarCompra()
        {
            var carrito = ObtenerCarrito();
            if (!carrito.Any())
            {
                TempData["Error"] = "El carrito está vacío.";
                return RedirectToAction("Index");
            }

            foreach (var item in carrito)
            {
                var producto = _productoService.ObtenerPorId(item.ProductoId);
                if (producto == null)
                {
                    TempData["Error"] = "Uno de los productos ya no existe.";
                    return RedirectToAction("Index");
                }

                if (producto.Stock < item.Cantidad)
                {
                    TempData["Error"] = $"No hay suficiente stock para {producto.NombreProducto}.";
                    return RedirectToAction("Index");
                }
            }

            var userId = User.Identity.GetUserId();
            var orden = new Orden
            {
                IdUsuario = userId,
                ClienteId = userId,
                NombreCliente = User.Identity.GetUserName() ?? "Cliente",
                EmailCliente = User.Identity.GetUserName() ?? string.Empty,
                DireccionEnvio = string.Empty,
                Ciudad = string.Empty,
                Pais = string.Empty,
                MetodoPago = "Pendiente",
                Estado = "Pendiente",
                FechaEntregaEstimada = DateTime.Now.AddDays(5),
                Total = carrito.Sum(x => x.Precio * x.Cantidad),
                Detalles = carrito.Select(item => new OrdenDetalle
                {
                    ProductoId = item.ProductoId,
                    NombreProducto = item.NombreProducto,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.Precio,
                    Subtotal = item.Precio * item.Cantidad
                }).ToList()
            };

            _ordenService.CrearOrden(orden);
            Session["Carrito"] = new List<CarritoItem>();
            TempData["Mensaje"] = "Tu orden fue creada correctamente.";

            return RedirectToAction("MisPedidos", "Ordenes");
        }

        private List<CarritoItem> ObtenerCarrito()
        {
            if (!(Session["Carrito"] is List<CarritoItem> carrito))
            {
                carrito = new List<CarritoItem>();
                Session["Carrito"] = carrito;
            }

            return carrito;
        }
    }
}