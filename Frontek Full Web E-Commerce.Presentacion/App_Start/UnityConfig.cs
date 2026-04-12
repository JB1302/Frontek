using Unity;

namespace Frontek_Full_Web_E_Commerce.Presentacion
{
    public static class UnityConfig
    {
        public static IUnityContainer Container => Frontek_Full_Web_E_Commerce.IoC.UnityConfig.Container;
    }
}