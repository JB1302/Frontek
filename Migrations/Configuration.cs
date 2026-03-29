namespace Frontek_Full_Web_E_Commerce.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using Frontek.Models;
    using Frontek_Full_Web_E_Commerce.Models;
    using Frontek_Full_Web_E_Commerce.Models.Tarjeta;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<Frontek.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Frontek.Models.ApplicationDbContext context)
        {
            SeedRoles(context);
            SeedUsers(context);
            context.SaveChanges();

            SeedClaims(context);
            context.SaveChanges();

            SeedProducto(context);
            context.SaveChanges();

            SeedTarjetas(context);
            context.SaveChanges();

            SeedResenas(context);
            context.SaveChanges();

            SeedOrdenes(context);
            context.SaveChanges();
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string[] roles = { "Cliente", "Administrador", "Vendedor" };

            foreach (var roleName in roles)
            {
                if (!roleManager.RoleExists(roleName))
                {
                    roleManager.Create(new IdentityRole(roleName));
                }
            }
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            EnsureUser(userManager, "admin@frontek.com", "Administrador General", "Administrador");
            EnsureUser(userManager, "vendedor@frontek.com", "Vendedor Principal", "Vendedor");
            EnsureUser(userManager, "cliente1@frontek.com", "Laura Ramírez", "Cliente");
            EnsureUser(userManager, "cliente2@frontek.com", "Carlos Méndez", "Cliente");
            EnsureUser(userManager, "cliente3@frontek.com", "Sofía Vargas", "Cliente");
        }

        private void EnsureUser(UserManager<ApplicationUser> userManager, string email, string nombre, string rol)
        {
            var user = userManager.FindByEmail(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    Nombre = nombre,
                    Activo = true,
                    UltimaConexion = DateTime.Now,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                };

                var result = userManager.Create(user, "Clave123");
                if (!result.Succeeded)
                {
                    throw new Exception("No se pudo crear el usuario " + email + ": " + string.Join("; ", result.Errors));
                }
            }

            if (!userManager.IsInRole(user.Id, rol))
            {
                userManager.AddToRole(user.Id, rol);
            }
        }

        private void SeedClaims(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var users = context.Users.ToList();

            foreach (var user in users)
            {
                var claims = userManager.GetClaims(user.Id);
                if (!claims.Any(c => c.Type == "seeded"))
                {
                    userManager.AddClaim(user.Id, new Claim("seeded", "true"));
                }
            }
        }

        private void SeedProducto(ApplicationDbContext context)
        {
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;

            AddOrUpdateProducto(context, "Teclado Mecánico RGB", 34990m, 18, true, DateTime.Now.AddDays(-25), DateTime.Now.AddDays(-3),
                "https://commons.wikimedia.org/wiki/Special:Redirect/file/Mechanical_Keyboard.jpg");

            AddOrUpdateProducto(context, "Mouse Gamer Pro", 21990m, 25, true, DateTime.Now.AddDays(-22), DateTime.Now.AddDays(-2),
                "https://commons.wikimedia.org/wiki/Special:Redirect/file/Gamers_Mouse.jpg");

            AddOrUpdateProducto(context, "Monitor Full HD 24", 119990m, 10, true, DateTime.Now.AddDays(-20), DateTime.Now.AddDays(-1),
                "https://commons.wikimedia.org/wiki/Special:Redirect/file/Computer_Screen_Monitor.jpg");

            AddOrUpdateProducto(context, "Headset Gaming USB", 27990m, 14, true, DateTime.Now.AddDays(-18), DateTime.Now.AddDays(-1),
                "https://commons.wikimedia.org/wiki/Special:Redirect/file/Headset_computer.png");

            AddOrUpdateProducto(context, "Laptop Gamer 15", 649990m, 6, true, DateTime.Now.AddDays(-15), DateTime.Now,
                "https://commons.wikimedia.org/wiki/Special:Redirect/file/Laptop_computer_monitor_(Unsplash).jpg");
        }

        private void AddOrUpdateProducto(ApplicationDbContext context, string nombre, decimal precio, int stock, bool activo,
            DateTime fechaIngreso, DateTime? fechaMod, string imageUrl)
        {
            var producto = context.Producto.FirstOrDefault(p => p.NombreProducto == nombre);
            var imagen = DescargarImagen(imageUrl);

            if (producto == null)
            {
                context.Producto.Add(new Producto
                {
                    NombreProducto = nombre,
                    Precio = precio,
                    Stock = stock,
                    Activo = activo,
                    FechaIngreso = fechaIngreso,
                    FechaMod = fechaMod,
                    Imagen1 = imagen
                });
            }
            else
            {
                producto.Precio = precio;
                producto.Stock = stock;
                producto.Activo = activo;
                producto.FechaIngreso = fechaIngreso;
                producto.FechaMod = fechaMod;
                if (imagen != null && imagen.Length > 0)
                {
                    producto.Imagen1 = imagen;
                }
            }
        }

        private void SeedTarjetas(ApplicationDbContext context)
        {
            var users = context.Users.OrderBy(u => u.Email).ToList();

            var datos = new Dictionary<string, Tuple<string, DateTime, string, string>>
            {
                { "admin@frontek.com", Tuple.Create("4111111111111111", new DateTime(2028, 12, 1), "321", "Administrador General") },
                { "vendedor@frontek.com", Tuple.Create("4000056655665556", new DateTime(2029, 10, 1), "456", "Vendedor Principal") },
                { "cliente1@frontek.com", Tuple.Create("5555555555554444", new DateTime(2027, 8, 1), "123", "Laura Ramírez") },
                { "cliente2@frontek.com", Tuple.Create("5200828282828210", new DateTime(2028, 6, 1), "654", "Carlos Méndez") },
                { "cliente3@frontek.com", Tuple.Create("4012888888881881", new DateTime(2029, 4, 1), "987", "Sofía Vargas") }
            };

            foreach (var user in users)
            {
                if (!datos.ContainsKey(user.Email))
                    continue;

                var seed = datos[user.Email];
                var actual = context.Tarjetas.Find(user.Id);

                if (actual == null)
                {
                    var tarjeta = new Tarjeta
                    {
                        IdUsuario = user.Id,
                        FechaVencimiento = seed.Item2,
                        Propietario = seed.Item4
                    };

                    tarjeta.NumeroTarjeta = seed.Item1;
                    tarjeta.CCV = seed.Item3;

                    context.Tarjetas.Add(tarjeta);
                }
                else
                {
                    actual.FechaVencimiento = seed.Item2;
                    actual.Propietario = seed.Item4;
                    actual.NumeroTarjeta = seed.Item1;
                    actual.CCV = seed.Item3;
                }
            }
        }

        private void SeedResenas(ApplicationDbContext context)
        {
            if (context.Resenas.Any())
                return;

            var rolCliente = context.Roles.FirstOrDefault(r => r.Name == "Cliente");
            if (rolCliente == null)
                return;

            var clientes = context.Users
                .Where(u => u.Roles.Any(r => r.RoleId == rolCliente.Id))
                .OrderBy(u => u.Email)
                .ToList();

            var Producto = context.Producto.OrderBy(p => p.Id).ToList();

            if (clientes.Count < 3 || Producto.Count < 5)
                return;

            var resenas = new List<Resena>
            {
                new Resena { ProductoId = Producto[0].Id, UsuarioId = clientes[0].Id, Titulo = "Muy buen teclado", Cuerpo = "Excelente tacto de teclas, buena iluminación y construcción sólida.", Calificacion = 5, Estado = 1, FechaCreacion = DateTime.Now.AddDays(-8) },
                new Resena { ProductoId = Producto[1].Id, UsuarioId = clientes[1].Id, Titulo = "Buen mouse", Cuerpo = "Cómodo para sesiones largas y con muy buena precisión.", Calificacion = 4, Estado = 1, FechaCreacion = DateTime.Now.AddDays(-7) },
                new Resena { ProductoId = Producto[2].Id, UsuarioId = clientes[2].Id, Titulo = "Pantalla nítida", Cuerpo = "Buena tasa de refresco y colores agradables.", Calificacion = 5, Estado = 1, FechaCreacion = DateTime.Now.AddDays(-6) },
                new Resena { ProductoId = Producto[3].Id, UsuarioId = clientes[0].Id, Titulo = "Audio claro", Cuerpo = "Micrófono decente y sonido envolvente para juegos.", Calificacion = 4, Estado = 1, FechaCreacion = DateTime.Now.AddDays(-5) },
                new Resena { ProductoId = Producto[4].Id, UsuarioId = clientes[1].Id, Titulo = "Buen rendimiento", Cuerpo = "La laptop responde muy bien para trabajo y gaming casual.", Calificacion = 5, Estado = 1, FechaCreacion = DateTime.Now.AddDays(-4) }
            };

            foreach (var r in resenas)
            {
                bool existe = context.Resenas.Any(x =>
                    x.ProductoId == r.ProductoId &&
                    x.UsuarioId == r.UsuarioId &&
                    x.Titulo == r.Titulo);

                if (!existe)
                {
                    context.Resenas.Add(r);
                }
            }
        }

        private void SeedOrdenes(ApplicationDbContext context)
        {
            if (context.Ordenes.Any())
                return;

            var rolCliente = context.Roles.FirstOrDefault(r => r.Name == "Cliente");
            if (rolCliente == null)
                return;

            var clientes = context.Users
                .Where(u => u.Roles.Any(r => r.RoleId == rolCliente.Id))
                .OrderBy(u => u.Email)
                .ToList();

            var Producto = context.Producto.OrderBy(p => p.Id).ToList();

            if (clientes.Count < 3 || Producto.Count < 5)
                return;

            var ordenes = new List<Orden>
            {
                new Orden { NumeroOrden = "FRK-1001", FechaCreacion = DateTime.Now.AddDays(-10), ClienteId = clientes[0].Id, NombreCliente = clientes[0].Nombre, EmailCliente = clientes[0].Email, DireccionEnvio = "San Pedro, Montes de Oca", Ciudad = "San José", Pais = "Costa Rica", Total = 56980m, MetodoPago = "Tarjeta", Estado = "Entregada", FechaEntregaEstimada = DateTime.Now.AddDays(-7), IdUsuario = clientes[0].Id },
                new Orden { NumeroOrden = "FRK-1002", FechaCreacion = DateTime.Now.AddDays(-9), ClienteId = clientes[1].Id, NombreCliente = clientes[1].Nombre, EmailCliente = clientes[1].Email, DireccionEnvio = "Heredia Centro", Ciudad = "Heredia", Pais = "Costa Rica", Total = 119990m, MetodoPago = "Tarjeta", Estado = "En camino", FechaEntregaEstimada = DateTime.Now.AddDays(-2), IdUsuario = clientes[1].Id },
                new Orden { NumeroOrden = "FRK-1003", FechaCreacion = DateTime.Now.AddDays(-8), ClienteId = clientes[2].Id, NombreCliente = clientes[2].Nombre, EmailCliente = clientes[2].Email, DireccionEnvio = "Cartago Centro", Ciudad = "Cartago", Pais = "Costa Rica", Total = 27990m, MetodoPago = "Tarjeta", Estado = "Procesando", FechaEntregaEstimada = DateTime.Now.AddDays(2), IdUsuario = clientes[2].Id },
                new Orden { NumeroOrden = "FRK-1004", FechaCreacion = DateTime.Now.AddDays(-6), ClienteId = clientes[0].Id, NombreCliente = clientes[0].Nombre, EmailCliente = clientes[0].Email, DireccionEnvio = "Curridabat", Ciudad = "San José", Pais = "Costa Rica", Total = 21990m, MetodoPago = "Tarjeta", Estado = "Entregada", FechaEntregaEstimada = DateTime.Now.AddDays(-3), IdUsuario = clientes[0].Id },
                new Orden { NumeroOrden = "FRK-1005", FechaCreacion = DateTime.Now.AddDays(-4), ClienteId = clientes[1].Id, NombreCliente = clientes[1].Nombre, EmailCliente = clientes[1].Email, DireccionEnvio = "Alajuela Centro", Ciudad = "Alajuela", Pais = "Costa Rica", Total = 649990m, MetodoPago = "Tarjeta", Estado = "Pendiente", FechaEntregaEstimada = DateTime.Now.AddDays(4), IdUsuario = clientes[1].Id }
            };

            foreach (var o in ordenes)
            {
                if (!context.Ordenes.Any(x => x.NumeroOrden == o.NumeroOrden))
                {
                    context.Ordenes.Add(o);
                }
            }

            context.SaveChanges();

            var orden1 = context.Ordenes.First(o => o.NumeroOrden == "FRK-1001");
            var orden2 = context.Ordenes.First(o => o.NumeroOrden == "FRK-1002");
            var orden3 = context.Ordenes.First(o => o.NumeroOrden == "FRK-1003");
            var orden4 = context.Ordenes.First(o => o.NumeroOrden == "FRK-1004");
            var orden5 = context.Ordenes.First(o => o.NumeroOrden == "FRK-1005");

            var detalles = new List<OrdenDetalle>
            {
                new OrdenDetalle { OrdenId = orden1.OrdenId, NombreProducto = Producto[0].NombreProducto, SKU = "KB-001", Cantidad = 1, PrecioUnitario = Producto[0].Precio, Subtotal = Producto[0].Precio, Garantia = "12 meses", ProductoId = Producto[0].Id },
                new OrdenDetalle { OrdenId = orden1.OrdenId, NombreProducto = Producto[1].NombreProducto, SKU = "MS-001", Cantidad = 1, PrecioUnitario = Producto[1].Precio, Subtotal = Producto[1].Precio, Garantia = "12 meses", ProductoId = Producto[1].Id },
                new OrdenDetalle { OrdenId = orden2.OrdenId, NombreProducto = Producto[2].NombreProducto, SKU = "MN-001", Cantidad = 1, PrecioUnitario = Producto[2].Precio, Subtotal = Producto[2].Precio, Garantia = "24 meses", ProductoId = Producto[2].Id },
                new OrdenDetalle { OrdenId = orden3.OrdenId, NombreProducto = Producto[3].NombreProducto, SKU = "HS-001", Cantidad = 1, PrecioUnitario = Producto[3].Precio, Subtotal = Producto[3].Precio, Garantia = "12 meses", ProductoId = Producto[3].Id },
                new OrdenDetalle { OrdenId = orden4.OrdenId, NombreProducto = Producto[1].NombreProducto, SKU = "MS-001", Cantidad = 1, PrecioUnitario = Producto[1].Precio, Subtotal = Producto[1].Precio, Garantia = "12 meses", ProductoId = Producto[1].Id },
                new OrdenDetalle { OrdenId = orden5.OrdenId, NombreProducto = Producto[4].NombreProducto, SKU = "LP-001", Cantidad = 1, PrecioUnitario = Producto[4].Precio, Subtotal = Producto[4].Precio, Garantia = "24 meses", ProductoId = Producto[4].Id }
            };

            foreach (var detalle in detalles)
            {
                bool existe = context.OrdenDetalles.Any(d =>
                    d.OrdenId == detalle.OrdenId &&
                    d.ProductoId == detalle.ProductoId &&
                    d.SKU == detalle.SKU);

                if (!existe)
                {
                    context.OrdenDetalles.Add(detalle);
                }
            }
        }

        private byte[] DescargarImagen(string url)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    return webClient.DownloadData(url);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}