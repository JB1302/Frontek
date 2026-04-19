using Frontek_Full_Web_E_Commerce.Application.Interfaces;
using Frontek_Full_Web_E_Commerce.Application.Services;
using Frontek_Full_Web_E_Commerce.Domain.Repositories;
using Frontek_Full_Web_E_Commerce.Infrastructure.Data;
using Frontek_Full_Web_E_Commerce.Infrastructure.Repositories;
using System;
using Unity;
using Unity.Lifetime;

namespace Frontek_Full_Web_E_Commerce.IoC
{
    public static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> container =
            new Lazy<IUnityContainer>(() =>
            {
                var unityContainer = new UnityContainer();
                RegisterTypes(unityContainer);
                return unityContainer;
            });

        public static IUnityContainer Container => container.Value;

        public static void RegisterTypes(IUnityContainer container)
        {
            
            container.RegisterType<ApplicationDbContext>(new TransientLifetimeManager());
            container.RegisterType<ICryptoService, CryptoService>();

            
            container.RegisterType<IProductoRepository, ProductoRepository>();
            container.RegisterType<IOrdenRepository, OrdenRepository>();
            container.RegisterType<IResenaRepository, ResenaRepository>();
            container.RegisterType<ITarjetaRepository, TarjetaRepository>();

           
            container.RegisterType<IProductoService, ProductoService>();
            container.RegisterType<IOrdenService, OrdenService>();
            container.RegisterType<IResenaService, ResenaService>();
            container.RegisterType<ITarjetaService, TarjetaService>();
            container.RegisterType<ICryptoService, CryptoService>();
        }
    }
}