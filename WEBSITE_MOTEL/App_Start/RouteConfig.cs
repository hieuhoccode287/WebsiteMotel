using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WEBSITE_MOTEL
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           

          
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "StartPage", action = "StartPage", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "VnPayReturn",
                url: "Cart/VnPayReturn",
                defaults: new { controller = "Cart", action = "VnPayReturn" }
            );

        }
    }
}
