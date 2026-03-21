using Frontek.Models;
using Frontek_Full_Web_E_Commerce.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce.Controllers
{

    [Authorize(Roles = "Administrador")]
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(); // aqui la devuelve vacia
        }
        public async Task<ActionResult> GetStats() // esto se llama por la ajax mediante js(talvez lo veamos esta semana que viene
        {
            var stats = new
            {
                totalUsuarios = await db.Users.CountAsync(),
                usuariosActivos = await db.Users.CountAsync(u => u.Activo),
                totalProductos = await db.Producto.CountAsync(),
                productosBajoStock = await db.Producto.CountAsync(p => p.Activo && p.Stock <= 5),
                totalOrdenes = await db.Ordenes.CountAsync(),
                ordenesEntregadas = await db.Ordenes.CountAsync(o => o.Estado == "Entregada"),
                ordenesPendientes = await db.Ordenes.CountAsync(o => o.Estado == "Pendiente"),
                ordenesEnCamino = await db.Ordenes.CountAsync(o => o.Estado == "En camino"),
                ordenesProcesando = await db.Ordenes.CountAsync(o => o.Estado == "Procesando"),
                ventasTotal = await db.Ordenes.SumAsync(o => (decimal?)o.Total) ?? 0, // es para evitar errores si no hay ordenes
                totalResenas = await db.Resenas.CountAsync(),
                resenasPendientes = await db.Resenas.CountAsync(r => r.Estado == 0)
            };
            return Json(stats, JsonRequestBehavior.AllowGet);
        }

        
        public async Task<ActionResult> GetUltimasOrdenes()// devuelve las 5 ordenes mas reciente 
        {
            var ordenes = await db.Ordenes
                .OrderByDescending(o => o.FechaCreacion)
                .Take(5)
                .Select(o => new
                {
                    numeroOrden = o.NumeroOrden,
                    nombreCliente = o.NombreCliente,
                    total = o.Total,
                    estado = o.Estado,
                    fechaCreacion = o.FechaCreacion.ToString()
                })
                .ToListAsync();

            return Json(ordenes, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetStockBajo() // devuelve los productos que esten en stock bajo a partir de 5 para abajo
        {
            var productos = await db.Producto
                .Where(p => p.Activo && p.Stock <= 5)
                .OrderBy(p => p.Stock)
                .Take(10)
                .Select(p => new
                {
                    nombreProducto = p.NombreProducto,
                    stock = p.Stock,
                    precio = p.Precio
                })
                .ToListAsync();

            return Json(productos, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}