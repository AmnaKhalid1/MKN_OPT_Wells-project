using SUBSURF_MKN_WellOptimization_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SUBSURF_MKN_WellOptimization_MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


        }
        
            protected void Session_Start(Object sender, EventArgs e)
        {

            if (User.Identity.Name != null)
            {

                 Task.Run(() => DataAccess.GetRole(@User.Identity.Name)).Wait();
              //  _ = DataAccess.GetRole(@User.Identity.Name);
            }
            

        }
       

    }
}