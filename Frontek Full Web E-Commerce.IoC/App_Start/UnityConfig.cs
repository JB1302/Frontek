using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Application.Services;
using Frontek_Full_Web_E_Commerce.Domain.Repositories;
using Frontek_Full_Web_E_Commerce.Infrastructure.Data;
using Frontek_Full_Web_E_Commerce.Infrastructure.Repositories;
using System;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace Frontek_Full_Web_E_Commerce.IoC
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
        }

        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<ApplicationDbContext>();

            // Repositorios
            container.RegisterType<IProductoRepository, ProductoRepository>();
            container.RegisterType<IOrdenRepository, OrdenRepository>();
            container.RegisterType<IResenaRepository, ResenaRepository>();
            container.RegisterType<ITarjetaRepository, TarjetaRepository>();

            // Servicios
            /*
            container.RegisterType<IProductoService, ProductoService>();
            container.RegisterType<IOrdenService, OrdenService>();
            container.RegisterType<IResenaService, ResenaService>();
            container.RegisterType<ITarjetaService, TarjetaService>();*/

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }
    }
}