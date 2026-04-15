using System.Linq;
using System.Web.Mvc;

using Unity.AspNet.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Frontek_Full_Web_E_Commerce.IoC.UnityMvcActivator), nameof(Frontek_Full_Web_E_Commerce.IoC.UnityMvcActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Frontek_Full_Web_E_Commerce.IoC.UnityMvcActivator), nameof(Frontek_Full_Web_E_Commerce.IoC.UnityMvcActivator.Shutdown))]

namespace Frontek_Full_Web_E_Commerce.IoC
{
    /// <summary>
    /// Provides the bootstrapping for integrating Unity with ASP.NET MVC.
    /// </summary>
    public static class UnityMvcActivator
    {
        /// <summary>
        /// Integrates Unity when the application starts.
        /// </summary>
        public static void Start() 
        {
            var defaultFilterProvider = FilterProviders.Providers
            .OfType<FilterAttributeFilterProvider>()
            .FirstOrDefault();

            if (defaultFilterProvider != null)
            {
                FilterProviders.Providers.Remove(defaultFilterProvider);
            }
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(UnityConfig.Container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityConfig.Container));

            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            // Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>
        /// Disposes the Unity container when the application is shut down.
        /// </summary>
        public static void Shutdown()
        {
            UnityConfig.Container.Dispose();
        }
    }
}