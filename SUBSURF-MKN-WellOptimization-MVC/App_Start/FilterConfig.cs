using System.Web;
using System.Web.Mvc;

namespace SUBSURF_MKN_WellOptimization_MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
