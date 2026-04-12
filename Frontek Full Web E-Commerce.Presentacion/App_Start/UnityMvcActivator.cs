using System.Linq;
using System.Web.Mvc;
using Unity.AspNet.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(
    typeof(Frontek_Full_Web_E_Commerce.Presentacion.UnityMvcActivator),
    nameof(Frontek_Full_Web_E_Commerce.Presentacion.UnityMvcActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(
    typeof(Frontek_Full_Web_E_Commerce.Presentacion.UnityMvcActivator),
    nameof(Frontek_Full_Web_E_Commerce.Presentacion.UnityMvcActivator.Shutdown))]

namespace Frontek_Full_Web_E_Commerce.Presentacion
{
    public static class UnityMvcActivator
    {
        public static void Start()
        {
            var container = Frontek_Full_Web_E_Commerce.IoC.UnityConfig.Container;

            var defaultFilterProvider = FilterProviders.Providers
                .OfType<FilterAttributeFilterProvider>()
                .FirstOrDefault();

            if (defaultFilterProvider != null)
            {
                FilterProviders.Providers.Remove(defaultFilterProvider);
            }

            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static void Shutdown()
        {
            Frontek_Full_Web_E_Commerce.IoC.UnityConfig.Container.Dispose();
        }
    }
}