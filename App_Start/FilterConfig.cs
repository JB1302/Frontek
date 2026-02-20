using System.Web;
using System.Web.Mvc;

namespace Frontek_Full_Web_E_Commerce
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
