using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocialEngineeringReporting
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "List",
                url: "List",
                defaults: new {controller = "SocialEngineering", action = "List"});

            routes.MapRoute(
                name: "Reporting",
                url: "Reporting",
                defaults: new { controller = "SocialEngineering", action = "Reporting" });

            routes.MapRoute(
                name: "GetRecentReport",
                url: "GetRecentReport",
                defaults: new { controller = "SocialEngineering", action = "GetRecentReport" });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "SocialEngineering", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
