using Frontek.Models;
using Frontek_Full_Web_E_Commerce.Models.Carrito;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            return RedirectToAction("Index");
        }


    }
}
